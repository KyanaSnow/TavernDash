using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour {

	private Transform _transform;

		// CLIENTS
	private List<Client> clients = new List<Client>();

		// CHAIRS
	private List<Chair> chairs = new List<Chair>();
	[Header("Chairs")]
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

	#region chairs
	public bool HasFreeChairs () {

		if (chairs.Count == 0) {
//			Debug.Log ("TABLE : no chairs for client");
			return false;
		}

		if (chairs.Count == clients.Count ) {
//			Debug.Log ("TABLE : too much client for client");
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

		int n = chairs.Count;  

		while (n > 1) {  
			n--;  
			int k = Random.Range (0, n + 1);
			Chair value = chairs[k];  
			chairs[k] = chairs[n];  
			chairs[n] = value;  
		}

		foreach ( Chair chair in chairs ) {
			if (chair.PickableState == Pickable.PickableStates.Pickable) {
				return chair;
			}
		}

		Debug.LogError ("le client n'a pas trouvé de chaise");
		return null;
	}
	public void AddChair ( Chair chair ) {
		chairs.Add (chair);
	}
	public void RemoveChair ( Chair chair )  {
		chairs.Remove (chair);
	}
	#endregion

	#region clients
	public void AddClient ( Client client ) {
		clients.Add (client);
	}
	#endregion

	public List<Client> Clients {
		get {
			return clients;
		}
	}
}
