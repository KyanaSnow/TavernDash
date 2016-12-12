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

	public static bool operator ==(Dish dish1, Dish dish2)
	{
		List<Ingredient> tmp = dish2.Ingredients;

		foreach ( Ingredient ingredient in dish1.Ingredients ) {
			int result = tmp.FindIndex (x => x == ingredient);
			if (result != -1)
				tmp.RemoveAt (result);
			else
				return false;
		}

		return true;
	}

	public static bool operator !=(Dish dish1, Dish dish2)
	{
		Debug.Log ("dish 1 : " + dish1.Ingredients);
		Debug.Log ("dish 1 : " + dish2.Ingredients);

		List<Ingredient> tmp = dish2.Ingredients;

		foreach ( Ingredient ingredient in dish1.Ingredients ) {
			int result = tmp.FindIndex (x => x == ingredient);
			if (result != -1)
				tmp.RemoveAt (result);
			else
				return true;
		}

		return false;
	}
}