using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Client : MonoBehaviour {

	// properties
    [Header("Components")]
	[SerializeField]
	private Transform _bodyTransform;
	private Transform _transform;
    [SerializeField]
    private Animator animator;
    private Dialogue dialogue;
    private HeadRotation headRotation;

	// states
	public enum States {
		GoToTable,
		WaitForOrder,
		WaitForDish,
		Eating,
		Leaving,
		Enraged,
		Dead,
	}
	private States previousState;
	private States currentState;

	public delegate void UpdateState();
	UpdateState updateState;

	private float timeInState = 0f;

    [Header("Movement")]
	// movement
	[SerializeField]
	private float rotationSpeed = 50f;
	[SerializeField]
	private float moveSpeed = 2f;

	// wait for order
	bool spoke = false;
	public float timeToOrder = 4f;

	// lerps
	Vector3 lerpInitialPos;
	Vector3 lerpInitalRot;

	// eating
	[SerializeField]
	private float eatingDuration = 10f;

    [Header("Tables")]
	// tables
	private Table targetTable;
	private Chair targetChair;
	[SerializeField] private float distanceToSitDown = 1f;

    [Header("Rage")]
	// patience
	[SerializeField]
	private GameObject rageFeedbackPrefab;
	private GameObject rageFeedbackObj;
	private Image rageFeedbackImage;
	[SerializeField]
	private Sprite lightningSprite;

	private float rageTimer = 1f;
	[SerializeField]
	private float timeToNextStep = 5f;
	private float currentRage = 1;

	[SerializeField]
	private float rageToEnrage = 6;
	[SerializeField]
	private int stepsWhenOrdering = 1;
	[SerializeField]
	private int stepsWhenServed = 2;

	// Use this for initialization
	void Start () {
		_transform = this.transform;
        
		dialogue = GetComponentInChildren<Dialogue> ();
        headRotation = GetComponentInChildren<HeadRotation>();
        headRotation.TargetPoint = GameObject.FindWithTag("Player").transform;

		ChangeState (States.GoToTable);

		CreateRageFeedback ();

	}
	
	// Update is called once per frame
	void Update () {

		if ( updateState != null ) {

            updateState();

			timeInState += Time.deltaTime;

            MoveTowards(targetPoint);
        }
    }

	#region go to table
	private void GoToTable_Start () {
		LookForTable ();
		dialogue.Speak ("Bonjour !");
        
	}
	private void GoToTable_Update () {

		if ( Vector3.Distance ( GetTransform.position , targetChair.GetTransform.position ) < distanceToSitDown ) {
			ChangeState (States.WaitForOrder);
		}

	}
	private void GoToTable_Exit () {
		//
	}
	private void LookForTable () {
		
		targetTable = TableManager.Instance.GetTable ();

		if (targetTable == null) {
			ChangeState (States.Leaving);
			return;
		} else {
			targetChair = targetTable.GetChair ();
			targetTable.AddClient (this);
            TargetPoint = targetChair.GetTransform;
            return;
		}
	}
	#endregion

	#region wait for order
	private void WaitForOrder_Start () {
        GetAnimator.SetBool("Order", true);
        GetAnimator.SetBool("Sit", true);
	}
	private void WaitForOrder_Update () {

        BodyTransform.forward = Vector3.Lerp(lerpInitalRot, targetChair.GetTransform.forward, timeInState);
		GetTransform.position = Vector3.Lerp( lerpInitialPos , targetChair.ClientAnchor.position , timeInState );

		if ( timeInState > timeToOrder ) {
			if (!spoke) {
				dialogue.Speak ("J'ai faim !");
				spoke = true;
			}

			UpdatePatience ();
		}
	}
	private void WaitForOrder_Exit () {
        GetAnimator.SetTrigger("Eat");
    }
    public void TakeOrder () {
		dialogue.Speak ("apportez moi un café!");

		ChangeState (States.WaitForDish);

		currentRage -= stepsWhenOrdering;
		UpdatePatience ();
	}
	#endregion

	#region wait for dish
	private void WaitForDish_Start () {
		
	}
	private void WaitForDish_Update () {
		
		//

		UpdatePatience ();
	}
	private void WaitForDish_Exit () {
		//
	}
	public void Serve () {
		dialogue.Speak ( "miam !" );

		ChangeState (States.Eating);
	}
	#endregion

	#region eating
	private void Eating_Start () {
		
	}
	private void Eating_Update () {
		if ( timeInState > eatingDuration ) {
			ChangeState (States.Leaving);
		}
	}
	private void Eating_Exit () {
		//
	}
	#endregion

	#region leaving
	private void Leaving_Start () {
        TargetPoint = ClientManager.Instance.DoorTransform;
	}
	private void Leaving_Update () {
		
		if (timeInState > 3f) {

            
			dialogue.Speak ("au revoir");

			if (Vector3.Distance (GetTransform.position, ClientManager.Instance.DoorTransform.position) < 0.2f) {
				ClientManager.Instance.RemoveClient (this);
				Destroy (this.gameObject);
			}
		}

	}
	private void Leaving_Exit () {
		//
	}
	#endregion

	#region enraged
	private void Enraged_Start () {
		dialogue.Speak ("GRRRRRRR");
		rageFeedbackImage.sprite = lightningSprite;
		rageFeedbackImage.color = Color.white;
	}
	private void Enraged_Update () {
		
		if (timeInState > 2) {
			rageFeedbackImage.transform.localScale = Vector3.one * (currentRage / rageToEnrage);
			UIManager.Instance.Place (rageFeedbackImage, dialogue.Anchor.position);
		}

	}
	private void Enraged_Exit () {
		//
	}
	#endregion

	#region rage
	private void CreateRageFeedback () {
		rageFeedbackObj = UIManager.Instance.CreateElement (rageFeedbackPrefab, UIManager.CanvasType.Patience);
		rageFeedbackImage = rageFeedbackObj.GetComponentInChildren<Image> ();
		rageFeedbackObj.SetActive (false);
	}
	private void UpdatePatience () {

			// activate bubble
		rageFeedbackObj.SetActive (currentRage > 1);

			// lerp colors
		float l = (currentRage / rageToEnrage);

		rageFeedbackImage.transform.localScale = Vector3.one * (currentRage/rageToEnrage);
		rageFeedbackImage.color = Color.Lerp ( Color.white , Color.black , l );

		UIManager.Instance.Place (rageFeedbackImage, dialogue.Anchor.position);

		rageTimer += Time.deltaTime;

		if ( rageTimer >= (currentRage*timeToNextStep) ) {
			
			++currentRage;

			if ( currentRage == rageToEnrage ) {
				ChangeState (States.Enraged);
				return;
			}
		}

	}
    #endregion

    #region movement
    private float currentSpeed = 0f;
    [SerializeField]
    private float acceleration = 1f;
    [SerializeField]
    private float maxSpeed = 2f;
    [SerializeField]
    private float turnDuration = 1f;
    [SerializeField]
    private float distanceToPoint = 1f;
    [SerializeField]
    private float distanceToWalls = 1f;
    [SerializeField]
    private float collisionDistance = 1f;
    [SerializeField]
    private LayerMask collisionLayer;
    [SerializeField]
    private Transform targetPoint;
    private float targetSpeed = 0f;

    private Transform TargetPoint
    {
        get
        {
            return targetPoint;
        }
        set
        {
            targetPoint = value;
        }
    }
    private void MoveTowards ( Transform point )
    {
       

        Vector3 direction = -(GetTransform.position - point.position).normalized;

        float targetSpeed = Vector3.Distance(GetTransform.position, point.position) < distanceToPoint ? 0f : maxSpeed;
        currentSpeed = Mathf.MoveTowards ( currentSpeed , targetSpeed, acceleration * Time.deltaTime );

        if (Vector3.Distance(GetTransform.position, point.position) > distanceToPoint)
        {
            BodyTransform.forward = Vector3.Lerp(lerpInitalRot, direction, timeInState / turnDuration);

        }

        GetAnimator.SetFloat( "Movement" , currentSpeed/ targetSpeed);

        GetTransform.Translate(direction * currentSpeed * Time.deltaTime);
    }
    #endregion

    #region state machine
    public void ChangeState ( States newState ) {
		previousState = currentState;
		currentState = newState;

		lerpInitalRot = BodyTransform.forward;
		lerpInitialPos = GetTransform.position;

		timeInState = 0f;

		ExitState ();
		EnterState ();
	}
	private void EnterState () {
		switch (currentState) {
		case States.GoToTable:
			updateState = GoToTable_Update;
			GoToTable_Start ();
			break;
		case States.WaitForOrder:
			updateState = WaitForOrder_Update;
			WaitForOrder_Start ();
			break;
		case States.WaitForDish:
			updateState = WaitForDish_Update;
			WaitForDish_Start ();
			break;
		case States.Eating:
			updateState = Eating_Update;
			Eating_Start ();
			break;
		case States.Leaving:
			updateState = Leaving_Update;
			Leaving_Start ();
			break;
		case States.Enraged:
			updateState = Enraged_Update;
			Enraged_Start ();
			break;
		case States.Dead :
			//la meme pour tous les états
			break;
		}
	}
	private void ExitState () {
		switch (previousState) {
		case States.GoToTable:
			GoToTable_Exit ();
			break;
		case States.WaitForOrder:
			WaitForOrder_Exit ();
			break;
		case States.WaitForDish:
			WaitForOrder_Exit ();
			break;
		case States.Eating:
			Eating_Exit ();
			break;
		case States.Leaving:
			Leaving_Exit ();
			break;
		case States.Enraged:
			Enraged_Exit ();
			break;
		case States.Dead :
			//
			break;
		}
	}
	#endregion

	#region properties
    public float TargetSpeed
    {
        get
        {
            return targetSpeed;
        }
        set
        {
            targetSpeed = value;
        }
    }
	public Transform GetTransform {
		get {
			return _transform;
		}
	}
    public Animator GetAnimator
    {
        get {
            return animator;
        }
    }
	public Transform BodyTransform {
		get {
			return _bodyTransform;
		}
	}
	public States CurrentState {
		get {
			return currentState;
		}
		set {
			currentState = value;
		}
	}
	#endregion

}
