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

	float lerp = 0f;
	private float lerpSpeed = 8.4f;

	// Use this for initialization
	void Start () {
		feedbackObj = UIManager.Instance.CreateElement (feedbackPrefab, UIManager.CanvasType.Dialogue);
		feedbackObj.SetActive (false);

		linkedPickable = GetComponentInParent<Pickable> ();
	}

	void OnTriggerStay ( Collider other ) {
		
		if (other.tag == "Player" ) {

			PlayerController playerControl = other.GetComponent<PlayerController> ();

			if (inside == false) {

				if (linkedPickable.PickableState == Pickable.PickableStates.Unpickable
					|| linkedPickable.PickableState == Pickable.PickableStates.Carried) {
					return;
				}


				if (playerControl.Pickable != null) {
					Exit (playerControl);
					return;
				}

				Enter (playerControl);

			} else {

				UIManager.Instance.Place (feedbackObj.GetComponent<RectTransform> (), this.transform.position + Vector3.up * 0.5f);

				if (linkedPickable.PickableState == Pickable.PickableStates.Unpickable
					|| linkedPickable.PickableState == Pickable.PickableStates.Carried) {
					Exit (playerControl);
				}

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

	public void Exit (PlayerController playerController) {

		playerController.GetComponent<PickableManager> ().PickableTriggers.Remove (this);

		feedbackObj.SetActive (false);
		lerp = 0f;
		inside = false;
	}

	public void Forward() {
		UIManager.Instance.Place (feedbackObj.GetComponent<RectTransform> (), this.transform.position + Vector3.up * 0.5f);
		feedbackObj.SetActive (true);
		feedbackObj.transform.localScale = Vector3.one * 2;
	}

	public void Back () {
		UIManager.Instance.Place (feedbackObj.GetComponent<RectTransform> (), this.transform.position + Vector3.up * 0.5f);
		feedbackObj.SetActive (true);
		feedbackObj.transform.localScale = Vector3.one;
	}

	public void Hide () {
		feedbackObj.SetActive (false);
//		lerp = 0f;
	}

	public Pickable LinkedPickable {
		get {
			return linkedPickable;
		}
	}
}
