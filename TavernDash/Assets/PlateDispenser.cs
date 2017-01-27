using UnityEngine;
using System.Collections;

public class PlateDispenser : MonoBehaviour {

	public GameObject platePrefab;

	public int maxPlate = 3;
	public Transform [] plateAnchors;

	int plateCount = 0;

	public float rate = 1f;
	float timer = 0f;

	void Update () {
		
		if ( plateCount < maxPlate ) {

			if ( timer > rate )
				newPlate ();

			timer += Time.deltaTime;

		}
	}

	void newPlate () {

		GameObject plate = Instantiate (platePrefab);

		plate.transform.position = transform.position;
		plate.transform.transform.forward = transform.forward;
		plate.transform.SetParent (transform);

		plateCount++;
		timer = 0f;

	}

	public int PlateCount {
		get {
			return plateCount;
		}
		set {
			plateCount = value;
		}
	}
}
