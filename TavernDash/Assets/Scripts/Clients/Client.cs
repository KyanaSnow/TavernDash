using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour {

	#region state vars
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

	delegate void UpdateState();
	UpdateState updateState;

	private float timeInState = 0f;
	#endregion

	#region go to table vars
	private Table targetTable;
	#endregion

	// Use this for initialization
	void Start () {
		ChangeState (States.GoToTable);
	}
	
	// Update is called once per frame
	void Update () {
		if ( updateState != null ) {
			updateState ();
			timeInState += Time.deltaTime;
		}
	}

	#region go to table
	private void GoToTable_Start () {
		targetTable = TableManager.Instance.GetTable ();
	}
	private void GoToTable_Update () {
		//
	}
	private void GoToTable_Exit () {
		//
	}
	#endregion

	#region wait for order
	private void WaitForOrder_Start () {
		//
	}
	private void WaitForOrder_Update () {
		//
	}
	private void WaitForOrder_Exit () {
		//
	}
	#endregion

	#region state machine
	public void ChangeState ( States newState ) {
		previousState = currentState;
		currentState = newState;

		timeInState = 0f;

		ExitState (previousState);
		EnterState (currentState);
	}
	private void EnterState (States targetState) {
		switch (targetState) {
		case States.GoToTable:
			GoToTable_Start ();
			updateState = GoToTable_Update;
			break;
		case States.WaitForOrder:
			WaitForOrder_Start ();
			updateState = WaitForOrder_Update;
			// la meme pour tous les états
			break;
		case States.WaitForDish :
			// la meme pour tous les états
			break;
		case States.Eating :
			//la meme pour tous les états
			break;
		case States.Leaving :
			//la meme pour tous les états
			break;
		case States.Enraged :
			//la meme pour tous les états
			break;
		case States.Dead :
			//la meme pour tous les états
			break;
		}
	}
	private void ExitState (States targetState) {
		switch (targetState) {
		case States.GoToTable:
			GoToTable_Exit ();
			break;
		case States.WaitForOrder :
			//
			break;
		case States.WaitForDish :
			//
			break;
		case States.Eating :
			//
			break;
		case States.Leaving :
			//
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
}
