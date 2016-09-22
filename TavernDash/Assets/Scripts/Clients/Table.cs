﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour {

	private Transform _transform;

		// CLIENTS
	private List<Client> clients = new List<Client>();

		// CHAIRS
	private List<Chair> chairs = new List<Chair>();
	[SerializeField]
	private int maxChairAmount = 4;


	// Use this for initialization
	void Start () {
		_transform = this.transform;

		TableManager.Instance.AddTable (this);
	}

	public Transform GetTransform {
		get {
			return _transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region chairs
	public bool HasFreeChairs () {
		
		if (chairs.Count == clients.Count && chairs.Count > 0 ) {
			Debug.Log ("TABLE : too much client for client");
			return false;
		}

		if (chairs.Count == 0) {
			Debug.Log ("TABLE : no chairs for client");
			return false;
		}

		return true;
	}
	public bool TooManyChairs () {
		return chairs.Count >= maxChairAmount;
	}
	public List<Chair> Chairs {
		get {
			return chairs;
		}
		set {
			chairs = value;
		}
	}
	public Chair GetChair () {
		return chairs [clients.Count];
	}
	public void AddChair ( Chair chair ) {
		chairs.Add (chair);
	}
	#endregion

	#region clients
	public void AddClient ( Client client ) {
		clients.Add (client);
	}
	#endregion
}
