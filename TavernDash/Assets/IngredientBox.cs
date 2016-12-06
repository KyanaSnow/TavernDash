using UnityEngine;
using System.Collections;

public class IngredientBox : MonoBehaviour {

	PlayerController playerController;

	public int id = 0;
	public GameObject[] ingredient_Prefabs;

	void Start () {
		playerController = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
	}

	void Update () {
		if ( Input.GetKeyDown (KeyCode.L) ) {

			Vector3 dir = (transform.position - playerController.GetTransform.position).normalized;

			if ( Vector3.Dot ( playerController.BodyTransform.forward , dir ) > 0.5f ) {
				GetIndredient ();
			}

		}
	}

	private void GetIndredient () {

		GameObject ingredient_Instance = Instantiate (ingredient_Prefabs[0], transform.position, Quaternion.identity) as GameObject;

		ingredient_Instance.transform.position = playerController.GetTransform.position + Vector3.up * 1.5f;

		ingredient_Instance.GetComponent<Pickable> ().Init ();

	}
}
