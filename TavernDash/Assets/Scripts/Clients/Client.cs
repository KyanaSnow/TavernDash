using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Client : Pickable {

	private int id = 0;

	// properties
    [Header("Components")]
	[SerializeField]
	private Transform _bodyTransform;
	private NavMeshAgent _agent;
	private Transform _transform;
	private BoxCollider _boxCollider;
    [SerializeField]
    private Animator animator;
    private Dialogue dialogue;
    private HeadRotation headRotation;
	private PlayerController playerController;

	private int[] dish;

	// states
	public enum States {
		None,

		GoToTable,
		WaitForOrder,
		WaitForDish,
		Eating,
		Leaving,
		Enraged,
		GetHit,
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

		Init ();

		_transform = this.transform;
		_boxCollider = GetComponentInChildren<BoxCollider> ();
		playerController = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		dialogue = GetComponentInChildren<Dialogue> ();
		_agent = GetComponent<NavMeshAgent> ();
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

        }

		if ( Input.GetKeyDown(KeyCode.L)) {
			ChangeState ( States.Enraged );
		}

		UIManager.Instance.Place (rageFeedbackImage, dialogue.Anchor.position);

    }

	#region go to table
	private void GoToTable_Start () {
		LookForTable ();
		dialogue.Speak ("Bonjour !");
	}
	private void GoToTable_Update () {

		MoveTowards(targetPoint);

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
			Debug.Log ("snig?");
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

//		_boxCollider.enabled = false;

		SitOnChair ();
		targetChair.TurnToTable ();

//        GetAnimator.SetBool("Order", true);
//        GetAnimator.SetBool("Sit", true);
	}
	private void WaitForOrder_Update () {

		BodyTransform.rotation = targetChair.GetTransform.rotation;
		GetTransform.position = Vector3.Lerp( lerpInitialPos , targetChair.ClientAnchor.position , timeInState );

		if ( timeInState > timeToOrder ) {
			if (!spoke) {
				dialogue.Speak ("J'ai faim !");
				spoke = true;
			}

			UpdatePatience ();
		}

		if ( Input.GetKeyDown (KeyCode.L) ) {

//			dialogue.Speak ("");

		}
	}
	private void WaitForOrder_Exit () {
//        GetAnimator.SetTrigger("Eat");
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

		_boxCollider.enabled = true;

        TargetPoint = ClientManager.Instance.DoorTransform;

		Vector3 targetPos = transform.position;
		targetPos.y = 0f;
		transform.position = targetPos;
	}
	private void Leaving_Update () {


		if (timeInState > 3f) {
            
			MoveTowards(targetPoint);

			dialogue.Speak ("au revoir");

			if (Vector3.Distance (GetTransform.position, ClientManager.Instance.DoorTransform.position) < 0.7f) {
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

	Pickable pickable;

	private void Enraged_Start () {

		LeaveChair ();

		dialogue.Speak ("GRRRRRRR");
		rageFeedbackImage.sprite = lightningSprite;
		rageFeedbackImage.color = Color.white;

		TargetPoint = Enraged_GetTarget;

	}
	private void Enraged_Update () {

		if ( TargetPoint == null ) {
			TargetPoint = Enraged_GetTarget;
		}

		if ( pickable != null ) {

			if (Vector3.Distance (GetTransform.position, TargetPoint.position) < 3.5f) {
				if (timeInState > 5) {
					pickable.Throw ((targetPoint.position-GetTransform.position).normalized);
					pickable = null;
					timeInState = 0f;
				}
			} else {
				MoveTowards (targetPoint);
			}


		} else {


			if ( targetChair == null ) {

				if ( timeInState > 3 ) {
					LookForTable ();
					if ( targetChair == null ) {
						dialogue.Speak ("Je n'ai meme pas de chaise !");
						return;
					}

					timeInState = 0f;
				}

			}

			if (timeInState > 2.5f) {
				if (Vector3.Distance (GetTransform.position, targetChair.GetTransform.position) < 1) {
					if (timeInState > 3.5f) {
						targetChair.PickUp (GetTransform);
						pickable = targetChair;
					}


				} else {
					MoveTowards (targetChair.GetTransform);
				}
			}


		}



	}
	private void Enraged_Exit () {
		//
	}
	#endregion

	#region get hit
	private void GetHit_Start () {
		_agent.enabled = false;
		Throw ( -BodyTransform.forward );

		if (previousState == States.Enraged) {
			--currentRage;
		} else {
			++currentRage;
		}

		UpdateFeedback ();
	}
	private void GetHit_Update () {
		if ( timeInState > 4 ) {

			if (previousState == States.Enraged) {
				if (currentRage == 0) {
					ChangeState (States.Leaving);
					return;
				}
			}

			ChangeState (previousState);
		}
	}
	private void GetHit_Exit () {
		_agent.enabled = true;
		Reset ();
	}
	#endregion

	#region rage
	private void CreateRageFeedback () {
		rageFeedbackObj = UIManager.Instance.CreateElement (rageFeedbackPrefab, UIManager.CanvasType.Patience);
		rageFeedbackImage = rageFeedbackObj.GetComponentInChildren<Image> ();
		rageFeedbackObj.SetActive (false);
	}
	private void UpdatePatience () {

		rageTimer += Time.deltaTime;

		if ( rageTimer >= (currentRage*timeToNextStep) ) {
			
			++currentRage;

			UpdateFeedback ();

			if ( currentRage == rageToEnrage ) {
				ChangeState (States.Enraged);
				return;
			}
		}

	}
	public void UpdateFeedback () {
		// activate bubble
		rageFeedbackObj.SetActive (currentRage > 1);

		// lerp colors
		float l = (currentRage / rageToEnrage);

		rageFeedbackImage.transform.localScale = Vector3.one * (currentRage/rageToEnrage);
	}
	public Transform Enraged_GetTarget {
		get {
			if (ClientManager.Instance.Clients.Count > 1) {

				int randomClientIndex = Random.Range (0,ClientManager.Instance.Clients.Count);
				if ( ClientManager.Instance.Clients [randomClientIndex].Id == Id ) {
					if (randomClientIndex + 1 == ClientManager.Instance.Clients.Count)
						--randomClientIndex;
					else
						++randomClientIndex;
				}

				return ClientManager.Instance.Clients [randomClientIndex].GetTransform;

			}

			return playerController.transform;
		}
	}
	#endregion

	#region chair
	public void SitOnChair () {

		Constrained = true;

	}
	public void LeaveChair () {
		targetChair.Occupied = false;

		Vector3 p = transform.position;
		p.y = 0f;
		transform.position = p;
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
	#endregion

	#region move towards
	private void MoveTowards ( Transform point )
	{
		float targetSpeed = Vector3.Distance(GetTransform.position, point.position) < distanceToPoint ? 0f : maxSpeed;
		currentSpeed = Mathf.MoveTowards ( currentSpeed , targetSpeed, acceleration * Time.deltaTime );

		NavMeshAgent.speed = currentSpeed;
		NavMeshAgent.SetDestination (point.position);
	}

//    private void MoveTowards ( Transform point )
//    {
//        Vector3 direction = -(GetTransform.position - point.position).normalized;
//
//        float targetSpeed = Vector3.Distance(GetTransform.position, point.position) < distanceToPoint ? 0f : maxSpeed;
//        currentSpeed = Mathf.MoveTowards ( currentSpeed , targetSpeed, acceleration * Time.deltaTime );
//
//        if (Vector3.Distance(GetTransform.position, point.position) > distanceToPoint)
//        {
//            BodyTransform.forward = Vector3.Lerp(lerpInitalRot, direction, timeInState / turnDuration);
//
//        }
//
//      	GetAnimator.SetFloat( "Movement" , currentSpeed/ targetSpeed);
//        GetTransform.Translate(direction * currentSpeed * Time.deltaTime);
//    }
    #endregion

    #region state machine
    public void ChangeState ( States newState ) {

		if (CurrentState == newState)
			return;

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
		case States.GetHit:
			updateState = GetHit_Update;
			GetHit_Start ();
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
		case States.GetHit:
			GetHit_Exit ();
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

	public int Id {
		get {
			return id;
		}
		set {
			id = value;
		}
	}

	public NavMeshAgent NavMeshAgent {
		get {
			return _agent;
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
}
