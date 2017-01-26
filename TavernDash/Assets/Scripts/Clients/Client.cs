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
	public float timeToOrder = 4f;
	private Dish wantedDish;

	// lerps
	Vector3 lerpInitialPos;
	Quaternion lerpInitalRot;

	// eating
	[SerializeField]
	private float eatingDuration = 10f;

    [Header("Tables")]
	// tables
	private Table targetTable;

	[Header("Chair")]
	private Chair targetChair;
	[SerializeField] private float distanceToSitDown = 1f;
	[SerializeField] private float sitDuration = 0.5f;
	private float sitTimer = 0f;
	private bool sitting = false;

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
		dialogue = GetComponentInChildren<Dialogue> ();
		_agent = GetComponent<NavMeshAgent> ();

		ChangeState (States.GoToTable);

		CreateRageFeedback ();

	}
	
	// Update is called once per frame
	void Update () {

		if ( updateState != null ) {

            updateState();

			timeInState += Time.deltaTime;

			if ( Sitting ) {

				SitUpdate ();

			}

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
			Debug.Log ("no tables ?");
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

		Sitting = true;
		targetChair.TurnToTable ();
	}
	private void WaitForOrder_Update () {

		UpdatePatience ();

	}
	private void WaitForOrder_Exit () {
//        GetAnimator.SetTrigger("Eat");
    }
    public void TakeOrder () {

		ChangeState (States.WaitForDish);

		wantedDish = DishManager.Instance.GetRandomDish;

		string phrase = "";
		foreach ( Ingredient ing in wantedDish.Ingredients ) {
			phrase += ing.ingredientName + " ";
		}

		dialogue.Speak (phrase);

		ChangeState (States.WaitForDish);

		CurrentRage -= stepsWhenOrdering;
		UpdatePatience ();
	}
	#endregion

	#region wait for dish
	private void WaitForDish_Start () {
		
	}
	private void WaitForDish_Update () {
		
		UpdatePatience ();

	}
	private void WaitForDish_Exit () {
		//
	}
	public void Serve (Plate plate) {

		CurrentRage = 0;
		UpdateFeedback ();

		plate.PickUp (GetTransform);
		pickable = plate.GetComponent<Pickable> ();

		dialogue.Speak ( "Miam !" );

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

		Sitting = false;

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

		Sitting = false;

		dialogue.Speak ("GRRRRRRR");
		rageFeedbackImage.sprite = lightningSprite;
		rageFeedbackImage.color = Color.white;

		timeInState = 3f;

	}
	private void Enraged_Update () {

		if ( TargetPoint == null ) {
			TargetPoint = Enraged_GetTarget;
		}

		if ( pickable != null ) {

			if (Vector3.Distance (GetTransform.position, TargetPoint.position) < 3.5f) {
				
				if (timeInState > 2.5f) {
					
					pickable.Throw ((targetPoint.position-GetTransform.position).normalized);
					pickable = null;

					TargetPoint = Enraged_GetTarget;
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
				
			if (timeInState > 3) {
				
				if (Vector3.Distance (GetTransform.position, targetChair.GetTransform.position) < 1.5f) {

					targetChair.PickUp (GetTransform);
					pickable = targetChair;

					timeInState = 0f;

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
		Throw ( -GetTransform.forward );

		if (previousState == States.Enraged) {
			--CurrentRage;
		} else {
			++CurrentRage;
		}

		UpdateFeedback ();
	}
	private void GetHit_Update () {
		if ( timeInState > 4 ) {

			if (previousState == States.Enraged) {
				if (CurrentRage == 0) {
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

		if ( rageTimer >= (CurrentRage*timeToNextStep) ) {
			
			++CurrentRage;

			UpdateFeedback ();

			if ( CurrentRage == rageToEnrage ) {
				ChangeState (States.Enraged);
				return;
			}
		}

	}
	public void UpdateFeedback () {
		// activate bubble
		rageFeedbackObj.SetActive (CurrentRage > 1);

		// lerp colors
		float l = (CurrentRage / rageToEnrage);

		rageFeedbackImage.transform.localScale = Vector3.one * (CurrentRage/rageToEnrage);
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

			return PlayerManager.Instance.getClosestToPoint (GetTransform.position).GetTransform;
		}
	}
	#endregion

	#region chair
	public bool Sitting {
		get {
			return sitting;
		}
		set {
			sitting = value;

			sitTimer = 0f;

			Constrained = true;

			_agent.enabled = !value;

			targetChair.Occupied = value;

			if (pickable != null)
				pickable.Drop ();

		}
	}
	private void SitUpdate () {
		sitTimer += Time.deltaTime;

		GetTransform.rotation = Quaternion.Lerp (lerpInitalRot , targetChair.GetTransform.rotation, sitTimer/sitDuration);
		GetTransform.position = Vector3.Lerp( lerpInitialPos , targetChair.ClientAnchor.position , sitTimer / sitDuration );

		if (sitTimer >= sitDuration)
			sitting = false;

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
    #endregion

    #region state machine
    public void ChangeState ( States newState ) {

		if (CurrentState == newState)
			return;

		previousState = currentState;
		currentState = newState;

		lerpInitalRot = GetTransform.rotation;
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
					Vector3 dir = (c.transform.position - GetTransform.position).normalized;
					dir.y = 0f;

					GetTransform.forward = dir;
					ChangeState (States.GetHit);
				}
			}
		}
	}

	public Dish WantedDish {
		get {
			return wantedDish;
		}
	}

	public Dialogue Dialogue {
		get {
			return dialogue;
		}
	}

	public float CurrentRage {
		get {
			return currentRage;
		}
		set {
			currentRage = Mathf.Clamp (value, 0, value);
		}
	}

}
