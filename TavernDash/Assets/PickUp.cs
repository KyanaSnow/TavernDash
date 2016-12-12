using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickUp : MonoBehaviour {

	private Pickable pickableItem;

	private List<Pickable> pickableItems = new List<Pickable>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay ( Collider other ) {
		if (other.gameObject.tag == "PickUpTrigger") {
//			pickableItems.Add (other.gameObject.GetComponentInParent<Pickable> ());
		}
	}
}
