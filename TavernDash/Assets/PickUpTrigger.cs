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

			if (linkedPickable.PickableState == Pickable.PickableStates.Unpickable )
				return;

			PlayerController playerControl = other.GetComponent<PlayerController> ();

			if (playerControl.Pickable != null) {
				Exit (playerControl);
				return;
			}

			Vector3 dir = ( transform.position - other.transform.position );

			if (Vector3.Dot (playerControl.BodyTransform.forward, dir) > angleToFeedback) {
				
				if (playerControl.Pickable == null) {

					Enter (playerControl);

					if (Input.GetButtonDown (playerControl.Input_Action)
						&& playerControl.TimeInState > 0.5f ) {
						linkedPickable.PickUp (playerControl.BodyTransform);
						playerControl.Pickable = linkedPickable;

						playerControl.TimeInState = 0;
					}

				}

			} else {
				Exit (playerControl);
			}

		}
	}

	void OnTriggerExit ( Collider other ) {
		if (other.tag == "Player") {
			Exit (other.GetComponent<PlayerController>());
		}
	}

	private void Enter (PlayerController playerController) {

		playerController.GetComponent<PickableManager> ().PickableTriggers.Add (this);

		feedbackObj.SetActive (true);
		inside = true;

	}

	private void Exit (PlayerController playerController) {

		playerController.GetComponent<PickableManager> ().PickableTriggers.Remove (this);

		feedbackObj.SetActive (false);
		inside = false;
	}

	public void Forward() {
		feedbackObj.transform.localScale = Vector3.one * 2;
	}

	public void Back () {
		feedbackObj.transform.localScale = Vector3.one;
	}

	public Pickable LinkedPickable {
		get {
			return linkedPickable;
		}
	}
}
