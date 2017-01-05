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

	// Use this for initialization
	void Start () {

		feedbackObj = UIManager.Instance.CreateElement (feedbackPrefab, UIManager.CanvasType.Dialogue);
		feedbackObj.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if ( inside ) {
			UIManager.Instance.Place (feedbackObj.GetComponent<Image>(), this.transform.position + Vector3.up * 0.5f );
		}
	}

	void OnTriggerStay ( Collider other ) {
		if (other.tag == "Player" ) {

			Vector3 dir = ( transform.position - other.transform.position );

			if (Vector3.Dot (other.GetComponent<PlayerController> ().BodyTransform.forward, dir) > angleToFeedback) {
				if (other.GetComponent<PlayerController> ().Pickable == null) {

					Enter ();

				}
			} else {
				Exit ();
			}

			
			if (other.GetComponent<PlayerController> ().Pickable != null) {
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
