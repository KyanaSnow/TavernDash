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

	Vector3 initRot = Vector3.zero;
	float lerpTimer = 0f;
	public float runMoveSpeed = 1f;
	public float runAnimSpeed = 1.5f;
	float targetAnimSpeed = 0f;
	float currentAnimSpeed = 0f;

    // Use this for initialization
    void Start()
    {
		Init ();

        _transform = this.transform;

        dialogue = GetComponentInChildren<Dialogue>();

        ChangeState(States.Moving);

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

		if ( Input.GetKeyDown(KeyCode.P) ) {
			if ( pickable != null && timeInState > 1 ) {
				pickable.Throw (BodyTransform.forward);

				timeInState = 0f;

				pickable = null;

			}
		}

		if ( Input.GetKeyDown(KeyCode.O) ) {
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
		Vector3 direction = InputDirection;
		float targetSpeed = moveSpeed;
		if ( Input.GetKey(KeyCode.LeftShift) )
			targetSpeed = runMoveSpeed;
		if ( PressingInput == false )
			targetSpeed = 0f;

		if (Vector3.Dot (BodyTransform.forward, direction) > -0.9f) {
			BodyTransform.forward = Vector3.MoveTowards (BodyTransform.forward, direction, rotationSpeed * Time.deltaTime);
		} else {
			BodyTransform.forward = direction;
		}

		if (Physics.Raycast(GetTransform.position + Vector3.up * 0.5f, direction, collisionDistance, collisionLayerMask))
			targetSpeed = 0f;

		currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

		//      GetAnimator.SetFloat( "Movement", currentSpeed / moveSpeed );
		//		GetAnimator.speed = Input.GetKey(KeyCode.LeftShift) ? runAnimSpeed : 1f;

		GetTransform.Translate(BodyTransform.forward * currentSpeed * Time.deltaTime);
    }
    #endregion

	#region get hit
	private void GetHit_Start () {
		Throw ( -BodyTransform.forward );
	}
	private void GetHit_Update () {
		if ( timeInState > 4 ) {
			ChangeState (previousState);
		}
	}
	private void GetHit_Exit () {
		Reset ();
	}
	#endregion

    private Vector3 InputDirection
    {
        get
        {
            return new Vector3( Input.GetAxis("Horizontal") , 0f , Input.GetAxis ("Vertical") );
        }
    }
    private bool PressingInput
    {
        get {
            return Input.GetAxis ("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
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
					ChangeState (States.GetHit);
				}
			}
		}
	}

	void OnMouseOver () {

		if ( Input.GetMouseButtonDown (0) ) {
			Debug.Log ("bonjour");
		}

	}
}
