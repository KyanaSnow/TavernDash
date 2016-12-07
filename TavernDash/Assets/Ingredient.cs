using UnityEngine;
using System.Collections;

public class Ingredient : Pickable {

	PlayerController playerController;

	public int ID = 0;

	bool onPlate = false;

	void Start () {
		
		Init ();

		playerController = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		playerController.Pickable = this;
		PickUp (playerController.BodyTransform);

	}

	void Update (){
		LerpObject ();
	}


	void OnTriggerStay ( Collider other ) {

		if ( other.tag == "Plate") {

			if (Input.GetKeyDown (KeyCode.L)) {
				
				Drop ();

				playerController.Pickable = null;

				Constrained = true;

				transform.SetParent (other.transform);

				other.GetComponentInParent<Plate> ().AddIngredient (ID);
			}

		}

	}
}
