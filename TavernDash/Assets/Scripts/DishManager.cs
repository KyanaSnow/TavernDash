using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DishManager : MonoBehaviour {

	public static DishManager Instance;

	void Awake () {
		Instance = this;
	}

	public Dish GetRandomDish {

		get {
			int amount = Random.Range (1, 4);

			Dish dish = new Dish ();
			for (int i = 0; i < amount; i++) {
				dish.AddIngredient (IngredientManager.Instance.GetRandomIngredient());
			}



			return dish;
		}

	}
}

[System.Serializable]
public class Dish {

	[SerializeField]
	private List<Ingredient> ingredients = new List<Ingredient> ();

	public List<Ingredient> Ingredients {
		get {
			return ingredients;
		}
	}

	public void ClearIngredients () {
		ingredients.Clear ();
	}

	public void AddIngredient ( Ingredient ingredient ) {
		ingredients.Add (ingredient);
	}
}