using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plate : Pickable {

	List<int> ingredientIDs = new List<int>();

	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {

		bool straight = Vector3.Dot (transform.up, Vector3.up) > 0.8f;

		WaitForPlayerPickUp ();

		LerpObject ();

		if (!straight) {
			DropIngredients ();
		}
	}

	private void DropIngredients () {

		if (GetComponentsInChildren<Ingredient> ().Length == 0)
			return;

		foreach ( Ingredient ingredient in GetComponentsInChildren<Ingredient>() ) {
			ingredient.Throw ( transform.up );
		}

		ingredientIDs.Clear ();
	}

	public void AddIngredient ( int id ) {

		ingredientIDs.Add ( id );

	}
}
