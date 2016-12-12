using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PickUpTrigger : MonoBehaviour {

	[SerializeField]
	private GameObject feedbackPrefab;

	GameObject feedbackObj;

	bool inside = false;

	// Use this for initialization
	void Start () {
		feedbackObj = UIManager.Instance.CreateElement (feedbackPrefab, UIManager.CanvasType.Dialogue);
		feedbackObj.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if ( inside ) {
			UIManager.Instance.Place (feedbackObj.GetComponent<Image>(), this.transform.position + Vector3.up * 0.5f );
		}
	}

	void OnTriggerEnter ( Collider other ) {
		if (other.tag == "Player") {
			feedbackObj.SetActive (true);
			inside = true;
			UIManager.Instance.Place (feedbackObj.GetComponent<Image>(), this.transform.position + Vector3.up * 0.5f );
		}
	}

	void OnTriggerExit ( Collider other ) {
		if (other.tag == "Player") {
			feedbackObj.SetActive (false);
			inside = false;
		}
	}
}
