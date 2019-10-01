using UnityEngine;
using System.Collections;

public class ThirdPersonView : MonoBehaviour
{
    private Transform target;
    public Transform player;
    public Transform dog;
    RaycastHit hit;

    public bool lockCursor;
    private bool isPlayer;
    public float mouseSensitivity = 10;
    public float dstFromTarget = 4;
    public Vector2 pitchMinMax = new Vector2(-5, 55);

    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;
    Vector3 newVector3;



    public float MinDistance = 1.0f;
    public float MaxDistance = 4.0f;
    public float Smooth = 10.0f;
    public float Distance;

    float yaw;
    float pitch;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        target = player;
        isPlayer = true;
    }

    private void Update()
    {
        Distance = transform.localPosition.magnitude;
        if (Input.GetKeyDown(KeyCode.V))
            SwapTarget();
    }

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;
        transform.position = target.position - transform.forward * dstFromTarget;

        CompensateForCollision();

    }

    void CompensateForCollision()
    {
        //Vector3 desiredCameraPos; //= transform.TransformPoint(dollyDir * Distance);
        Debug.DrawLine(transform.parent.position, transform.position, Color.red);

        if (Physics.Linecast(transform.parent.position, transform.position, out hit))
        {
            if (hit.collider.name != "Main Camera" && hit.collider.name != "Player")
            {
                Debug.DrawRay(hit.point, hit.normal, Color.cyan);
                //transform.position = hit.point;

                Distance = Mathf.Clamp((hit.distance), MinDistance, MaxDistance);
                newVector3 = target.position - transform.forward * Distance;
                transform.position = new Vector3(hit.point.x, newVector3.y, hit.point.z);
            }
        }
    }

    void SwapTarget()
    {
        if (Vector3.Distance(player.transform.position, dog.transform.position) < 6)
        {
            isPlayer = !isPlayer;

            if (isPlayer)
                target = player;
            else if (!isPlayer)
                target = dog;
        }
    }

}