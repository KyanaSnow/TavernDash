using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager Instance;

	PlayerController[] playerControls;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		playerControls = GetComponentsInChildren<PlayerController> ();
	}

	public PlayerController getClosestToPoint ( Vector3 p ) {

		PlayerController closest = playerControls [0];

		if (playerControls.Length == 1)
			return closest;

		foreach ( PlayerController playerController in playerControls ) {

			float d1 = Vector3.Distance (closest.GetTransform.position, p);
			float d2 = Vector3.Distance (playerController.GetTransform.position, p);

			if ( d2 < d1 ) {
				closest = playerController;
			}

		}

		return closest;
	}
}
