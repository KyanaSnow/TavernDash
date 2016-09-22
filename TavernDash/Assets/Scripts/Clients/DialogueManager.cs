using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour {

	public static DialogueManager Instance;

	[SerializeField]
	private Transform dialogueCanvas;

	[SerializeField]
	private GameObject bubblePrefab;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
