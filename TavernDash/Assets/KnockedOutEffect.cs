using UnityEngine;
using System.Collections;

public class KnockedOutEffect : MonoBehaviour {

	public Transform parent;
	public float speed = 50f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		parent.Rotate (Vector3.forward * speed * Time.deltaTime);
	}
}
