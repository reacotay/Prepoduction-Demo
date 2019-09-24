using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    
    public float MinDistance = 1.0f;
    public float MaxDistance = 4.0f;
    public float Smooth = 10.0f;
    public float Distance;

    Vector3 dollyDir;
    public Vector3 DollyDirAdjusted;
    

    void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        Distance = transform.localPosition.magnitude;
    }

    void Update()
    {
        CompensateForCollision();
    }

    void CompensateForCollision()
    {
        Vector3 desiredCameraPos = transform.TransformPoint(dollyDir * Distance);

        Debug.DrawLine(transform.parent.position, transform.position, Color.red);

        RaycastHit hit;
        if (Physics.Linecast(transform.parent.position, transform.position, out hit))
        {
            Debug.DrawRay(hit.point, Vector3.left, Color.cyan);
            Distance = Mathf.Clamp((hit.distance * 0.87f), MinDistance, MaxDistance);

            desiredCameraPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        }
        else
        {
            Distance = MaxDistance;
        }

        transform.position = desiredCameraPos;
        //transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * Distance, Time.deltaTime * Smooth);
    }
}