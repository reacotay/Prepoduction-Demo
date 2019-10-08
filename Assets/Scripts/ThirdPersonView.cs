using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ThirdPersonView : MonoBehaviour
{
    private bool swappingTarget = false;
    private float lerpTime = 3;
    private float currentLerpTime = 0;
    private float minAngleDiffForSwap = 15;
    private float maxSwapDistance = 6;
    private float yaw;
    private float pitch;

    public float MinDistance = 1.0f;
    public float MaxDistance = 4.0f;
    public float Smooth = 10.0f;
    public float Distance;
    public float mouseSensitivity = 10;
    public float dstFromTarget = 4;
    public float rotationSmoothTime = .12f;
    public bool lockCursor;

    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;
    private Vector3 newCamPosition;
    private GameObject target;
    private GameObject other;
    private RaycastHit hit;
    private CharacterMovement targetCM;
    private CharacterController targetCC;
    private FollowerAI otherFAI;
    private NavMeshAgent otherNVA;

    public GameObject Human;
    public GameObject Dog;
    public Vector2 pitchMinMax = new Vector2(-5, 55);

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        target = Human;
        other = Dog;

        targetCM = target.GetComponent<CharacterMovement>();
        targetCC = target.GetComponent<CharacterController>();
        otherFAI = other.GetComponent<FollowerAI>();
        otherNVA = other.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && !swappingTarget)
        {
            swappingTarget = true;
        }

        if (swappingTarget)
            SwapTarget();
    }

    void LateUpdate()
    {
        if (!swappingTarget)
        {
            Distance = transform.localPosition.magnitude;

            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
            transform.eulerAngles = currentRotation;
            transform.position = target.transform.position - transform.forward * dstFromTarget;

            CompensateForCollision();
        }
    }

    public GameObject GetCurrentTarget()
    {
        return target;
    }

    void CompensateForCollision()
    {
        //Vector3 desiredCameraPos; //= transform.TransformPoint(dollyDir * Distance);
        Debug.DrawLine(target.transform.position, transform.position, Color.red);

        if (Physics.Linecast(target.transform.position, transform.position, out hit))
        {
            if (hit.collider.name != "Main Camera" && hit.collider.name != "Player" && hit.collider.name != "Dog" && hit.collider.name != "Cylinder")
            {
                Debug.DrawRay(hit.point, hit.normal, Color.cyan);
                //transform.position = hit.point;

                Distance = Mathf.Clamp((hit.distance), MinDistance, MaxDistance);
                newCamPosition = target.transform.position - transform.forward * Distance;
                transform.position = new Vector3(hit.point.x, newCamPosition.y, hit.point.z);
            }
        }
    }

    void SwapTarget()
    {
        //Om "target" och "other" är inom ett visst avstånd från varandra.
        if (Vector3.Distance(target.transform.position, other.transform.position) < maxSwapDistance)
        {
            targetCM.enabled = false;
            targetCC.enabled = false;
            otherFAI.enabled = false;
            otherNVA.enabled = false;

            float targetFacing = Vector3.Angle((other.transform.position - target.transform.position), target.transform.forward);
            float otherFacing = Vector3.Angle((target.transform.position - other.transform.position), other.transform.forward);

            //Så länge "target" inte tittar på människan.
            if (targetFacing > minAngleDiffForSwap)
            { 
                Vector3 dirFromTargetToOther = other.transform.position - target.transform.position; dirFromTargetToOther.Normalize();
                dirFromTargetToOther.y = 0f;
                Quaternion lookRotationDog = Quaternion.LookRotation(dirFromTargetToOther);
                target.transform.rotation = Quaternion.Lerp(target.transform.rotation, lookRotationDog, Time.deltaTime * 2);
            }

            //Så länge "other" inte tittar på hunden.
            if (otherFacing > minAngleDiffForSwap)
            {
                Vector3 dirFromOtherToTarget = target.transform.position - other.transform.position; dirFromOtherToTarget.Normalize();
                dirFromOtherToTarget.y = 0f;
                Quaternion lookRotationHuman = Quaternion.LookRotation(dirFromOtherToTarget);
                other.transform.rotation = Quaternion.Lerp(other.transform.rotation, lookRotationHuman, Time.deltaTime * 2);
            }

            //Roterar kameran från den ena targeten till den andra.
            if (targetFacing < minAngleDiffForSwap && otherFacing < minAngleDiffForSwap)
            { 
                Vector3 newCamTargetPos = other.transform.position + (-other.transform.forward * dstFromTarget) + Vector3.up * 2;

                //Kameran kollar om den har bytat position till sitt nya target, och bytar target isåfall.
                if (currentLerpTime >= 1)
                {
                    if (target == Dog)
                    {
                        target = Human;
                        other = Dog;
                    }
                    else
                    {
                        target = Dog;
                        other = Human;
                    }

                    targetCM = target.GetComponent<CharacterMovement>();
                    targetCC = target.GetComponent<CharacterController>();
                    otherFAI = other.GetComponent<FollowerAI>();
                    otherNVA = other.GetComponent<NavMeshAgent>();

                    targetCM.enabled = true;
                    targetCC.enabled = true;
                    otherFAI.enabled = true;
                    otherNVA.enabled = true;

                    swappingTarget = false;
                    currentLerpTime = 0;
                }
                //If the camera hasn't rotated to the new target.
                else
                {
                    Vector3 tempCamTargetPos = target.transform.position + (target.transform.forward * (Vector3.Distance(target.transform.position, other.transform.position) / 2));
                    currentLerpTime += Time.deltaTime;
                    transform.LookAt(tempCamTargetPos);
                    transform.position = Vector3.Lerp(transform.position, newCamTargetPos, currentLerpTime / lerpTime);

                }
            }
        }
        else
            swappingTarget = false;
    }
}