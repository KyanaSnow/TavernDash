using UnityEngine;
using System.Collections;

public class HeadRotation : MonoBehaviour {

    [SerializeField]
    private Transform headTransform;

    [SerializeField]
    private Transform bodyTransform;

    private Transform targetPoint;

    public Transform testTransform;

    [Range(0,90)]
    [SerializeField]
    private float angleToReturnX = 90f;
    [SerializeField]
    [Range(0,90)]
    private float angleToReturnY = 90f;

    [Range(0,90)]
    [SerializeField]
    private float angleToStopX = 60f;
    [Range(0,90)]
    [SerializeField]
    private float angleToStopY = 60f;

    float currentAngleX = 0f;
    float currentAngleY = 0f;

    [SerializeField]
    private float turnSpeed = 2f;

    void LateUpdate ()
    {
        if (targetPoint != null)
            RotateHead();
    }

    private void RotateHead()
    {
        // VERTICAL
        /*Vector3 directionToTargetY = (targetPoint.position - headTransform.position).normalized;

        int wayY = Vector3.Dot(bodyTransform.up, directionToTargetY) > 0 ? -1 : 1;

        float targetAngleY = Vector3.Angle(bodyTransform.forward, directionToTargetY) * wayY;
        if (targetAngleY > angleToReturnY || targetAngleY < -angleToReturnY)
            targetAngleY = 0f;

        currentAngleY = Mathf.MoveTowards(currentAngleY, Mathf.Clamp(targetAngleY, -angleToStopY, angleToStopY), turnSpeed * Time.deltaTime);

        headTransform.Rotate(Vector3.forward * currentAngleY);*/

        // HORIZONTAL
        Vector3 directionToTargetX = (targetPoint.position - headTransform.position).normalized;
        directionToTargetX.y = 0f;

        int wayX = Vector3.Dot(bodyTransform.right, directionToTargetX) < 0 ? -1 : 1;

        float targetAngleX = Vector3.Angle(bodyTransform.forward, directionToTargetX) * wayX;
        if (targetAngleX > angleToReturnX || targetAngleX < -angleToReturnX)
            targetAngleX = 0f;

        currentAngleX = Mathf.MoveTowards(currentAngleX, Mathf.Clamp(targetAngleX, -angleToStopX, angleToStopX), turnSpeed * Time.deltaTime);

        headTransform.Rotate(Vector3.up * currentAngleX, Space.World);
    }

    public Transform TargetPoint
    {
        get
        {
            return targetPoint;
        }
        set
        {
            targetPoint = value;
        }
    }
}
