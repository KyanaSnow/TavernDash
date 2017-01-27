using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public static UIManager Instance;

	[SerializeField]
	private Transform[] allCanvas;

	public enum CanvasType {
		Dialogue,
		Patience,
	}

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Place ( RectTransform recTransform, Vector3 worldPos ) {

		Vector3 v = Camera.main.WorldToViewportPoint (worldPos);

		recTransform.anchorMin = new Vector2 (v.x , v.y);
		recTransform.anchorMax = new Vector2 (v.x , v.y);


	}

	public GameObject CreateElement (GameObject prefab, CanvasType canvasType) {
		GameObject go = Instantiate (prefab) as GameObject;
		go.transform.SetParent (allCanvas[(int)canvasType]);
		go.transform.localScale = Vector3.one;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;

		return go;
	}
}
