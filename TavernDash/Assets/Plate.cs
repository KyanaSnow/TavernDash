using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plate : Pickable {

	private Dish dish;

	// Use this for initialization
	void Start () {
		Init ();

		dish = new Dish ();
	}
	
	// Update is called once per frame
	void Update () {

		WaitForPlayerPickUp ();

		LerpObject ();

		if (dish.Ingredients.Count > 0 && !Straight) {
			DropIngredients ();
		}
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
