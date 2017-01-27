using UnityEngine;
using System.Collections;

public class ClientTrigger : MonoBehaviour {

	public Client client;

	[SerializeField]
	private GameObject orderFeedbackPrefab;
	private GameObject orderFeedbackObj;

	bool inside = false;

	void Start () {
		orderFeedbackObj = UIManager.Instance.CreateElement (orderFeedbackPrefab, UIManager.CanvasType.Patience);
		orderFeedbackObj.SetActive (false);
	}

	void OnTriggerStay ( Collider other ) {

		if (other.tag == "Player" ) {

			if (client.CurrentState == Client.States.WaitForOrder) {

				if ( inside == false ) {
					other.GetComponent<PickableManager> ().ShowFeedbacks = false;
					orderFeedbackObj.SetActive (true);

					inside = true;
				}

				PlayerController playerControl = other.GetComponent<PlayerController> ();

				UIManager.Instance.Place (orderFeedbackObj.GetComponent<RectTransform> (), client.Dialogue.Anchor.position);

				if ( Input.GetButtonDown (playerControl.Input_Action) ) {

					client.TakeOrder ();
					Exit (other);

				}

			}



		}
	}

	void OnTriggerExit ( Collider other ) {

		if (other.tag == "Player" ) {
			Exit (other);
		}
	}

	void Exit (Collider other) {
		other.GetComponent<PickableManager> ().ShowFeedbacks = true;

		inside = false;

		orderFeedbackObj.SetActive (false);

		PlayerController playerControl = other.GetComponent<PlayerController> ();
	}
}
