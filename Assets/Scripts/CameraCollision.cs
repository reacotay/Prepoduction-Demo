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
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * Distance);

        Debug.DrawLine(transform.parent.position, desiredCameraPos, Color.red);
   //     Debug.DrawLine();

        RaycastHit hit;
        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
        {
            Debug.DrawRay(hit.point, Vector3.left, Color.cyan);
            Distance = Mathf.Clamp((hit.distance * 0.8f), MinDistance, MaxDistance);
            
        }
        else
        {
            Distance = MaxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * Distance, Time.deltaTime * Smooth);
    }
}
