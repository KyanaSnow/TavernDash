using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour {

	private List<Client> clients = new List<Client>();

	[SerializeField]
	private Transform[] chair_Transforms;

	// Use this for initialization
	void Start () {
		TableManager.Instance.AddTable (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
