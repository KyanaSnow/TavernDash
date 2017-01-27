using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickableManager : MonoBehaviour {

	List<PickUpTrigger> pickableTriggers = new List<PickUpTrigger> ();

	PlayerController playerControl;

	bool showFeedbacks = false;

	void Start () {
		playerControl = GetComponent<PlayerController> ();
	}

	void Update () {

		if (pickableTriggers.Count > 0) {
		
			if ( playerControl.Pickable != null || ShowFeedbacks == false) {
				for (int i = 0; i < pickableTriggers.Count; ++i)
					pickableTriggers [i].Hide ();

				return;
			}

			PickUpTrigger closestTrigger = pickableTriggers [0];

			foreach ( PickUpTrigger trigger in pickableTriggers ) {

				float newDot = Vector3.Dot ( playerControl.BodyTransform.forward , (trigger.transform.position - transform.position).normalized );
				float initDot = Vector3.Dot ( playerControl.BodyTransform.forward , (closestTrigger.transform.position - transform.position).normalized );

				if (newDot > initDot)
					closestTrigger = trigger;
				else
					trigger.Back ();

			}

			closestTrigger.Forward ();

			if (Input.GetButtonDown (playerControl.Input_Action) && playerControl.TimeInState > 0.5f ) {

				closestTrigger.LinkedPickable.PickUp (playerControl.BodyTransform);
				playerControl.Pickable = closestTrigger.LinkedPickable;

				closestTrigger.Exit (playerControl);

				playerControl.TimeInState = 0;

			}

		}

	}

	public List<PickUpTrigger> PickableTriggers {
		get {
			return pickableTriggers;
		}
		set {
			pickableTriggers = value;
		}
	}

	public bool ShowFeedbacks {
		get {
			return showFeedbacks;
		}
		set {
			showFeedbacks = value;
		}
	}
}
