using UnityEngine;
using System.Collections;

public class ThirdPersonView : MonoBehaviour
{
    private Transform target;
    public Transform player;
    public Transform dog;

    public bool lockCursor;
    private bool isPlayer;
    public float mouseSensitivity = 10;
    public float dstFromTarget = 4;
    public Vector2 pitchMinMax = new Vector2(-40, 55);

    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

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