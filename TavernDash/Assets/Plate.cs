using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plate : Pickable {

	private Dish dish;

	void Start () {
		Init ();
		dish = new Dish ();
		Throw (transform.forward);
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
		GetComponentInParent<PlateDispenser> ().PlateCount -= 1;

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
