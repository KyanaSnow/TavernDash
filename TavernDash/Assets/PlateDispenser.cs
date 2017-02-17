using UnityEngine;
using System.Collections;

public class PlateDispenser : MonoBehaviour {

	public GameObject platePrefab;
	private GameObject currentPlate;

	float timer = 0f;
	[SerializeField]
	private float distanceToCreate = 1f;

	bool created = false;

	void Start () {
		createPlate ();
	}

	void Update () {

		if ( !created ) {
			if (Vector3.Distance (currentPlate.transform.position, transform.position) > distanceToCreate)
				createPlate ();
		}

	}

	public void newPlate () {
		created = false;
	}

	private void createPlate () {
		currentPlate = Instantiate (platePrefab);

		currentPlate.transform.position = transform.position;
		currentPlate.transform.transform.forward = transform.forward;
		currentPlate.transform.SetParent (transform);

		created = true;
	}
}
