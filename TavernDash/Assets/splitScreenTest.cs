using UnityEngine;
using System.Collections;

public class splitScreenTest : MonoBehaviour {

	public Camera[] cameras;

	bool lerping = false;

	float t = 0f;

	public bool split = false;

	public Vector2 splitPos;
	public Vector2 splitWidth;
	public Vector2 joinedPos;
	public Vector2 joinedWidth;
	public float[] xPos;

	public float d = 0.5f;

	public Transform testTarget;
	public Transform t1;
	public Transform t2;
	
	void Update () {

		testTarget.position = t1.position + (t2.position - t1.position) / 2;

		if (Input.GetKeyDown (KeyCode.P))
			Split ();

		if ( lerping ) {

			t += Time.deltaTime;

			float l = split ? 1 - t / d : t / d;

			int index = 0;
			foreach (Camera cam in cameras) {

				Vector2 pos = new Vector2 ( Mathf.Lerp (xPos[index],0f,l) , 0f );
				Vector2 scale = new Vector2 ( Mathf.Lerp (0.5f,1f,l) , 1f );

				cam.rect = new Rect (pos, scale);

				++index;
			}

			if (t > d)
				lerping = false;

		}
	}

	private void Split () {

		lerping = true;

		t = 0f;

		split = !split;

		cameras [0].GetComponent<CameraBehavior> ().TargetPoint = split ? t1 : testTarget;
		cameras [1].GetComponent<CameraBehavior> ().TargetPoint = split ? t2 : testTarget;
	}
}
