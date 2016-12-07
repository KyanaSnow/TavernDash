using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Pickable : MonoBehaviour {

	[Header("Pickable Params")]
	[SerializeField]
	private float distanceToPickUp = 1f;

	private Rigidbody rigidbody;

	[SerializeField]
	private float force = 500f;

	[SerializeField]
	private float torque = 250f;

	private Transform initParent;

	PlayerController playerControl;

	bool carried = false;

	float t = 0f;

	float timer = 0f;
	[SerializeField]
	private float lerpDuration = 1f;
	private bool lerping = false;

	Transform target;

	Vector3 lerp_InitPos = Vector3.zero;
	Vector3 lerp_InitRot = Vector3.zero;

	public void LerpObject () {
		if ( lerping ) {

			Vector3 targetPos = target.position + (target.forward * 0.72f) + Vector3.up * 1f;
			transform.position = Vector3.Lerp (lerp_InitPos, targetPos, timer/lerpDuration);
			transform.up = Vector3.Lerp (lerp_InitRot, Vector3.up, timer/lerpDuration);

			timer += Time.deltaTime;

			if (timer >= lerpDuration)
				lerping = false;
		}
	}

	public void Init () {
		initParent = transform.parent;

		rigidbody = GetComponent<Rigidbody> ();
		playerControl = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		Constrained = true;

	}

	public void WaitForPlayerPickUp () {
		
		t += Time.deltaTime;

		if (Input.GetKeyDown (KeyCode.P) && playerControl.Pickable == null && t > 0.5f) {
			if (Vector3.Distance (playerControl.GetTransform.position, transform.position) < distanceToPickUp) {
				
				PickUp (playerControl.BodyTransform);
				playerControl.Pickable = this;

			}
		}
	}

	public virtual void PickUp (Transform _target) {

		target = _target;

		transform.SetParent (target);

		Constrained = true;

		lerping = true;
		timer = 0f;

		lerp_InitRot = transform.up;
		lerp_InitPos = transform.position;
	}

	public void Throw ( Vector3 direction ) {

		Constrained = false;

		rigidbody.AddTorque ( direction * torque );
		rigidbody.AddForce ( direction * force);

		carried = false;
		transform.parent = initParent;

		t = 0f;
	}

	public void Drop () {

		Constrained = false;

		carried = false;
		transform.parent = initParent;

		t = 0f;
	}

	public Rigidbody Rigidbody {
		get {
			return rigidbody;
		}
	}

	public void Reset () {

		transform.rotation = Quaternion.LookRotation ( Vector3.forward , Vector3.up );

		Vector3 p = transform.position;
		p.y = 0f;
		transform.position = p;

		Constrained = true;
	}

	public bool Constrained {
		get {
			if ( Rigidbody.constraints == RigidbodyConstraints.FreezeAll )
				return true;
			else
				return false;
		}
		set {
			if (value == true)
				Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			else
				Rigidbody.constraints = RigidbodyConstraints.None;
		}
	}
}