using UnityEngine;
using System.Collections;

public class Chair : MonoBehaviour {

	private Transform _transform;

	private bool occupied = false;

	#region table vars
	[SerializeField]
	private float distanceToAssignTable = 5f;
	private Table assignedTable;
	#endregion

	#region turning effect vars
	Vector3 initialTurn = Vector3.zero;
	bool turningToTable = false;
	[SerializeField]
	private float timeToTurn = 0.7f;

	float timer = 0f;
	#endregion


	void Start () {
		_transform = this.transform;
		LookForTable ();
	}

	void Update () {
		if ( turningToTable ) {
			float l = timer / timeToTurn;
			GetTransform.forward = Vector3.Lerp ( initialTurn , (assignedTable.GetTransform.position-GetTransform.position).normalized , l );

			if (l >= 1)
				turningToTable = false;

			timer += Time.deltaTime;
		}
	}

	#region table
	private void LookForTable () {
		foreach ( Table table in TableManager.Instance.Tables ) {
			if ( Vector3.Distance ( GetTransform.position , table.GetTransform.position ) < distanceToAssignTable ) {
				if ( table.TooManyChairs () == false) {
					AssignTable (table);
					return;
				}
			}
		}

		Debug.Log ("there are no tables in this room for this chair");
	}
	private void AssignTable ( Table table ) {

		assignedTable = table;
		assignedTable.AddChair (this);

		turningToTable = true;

		initialTurn = GetTransform.forward;

		timer = 0f;
	}
	#endregion

	public Transform GetTransform {
		get {
			return _transform;
		}
	}

//	void OnDrawGizmos () {
//		Gizmos.DrawWireSphere (this.transform.position, distanceToAssignTable);
//	}

	public bool Occupied {
		get {
			return occupied;
		}
		set {
			occupied = value;
		}
	}
}
