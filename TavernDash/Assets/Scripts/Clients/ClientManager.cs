using UnityEngine;
using System.Collections;

public class ClientManager : MonoBehaviour {

	public static ClientManager Instance;

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
	}

	private void NewClient () {
		GameObject client = Instantiate (clientPrefab) as GameObject;
		client.transform.position = spawnPoint.position;
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
