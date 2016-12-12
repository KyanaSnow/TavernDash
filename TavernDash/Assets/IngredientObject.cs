using UnityEngine;
using System.Collections;

public class IngredientObject : Pickable {

	[SerializeField]
	private int ingredientID = 0;
	private Ingredient ingredient;

	PlayerController playerController;

	private bool onPlate = false;

	void Start () {
		
		Init ();

		ingredient = IngredientManager.Instance.Ingredients [ingredientID];

		playerController = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		playerController.Pickable = this;
		PickUp (playerController.BodyTransform);

	}

	void Update (){
		LerpObject ();
	}

	void OnTriggerStay ( Collider other ) {

		if ( other.tag == "Plate") {

			if ( PickableState == PickableStates.Dropped ) {
				Constrained = true;

				BoxCollider.enabled = false;
				PickableState = PickableStates.Unpickable;

				transform.SetParent (other.transform);

				other.GetComponentInParent<Plate> ().AddIngredientToDish (ingredient);
			}

		}

	}

	public Ingredient Ingredient {
		get {
			return ingredient;
		}
	}
}
