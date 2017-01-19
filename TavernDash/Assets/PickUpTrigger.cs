using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PickUpTrigger : MonoBehaviour {

	[SerializeField]
	private GameObject feedbackPrefab;

	GameObject feedbackObj;

	bool inside = false;

	[SerializeField]
	private float angleToFeedback = 0.5f;

	Pickable linkedPickable = null;

	// Use this for initialization
	void Start () {
		feedbackObj = UIManager.Instance.CreateElement (feedbackPrefab, UIManager.CanvasType.Dialogue);
		feedbackObj.SetActive (false);

		linkedPickable = GetComponentInParent<Pickable> ();
	}
	
	// Update is called once per frame
	void Update () {
		if ( inside ) {
			UIManager.Instance.Place (feedbackObj.GetComponent<Image>(), this.transform.position + Vector3.up * 0.5f );
		}
	}

	void OnTriggerStay ( Collider other ) {
		
		if (other.tag == "Player" ) {

			if (linkedPickable.PickableState == Pickable.PickableStates.Unpickable
				|| linkedPickable.PickableState == Pickable.PickableStates.Thrown )
				return;

			PlayerController playerControl = other.GetComponent<PlayerController> ();

			if (playerControl.Pickable != null) {
				Exit ();
				return;
			}

			Vector3 dir = ( transform.position - other.transform.position );

			if (Vector3.Dot (playerControl.BodyTransform.forward, dir) > angleToFeedback) {
				
				if (playerControl.Pickable == null) {

					Enter ();

					if (Input.GetButtonDown (playerControl.Input_Action)
						&& playerControl.TimeInState > 0.5f ) {
						linkedPickable.PickUp (playerControl.BodyTransform);
						playerControl.Pickable = linkedPickable;

						playerControl.TimeInState = 0;
					}

				}

			} else {
				Exit ();
			}

		}
	}

	void OnTriggerExit ( Collider other ) {
		if (other.tag == "Player") {
			Exit ();
		}
	}

	private void Enter () {

		feedbackObj.SetActive (true);
		inside = true;
		UIManager.Instance.Place (feedbackObj.GetComponent<Image>(), this.transform.position + Vector3.up * 0.5f );

	}

	private void Exit () {
		feedbackObj.SetActive (false);
		inside = false;
	}
}
