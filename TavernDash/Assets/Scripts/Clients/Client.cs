using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour {

	// properties
	private Transform _transform;
	[SerializeField]
	private Transform _bodyTransform;

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

	// movement
	[SerializeField]
	private float rotationSpeed = 50f;
	[SerializeField]
	private float moveSpeed = 2f;

	// lerps = 
	Vector3 lerpInitialPos;
	Vector3 lerpInitalRot;

	// tables
	private Table targetTable;
	private Chair targetChair;
	[SerializeField] private float distanceToSitDown = 1f;


	// Use this for initialization
	void Start () {
		_transform = this.transform;

		ChangeState (States.GoToTable);

	}
	
	// Update is called once per frame
	void Update () {
		if ( updateState != null ) {
			timeInState += Time.deltaTime;
			updateState ();
		}
	}

	#region go to table
	private void GoToTable_Start () {
		LookForTable ();
	}
	private void GoToTable_Update () {

		Vector3 dirToChair = (targetChair.GetTransform.position - GetTransform.position).normalized;
		BodyTransform.forward = Vector3.MoveTowards(BodyTransform.forward , dirToChair , rotationSpeed * Time.deltaTime );

		GetTransform.Translate (BodyTransform.forward * moveSpeed * Time.deltaTime);

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
			Debug.Log ("no table available, client leaving");
			ChangeState (States.Leaving);
			return;
		} else {
			targetChair = targetTable.GetChair ();
			targetTable.AddClient (this);
			return;
		}


	}
	#endregion

	#region wait for orderx
	private void WaitForOrder_Start () {
		
	}
	private void WaitForOrder_Update () {

		float l = timeInState;

		GetTransform.position = Vector3.Lerp (lerpInitialPos, targetChair.GetTransform.position + Vector3.up, l);
		BodyTransform.forward = Vector3.Lerp (lerpInitalRot, (targetTable.GetTransform.position-GetTransform.position).normalized , l);
	}
	private void WaitForOrder_Exit () {
		//
	}
	#endregion

	#region wait for order
	private void Leaving_Start () {
		
	}
	private void Leaving_Update () {
		if (timeInState > 3f) {
			Vector3 dirToDoor = (ClientManager.Instance.DoorTransform.position - GetTransform.position).normalized;
			BodyTransform.forward = Vector3.MoveTowards (BodyTransform.forward, dirToDoor, rotationSpeed * Time.deltaTime);

			GetTransform.Translate (BodyTransform.forward * moveSpeed * Time.deltaTime);

			if (Vector3.Distance (GetTransform.position, ClientManager.Instance.DoorTransform.position) < 0.2f) {
				Destroy (this.gameObject);
			}
		}
	}
	private void Leaving_Exit () {
		//
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
			// la meme pour tous les états
			break;
		case States.WaitForDish :
			// la meme pour tous les états
			break;
		case States.Eating :
			//la meme pour tous les états
			break;
		case States.Leaving:
			updateState = Leaving_Update;
			Leaving_Start ();
			break;
		case States.Enraged :
			//la meme pour tous les états
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
		case States.WaitForDish :
			//
			break;
		case States.Eating :
			//
			break;
		case States.Leaving:
			Leaving_Exit ();
			break;
		case States.Enraged :
			//
			break;
		case States.Dead :
			//
			break;
		}
	}
	#endregion

	#region properties
	public Transform GetTransform {
		get {
			return _transform;
		}
		set {
			_transform = value;
		}
	}
	public Transform BodyTransform {
		get {
			return _bodyTransform;
		}
		set {
			_bodyTransform = value;
		}
	}

	#endregion
}
