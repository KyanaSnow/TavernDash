using UnityEngine;
using System.Collections;

public class IngredientBox : MonoBehaviour {

	public GameObject ingredient_Prefab;

	public float decalY = 1f;

	void Start () {
		NewIngredient ();
	}

	public void NewIngredient () {

		GameObject ingredient_Instance = Instantiate (ingredient_Prefab, transform.position + (Vector3.up), Quaternion.identity) as GameObject;
//		ingredient_Instance.GetComponent<Pickable> ().Init ();
		ingredient_Instance.transform.SetParent (transform);

	}
}
