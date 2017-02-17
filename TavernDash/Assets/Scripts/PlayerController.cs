using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : Pickable
{

    // properties
    [Header("Components")]
    [SerializeField]
    private Transform _bodyTransform;
    private Transform _transform;
    [SerializeField]
    private Animator animator;
    private Dialogue dialogue;

    // states
    public enum States
    {
        Moving,
		GetHit
    }
    private States previousState;
    private States currentState;

    public delegate void UpdateState();
    UpdateState updateState;

	private float timeInState = 0f;

    // lerps
    Vector3 lerpInitialPos;
    Vector3 lerpInitalRot;

    [Header("Movement")]
    // movement
    [SerializeField]
    private float rotationSpeed = 50f;
    [SerializeField]
    private float moveSpeed = 2f;

    float currentSpeed = 0f;
    [SerializeField]
    private float acceleration = 1f;
    [SerializeField]
    private float collisionDistance = 1f;
    [SerializeField]
    private LayerMask collisionLayerMask;

	// pickable
	private Pickable pickable;
	float pickableTimer = 0f;
	[SerializeField] private float pickableRate = 0.5f;

	Vector3 initRot = Vector3.zero;
	float lerpTimer = 0f;
	public float runMoveSpeed = 1f;
	public float runAnimSpeed = 1.5f;
	float targetAnimSpeed = 0f;
	float currentAnimSpeed = 0f;

	[SerializeField]
	private GameObject knockedOutFeedbackPrefab;
	private GameObject knockedOutFeedbackObj;

	// input
	[SerializeField] private string input_Action 		= "";
	[SerializeField] private string input_Run 			= "";
	[SerializeField] private string input_Hit 			= "";
	[SerializeField] private string input_Horizontal 	= "";
	[SerializeField] private string input_Vertical 		= "";

    // Use this for initialization
    void Start()
    {
		Init ();

        _transform = this.transform;

        dialogue = GetComponentInChildren<Dialogue>();

        ChangeState(States.Moving);

		CreateFeedbacks ();

    }

	private void CreateFeedbacks () {
		knockedOutFeedbackObj = UIManager.Instance.CreateElement (knockedOutFeedbackPrefab, UIManager.CanvasType.Patience);
		knockedOutFeedbackObj.SetActive (false);
	}
    // Update is called once per frame
    void FixedUpdate()
    {
        if (updateState != null)
        {
            updateState();
            timeInState += Time.deltaTime;
        }
    }

	#region go to table
	private void Moving_Start()
	{
		//
	}

    private void Moving_Update()
    {
		ApplyMovement ();

		if ( Input.GetButtonDown(input_Hit) ) {
			if ( pickable != null && timeInState > 1 ) {
				pickable.Throw (BodyTransform.forward);

				timeInState = 0f;

				pickable = null;

			}
		}

		if ( Input.GetButtonDown(input_Action)  ) {
			if ( pickable != null && timeInState > 1 ) {
				pickable.Drop ();

				timeInState = 0f;

				pickable = null;

			}
		}
    }

    private void Moving_Exit()
    {
        //
    }

    private void ApplyMovement ()
    {
		Vector3 direction = Camera.main.transform.TransformDirection (InputDirection);
		direction.y = 0f;

		float targetSpeed = moveSpeed;
		if ( Input.GetButton(input_Run) )
			targetSpeed = runMoveSpeed;
		if ( PressingInput == false )
			targetSpeed = 0f;

		if (Vector3.Dot (BodyTransform.forward, direction) > -0.9f) {
			BodyTransform.forward = Vector3.MoveTowards (BodyTransform.forward, direction, rotationSpeed * Time.deltaTime);
		} else {
			BodyTransform.forward = direction;
		}

		currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

		GetTransform.Translate(BodyTransform.forward * currentSpeed * Time.deltaTime);

    }
    #endregion

	#region get hit
	private void GetHit_Start () {
		Throw ( -BodyTransform.forward );

		knockedOutFeedbackObj.SetActive (true);
	}
	private void GetHit_Update () {

		UIManager.Instance.Place (knockedOutFeedbackObj.GetComponent<RectTransform>(), Vector3.up * 1.2f);

		if ( timeInState > 4 ) {
			ChangeState (States.Moving);
		}
	}
	private void GetHit_Exit () {
		knockedOutFeedbackObj.SetActive (false);
		Reset ();
	}
	#endregion

    private Vector3 InputDirection
    {
        get
        {
            return new Vector3( Input.GetAxis(input_Horizontal) , 0f , Input.GetAxis (input_Vertical) );
        }
    }
    private bool PressingInput
    {
        get {
            return Input.GetAxis (input_Horizontal) != 0 || Input.GetAxis(input_Vertical) != 0;
        }
    }

    #region state machine
    public void ChangeState(States newState)
    {
        previousState = currentState;
        currentState = newState;

        lerpInitalRot = BodyTransform.forward;
        lerpInitialPos = GetTransform.position;

        timeInState = 0f;

        ExitState();
        EnterState();
    }
    private void EnterState()
    {
        switch (currentState)
        {
        case States.Moving:
            updateState = Moving_Update;
            Moving_Start();
            break;
		case States.GetHit:
			updateState = GetHit_Update;
			GetHit_Start ();
			break;
        }
    }
    private void ExitState()
    {
        switch (previousState)
        {
            case States.Moving:
                Moving_Exit();
                break;
			case States.GetHit:
				GetHit_Exit ();
				break;
        }
    }
    #endregion

    #region properties
    public Transform GetTransform
    {
        get
        {
            return _transform;
        }
    }
    public Animator GetAnimator
    {
        get
        {
            return animator;
        }
    }
    public Transform BodyTransform
    {
        get
        {
            return _bodyTransform;
        }
    }
    public States CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
        }
    }
    #endregion

	public Pickable Pickable {
		get {
			return pickable;
		}
		set {
			pickable = value;
		}
	}

	void OnCollisionEnter ( Collision c ) {

		if ( c.gameObject.tag == "Pickable") {

			if ( c.gameObject.GetComponent<Pickable>().Constrained == false ) {
				if (c.relativeVelocity.magnitude > 5) {

					Vector3 dir = (c.transform.position - GetTransform.position).normalized;
					dir.y = 0f;

					BodyTransform.forward = dir;
					ChangeState (States.GetHit);
				}
			}
		}
	}

	public float PickableTimer {
		get {
			return pickableTimer;
		}
	}

	public float TimeInState {
		get {
			return timeInState;
		}
		set {
			timeInState = value;
		}
	}

	public string Input_Action {
		get {
			return input_Action;
		}
	}
}
