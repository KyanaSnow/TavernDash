using UnityEngine;
using System.Collections;

public class Plate : Pickable {

	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		WaitForPlayerPickUp ();
	}

}
