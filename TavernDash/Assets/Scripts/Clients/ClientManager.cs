using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientManager : MonoBehaviour {

	public static ClientManager Instance;

	private List<Client> clients = new List<Client>();

	void Awake () {
		Instance = this;
	}

	[SerializeField]
	private Transform spawnPoint;

	[SerializeField]
	private Transform doorTransform;

	[SerializeField]
	private GameObject clientPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetKeyDown (KeyCode.A) ) {
			NewClient ();
		}

		if ( Input.GetKeyDown (KeyCode.Z) ) {
//			int randomIndex = Random.Range ( 0 , clients.Count );

			if ( clients[0].CurrentState == Client.States.WaitForOrder ) {
				clients [0].TakeOrder ();
			} else if ( clients[0].CurrentState == Client.States.WaitForDish ){
				clients [0].Serve ();
			}
		}
	}

	public void NewClient () {
		GameObject client = Instantiate (clientPrefab) as GameObject;
		client.transform.position = spawnPoint.position;
		clients.Add ( client.GetComponent<Client>() );
	}

	public void RemoveClient ( Client client ) {
		clients.Remove (client);
	}

	public Transform DoorTransform {
		get {
			return doorTransform;
		}
		set {
			doorTransform = value;
		}
	}
}
