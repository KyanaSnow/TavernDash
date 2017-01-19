using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Pickable : MonoBehaviour {

	public enum PickableStates {
		Pickable,
		Unpickable,

		Carried,
		Thrown,
		Dropped
	}
	PickableStates pickableState = PickableStates.Pickable;

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

	float t = 0f;

	float timer = 0f;
	[SerializeField]
	private float lerpDuration = 1f;
	private bool lerping = false;

	[SerializeField]
	private BoxCollider boxCollider;

	Transform target;

	Vector3 lerp_InitPos = Vector3.zero;
	Vector3 lerp_InitRot = Vector3.zero;

	[SerializeField]
	private Vector3 decalToTarget = new Vector3 (0f, 1.2f , 0.7f);

	[SerializeField]
	private float angleToStand = 0.8f;

	public void LerpObject () {
		
		if ( lerping ) {

			Vector3 decal = target.TransformDirection (decalToTarget);
			Vector3 targetPos = target.position + decal;
			transform.position = Vector3.Lerp (lerp_InitPos, targetPos, timer/lerpDuration);
			transform.up = Vector3.Lerp (lerp_InitRot, Vector3.up, timer/lerpDuration);

			timer += Time.deltaTime;

			if (timer >= lerpDuration)
				lerping = false;
		}

	}

	public void Init () {
		
		pickableState = PickableStates.Pickable;

		initParent = transform.parent;

		rigidbody = GetComponent<Rigidbody> ();
		boxCollider = GetComponentInChildren<BoxCollider> ();

		playerControl = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		Constrained = true;

	}

	public virtual void PickUp (Transform _target) {

		if ( PickableState == PickableStates.Unpickable )
			return;

		pickableState = PickableStates.Carried;

		target = _target;

		transform.SetParent (target);

		Constrained = true;

		lerping = true;
		timer = 0f;

		lerp_InitRot = transform.up;
		lerp_InitPos = transform.position;
	}

	public void Throw ( Vector3 direction ) {
		pickableState = PickableStates.Thrown;

		Constrained = false;

		rigidbody.AddTorque ( direction * torque );
		rigidbody.AddForce ( direction * force);

		transform.parent = initParent;

		t = 0f;
	}

	public virtual void Drop () {

		pickableState = PickableStates.Dropped;

		Constrained = false;

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

	public PickableStates PickableState {
		get {
			return pickableState;
		}
		set {
			pickableState = value;
		}
	}

	public BoxCollider BoxCollider {
		get {
			return boxCollider;
		}
	}

	public bool Straight {
		get {
			return Vector3.Dot (transform.up, Vector3.up) > angleToStand;
		}
	}
}