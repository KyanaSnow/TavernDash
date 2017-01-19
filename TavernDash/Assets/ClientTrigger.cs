using UnityEngine;
using System.Collections;

public class ClientTrigger : MonoBehaviour {

	public Client client;

	void OnTriggerStay ( Collider other ) {

		if (other.tag == "Player" ) {

			PlayerController playerControl = other.GetComponent<PlayerController> ();

			if ( Input.GetButtonDown (playerControl.Input_Action) ) {
				
				if (client.CurrentState == Client.States.WaitForOrder)
					client.TakeOrder ();
				
			}

		}
	}
}
