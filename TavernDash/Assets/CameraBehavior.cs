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
}
