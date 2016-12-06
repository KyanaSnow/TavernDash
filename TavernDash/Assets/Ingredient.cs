using UnityEngine;
using System.Collections;

public class Ingredient : Pickable {

	PlayerController playerController;

	public int ID = 0;

	void Start () {
		Init ();

		playerController = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		playerController.Pickable = this;
		PickUp (playerController.BodyTransform);

	}
}
