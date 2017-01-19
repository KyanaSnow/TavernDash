using UnityEngine;
using System.Collections;

public class IngredientBox : MonoBehaviour {

	public GameObject ingredient_Prefab;

	void OnTriggerStay (Collider other ) {
		if (other.tag == "Player") {

			PlayerController playerController = other.GetComponent<PlayerController> ();

			if (Input.GetButtonDown (playerController.Input_Action)) {

				Vector3 dir = (transform.position - playerController.GetTransform.position).normalized;

				if (Vector3.Dot (playerController.BodyTransform.forward, dir) > 0.7f
				    && Vector3.Distance (playerController.GetTransform.position, transform.position) < 1.5f) {
					GetIndredient ();
				}

			}
		}
	}

	private void GetIndredient () {

		GameObject ingredient_Instance = Instantiate (ingredient_Prefab, transform.position + (Vector3.up), Quaternion.identity) as GameObject;

		ingredient_Instance.GetComponent<Pickable> ().Init ();

	}
}
