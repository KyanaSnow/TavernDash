using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plate : Pickable {

	private Dish dish;

	void Start () {
		Init ();
		dish = new Dish ();

		Drop ();

//		Throw (transform.forward);
	}

	// Update is called once per frame
	void Update () {

		LerpObject ();

		if (dish.Ingredients.Count > 0 && !Straight) {
			DropIngredients ();
		}
	}

	public override void PickUp (Transform _target)
	{
		if (GetComponentInParent<PlateDispenser> ()) {
<<<<<<< HEAD
			GetComponentInParent<PlateDispenser> ().newPlate ();
=======
			GetComponentInParent<PlateDispenser> ().PlateCount -= 1;
>>>>>>> 1ac3a55293fb3196c09c14a4f99defedbb5d7d9f
		
			transform.parent = null;
		}

		base.PickUp (_target);
	}

	private void DropIngredients () {

		if (GetComponentsInChildren<IngredientObject> ().Length == 0)
			return;

		foreach ( IngredientObject ingredient in GetComponentsInChildren<IngredientObject>() ) {
			ingredient.BoxCollider.enabled = true;
			ingredient.Throw ( transform.up );
		}

		dish.ClearIngredients ();

	}

	public void AddIngredientToDish ( Ingredient ingredient ) {

		dish.AddIngredient (ingredient);

	}

	public Dish Dish {
		get {
			return dish;
		}
	}
}
