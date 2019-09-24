using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonView : MonoBehaviour
{
    public bool LockCursor;
    public float MouseSensitivity = 10;
    public Transform Target;
    public float DstFromTarget = 2;
    public Vector2 PitchMinMax = new Vector2(-20, 85);
    public Transform PlayerTransform;

    public float RotationSmoothTime = 0.12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;
    private Vector3 cameraCompensationPos;

    float yaw;
    float pitch;

    private float mouseX, mouseY;

    private void Start()
    {
        if (LockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void LateUpdate()
    {
        CamControl();
        //CompensateForWalls(PlayerTransform.position, transform.position);
    }

    void CamControl()
    {
        yaw += Input.GetAxis("Mouse X") * MouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * MouseSensitivity;
        pitch = Mathf.Clamp(pitch, PitchMinMax.x, PitchMinMax.y);


        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, RotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = Target.position - transform.forward * DstFromTarget;
    }

    private void CompensateForWalls(Vector3 fromObject, Vector3 toTarget)
    {
        //Debug.DrawLine(fromObject, toTarget, Color.red);
        //RaycastHit wallHit = new RaycastHit();
        //if(Physics.Linecast(fromObject, toTarget, out wallHit))
        //{
        //    Debug.DrawRay(wallHit.point, Vector3.left, Color.cyan);
        //    cameraCompensationPos = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
        //    //transform.position = cameraCompensationPos;
        //}
    }


}