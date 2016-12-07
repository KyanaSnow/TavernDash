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

			if ( Vector3.Dot ( playerController.BodyTransform.forward , dir ) > 0.7f
				&& Vector3.Distance (playerController.GetTransform.position , transform.position ) < 1.5f ) {
				GetIndredient ();
			}

		}
	}

	private void GetIndredient () {

		GameObject ingredient_Instance = Instantiate (ingredient_Prefabs[id], transform.position + (Vector3.up), Quaternion.identity) as GameObject;

		ingredient_Instance.GetComponent<Pickable> ().Init ();

	}
}
