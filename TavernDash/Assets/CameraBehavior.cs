using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    [SerializeField]
    private Vector3 decalToPlayer;

    [SerializeField]
    private float speedToPosition = 1f;

    [SerializeField]
    private float speedToRotation = 10f;

    [SerializeField]
	private Transform targetPoint;

    [SerializeField]
    private Vector3 minPos;

    [SerializeField]
    private Vector3 maxPos;

    [SerializeField]
    private float moveSpeed = 0.2f;

    [SerializeField]
    private float zoomSpeed = 1f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetDirection = (targetPoint.position - transform.position).normalized;

        transform.forward = Vector3.MoveTowards(transform.forward, targetDirection, speedToRotation * Time.deltaTime);

        Vector3 targetPos = Vector3.MoveTowards( transform.position , targetPoint.position + decalToPlayer , speedToPosition * Time.deltaTime );
        targetPos.x = Mathf.Clamp(targetPos.x, minPos.x , maxPos.x );
        targetPos.y = Mathf.Clamp(targetPos.y, minPos.y , maxPos.y );
        targetPos.z = Mathf.Clamp(targetPos.z, minPos.z , maxPos.z );

        transform.position = targetPos;
    }

	public bool drawGizmos = true;

	void OnDrawGizmos () {
		if ( drawGizmos ) {

			Color c = Color.blue;
//			c.a = 0.5f;

			Gizmos.color = c;

			float x = minPos.x/2;
			float y = minPos.y/2;
			float z = minPos.z ;

			Vector3 p = new Vector3 (x,y,z);

			Gizmos.DrawWireCube ( (minPos+maxPos)/2 , maxPos-minPos );
//				Gizmos.DrawWireCube ( (maxPos)/2 , maxPos );

		}
	}

	public Transform TargetPoint {
		get {
			return targetPoint;
		}
		set {
			targetPoint = value;
		}
	}
}
