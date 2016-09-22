using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dialogue : MonoBehaviour {

	bool speaking = false;

	[SerializeField]
	private GameObject bubblePrefab;
	private GameObject bubbleObj;
	private Image bubbleImage;
	private Text bubbleText;

	[SerializeField]
	private float duration = 1f;
	float timer = 0f;

	[SerializeField]
	private Transform anchor;

	// Use this for initialization
	void Start () {
		CreateBubble ();
	}

	private void CreateBubble () {
		bubbleObj = UIManager.Instance.CreateElement (bubblePrefab, UIManager.CanvasType.Dialogue);
		bubbleImage = bubbleObj.GetComponent<Image> ();
		bubbleText = bubbleObj.GetComponentInChildren<Text> ();

		Exit ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if ( speaking ) {

			UIManager.Instance.Place (bubbleImage, anchor.position);

			if ( timer > duration )
				Exit ();

			timer += Time.deltaTime;


		}
			
	}

	public void Speak ( string phrase ) {

		if (speaking)
			return;

		bubbleText.text = phrase;

		speaking = true;

		timer = 0f;

		bubbleObj.SetActive (true);
	}

	private void Exit () {

		speaking = false;

		bubbleObj.SetActive (false);
	}

	public Transform Anchor {
		get {
			return anchor;
		}
		set {
			anchor = value;
		}
	}
}
