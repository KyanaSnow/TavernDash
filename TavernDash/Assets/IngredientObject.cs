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

	}

	void Update (){
		LerpObject ();
	}

	public override void PickUp (Transform _target)
	{
		GetComponentInParent<IngredientBox> ().NewIngredient ();

		GetComponentInChildren<PickUpTrigger> ().GetComponent<BoxCollider> ().enabled = false;

		base.PickUp (_target);
	}


	void OnTriggerStay ( Collider other ) {

		if ( other.tag == "Plate") {

			Debug.Log (other.tag);

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
