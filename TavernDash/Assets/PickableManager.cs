using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickableManager : MonoBehaviour {

	List<PickUpTrigger> pickableTriggers = new List<PickUpTrigger> ();

	PlayerController playerController;

	void Start () {
		playerController = GetComponent<PlayerController> ();
	}

	void Update () {

		if (pickableTriggers.Count > 0) {
		
			PickUpTrigger closestTrigger = pickableTriggers [0];

			foreach ( PickUpTrigger trigger in pickableTriggers ) {

				float newDot = Vector3.Dot ( playerController.BodyTransform.forward , (trigger.transform.position - transform.position).normalized );
				float initDot = Vector3.Dot ( playerController.BodyTransform.forward , (closestTrigger.transform.position - transform.position).normalized );

				if (newDot > initDot)
					closestTrigger = trigger;
				else
					trigger.Back ();

			}

			closestTrigger.Forward ();

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


}
