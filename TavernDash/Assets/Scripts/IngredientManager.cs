using UnityEngine;
using System.Collections;

public class IngredientManager : MonoBehaviour {

	[SerializeField]
	private Ingredient[] ingredients;

	public static IngredientManager Instance;

	void Awake () {
		Instance = this;
	}

	void Start () {
//		for (int i = 0; i < ingredients.Length; ++i ) {
//			ingredients [i].id = i;
//		}
	}

	public Ingredient GetRandomIngredient () {
		return ingredients [Random.Range (0, ingredients.Length)];
	}

	public Ingredient[] Ingredients {
		get {
			return ingredients;
		}
	}
}

[System.Serializable]
public class Ingredient {

	public string ingredientName = "";

}