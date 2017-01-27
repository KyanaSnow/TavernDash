using UnityEngine;
using System.Collections;

public class Chair : Pickable {
	
    private Transform _transform;

    private bool occupied = false;

	bool hasClient = false;

	[Header("Table")]
    [SerializeField]
    private float distanceToAssignTable = 5f;
    private Table assignedTable;

	[Header("Turn")]
	[SerializeField]
	private float timeToTurn = 0.7f;
    Vector3 initialTurn = Vector3.zero;
    bool turningToTable = false;

    [SerializeField]
    private Transform clientAnchor;

    float lerpTimer = 0f;

    void Start() {
        _transform = this.transform;
		Init ();
        LookForTable();

		if ( Assigned ) {
			TurnToTable ();
		}
    }

	void Update() {

		CheckTable ();

		LerpObject ();

		TurnToTableUpdate ();
	}

	#region pick up
	public override void PickUp (Transform target)
	{
		if (Occupied) {
			Debug.Log ("OCCUPE");
			return;
		}

		Debug.Log ("pick chair");

		base.PickUp (target);

		UnassignTable ();
	}
	#endregion

	#region turn to table
	public void TurnToTable () {

		Constrained = true;

		turningToTable = true;

		initialTurn = GetTransform.forward;

		lerpTimer = 0f;
	}

	private void TurnToTableUpdate () {
		if (turningToTable) {

			if (assignedTable == null) {
				Debug.Log ("exit table");
				turningToTable = false;
				return;
			}

			float l = lerpTimer / timeToTurn;
			GetTransform.forward = Vector3.Lerp(initialTurn, (assignedTable.GetTransform.position - GetTransform.position).normalized, l);

			lerpTimer += Time.deltaTime;

			if (l >= 1)
				turningToTable = false;
		}
	}
	#endregion

    #region table
	private void CheckTable () {
		if (Assigned) {

			bool closeToTable = Vector3.Distance (GetTransform.position, assignedTable.GetTransform.position) < distanceToAssignTable;

			if (!closeToTable || !Straight) {
				UnassignTable ();
			}

		} else {

			lerpTimer += Time.deltaTime;

			if (lerpTimer > 1f) {

				lerpTimer = 0f;

				LookForTable ();
//				if ( Assigned )
//					TurnToTable ();
			}

		}
	}
    private void LookForTable() {
        foreach (Table table in TableManager.Instance.Tables) {
            if (Vector3.Distance(GetTransform.position, table.GetTransform.position) < distanceToAssignTable) {
                if (table.TooManyChairs() == false) {
                    AssignTable(table);
                    return;
                }
            }
        }

    }
    private void AssignTable(Table table) {
        assignedTable = table;
        assignedTable.AddChair(this);
    }
	public void UnassignTable () {
		if (!Assigned)
			return;
			
		assignedTable.RemoveChair (this);
		assignedTable = null;
	}
    #endregion

    public Transform GetTransform {
        get {
            return _transform;
        }
    }

    public bool Occupied {
        get {
            return occupied;
        }
        set {
            occupied = value;
        }
    }

    public Transform ClientAnchor {
        get{
            return clientAnchor;
        }
    }

	public bool Assigned {
		get { return assignedTable != null; }
	}
}
