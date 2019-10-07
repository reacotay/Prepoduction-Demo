using UnityEngine;
using System;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour
{
    public delegate void Delegate();
    Delegate d1;
    public List<Delegate> dList;
    RaycastHit hit;

    float getOverLedgeTimer = 0;
    private float distanceToClimb = 0;
    private float climbedDistance = 0;
    private float distanceToBackUp = 0;
    private float distanceBackedUp = 0;

    public float CurrentWalkSpeed = 6;
    private float originalWalkSpeed = 6;
    public float gravity = -12;
    [Range(0, 1)]

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;

    public bool Locked = false;

    //Animator animator;
    Transform cameraT;
    CharacterController controller;

    void Start()
    {
        // animator = GetComponent<Animator>();
        hit = new RaycastHit();
        dList = new List<Delegate>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (Locked)
        {
            Locked = false;
            return;
        }

        if (dList.Count != 0)
        {
            dList[0].Method.Invoke(this, null);
        }
        else
        {
            Move(inputDir);
        }

        CheckForClimb();
        CheckforDrop();

        //PlayerMovement();

        // animator
        // float animationSpeedPercent = (currentSpeed / walkSpeed * .5f);
        //  animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

    }

    void CheckForClimb()
    {
        if (dList.Count == 0)
        {
            RaycastHit straightHit;
            RaycastHit overLedgeHit;
            int layerMask = 1 << 9;
            int lookDist = 100;

            float acceptableDist = 2f;
            float maxClimbHeight = 1.2f;

            //Debug.DrawRay(transform.position, )
            if (Physics.Raycast(transform.position, transform.forward, out straightHit, lookDist, layerMask))
            {
                if (straightHit.distance < acceptableDist)
                {
                    if (Input.GetKeyDown(KeyCode.E) && Physics.Raycast(transform.position + (transform.forward * 1.1f) + (transform.up * controller.height),
                        -transform.up, out overLedgeHit, lookDist, layerMask))
                    {
                        Debug.Log(overLedgeHit.distance);
                        if (overLedgeHit.distance < maxClimbHeight)
                        {
                            hit = straightHit;
                            distanceToClimb = controller.height - hit.distance + transform.position.y;
                            Debug.Log("Ledge is short enought to climb");
                            //Add the order of events that will comprise this action
                            d1 = new Delegate(TurnTowardsWall);
                            dList.Add(d1);
                            d1 = new Delegate(ClimbUp);
                            dList.Add(d1);
                            d1 = new Delegate(WalkForwardsABit);
                            dList.Add(d1);
                        }
                        else
                        {
                            Debug.Log("Ledge is to short to climb");
                        }
                    }
                }
            }
        }
    }

    void CheckforDrop()
    {
        if (dList.Count == 0)
        {
            RaycastHit groundHit;
            int layerMask = 1 << 9;
            int lookDist = 10;

            float minDistToGround = 3;
            float maxDistToGround = 4;

            Debug.DrawRay(transform.position, Vector3.down);

            if (Physics.Raycast(transform.position + (transform.forward * 0.8f), Vector3.down, out groundHit, lookDist, layerMask))
            {

                Debug.DrawRay(transform.position + (transform.forward * 0.8f), Vector3.down);
                if (minDistToGround < groundHit.distance && groundHit.distance < maxDistToGround)
                {
                    CurrentWalkSpeed = 0;

                    //Debugging
                    Physics.Raycast(transform.position + (transform.forward * 1f) + (-transform.up * controller.height),
                        -transform.forward, out hit, lookDist, layerMask);
                    Debug.DrawRay(transform.position + (transform.forward * 1f) + (-transform.up * controller.height),
                        -transform.forward);
                    Debug.DrawRay(hit.point, hit.normal);
                    //Debugging

                    if (Input.GetKeyDown(KeyCode.E) && Physics.Raycast(transform.position + (transform.forward * 1f) + (-transform.up * controller.height),
                        -transform.forward, out hit, lookDist, layerMask))
                    {
                        //Debug.Log(hit.distance);
                        distanceToBackUp = hit.distance;
                        d1 = new Delegate(TurnTowardsWall);
                        dList.Add(d1);
                        d1 = new Delegate(WalkBackwardsABit);
                        dList.Add(d1);
                        d1 = new Delegate(ClimbDown);
                        dList.Add(d1);
                    }
                }
                else if (minDistToGround < groundHit.distance && groundHit.distance > maxDistToGround)
                {
                    CurrentWalkSpeed = 0;
                }
                else
                {
                    CurrentWalkSpeed = originalWalkSpeed;
                }
            }
        }
    }

    void Move(Vector2 inputDir)
    {
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }
        
        float targetSpeed = CurrentWalkSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);


        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded)
        {
            velocityY = 0;
        }

    }

    void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 playerMovement = new Vector3(horizontal, 0f, vertical) * CurrentWalkSpeed * Time.deltaTime;

        transform.Translate(playerMovement, Space.Self);
    }

    void TurnTowardsWall()
    {
        Debug.DrawRay(transform.position, transform.forward);

        Debug.DrawLine(hit.point, hit.point + hit.normal * 5, Color.red);

        if (Vector3.Angle(transform.forward, -hit.normal) > 2)
        {
            Quaternion lookRotation = Quaternion.LookRotation(-hit.normal);
            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10);
        }
        else
        {
            dList.RemoveAt(0);
        }
    }

    private void ClimbDown()
    {
        if (!controller.isGrounded)
        {
            Vector3 velocity = -transform.up;
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            dList.RemoveAt(0);
        }
    }

    //The actual climbing movement
    void ClimbUp()
    {
        if (climbedDistance < distanceToClimb)
        {
            Vector3 velocity = transform.up;
            controller.Move(velocity * Time.deltaTime);
            climbedDistance += velocity.y * Time.deltaTime;
        }
        else
        {
            climbedDistance = 0;
            dList.RemoveAt(0);
        }
    }

    void WalkForwardsABit()
    {
        getOverLedgeTimer += Time.deltaTime;
        controller.Move(transform.forward * Time.deltaTime);
        if (getOverLedgeTimer > 1)
        {
            getOverLedgeTimer = 0;
            dList.RemoveAt(0);
        }
    }

    void WalkBackwardsABit()
    {
        if (distanceBackedUp < distanceToBackUp)
        {
            Debug.Log("Dist to Back: " + distanceToBackUp);
            Debug.Log("Dist Backed: " + distanceBackedUp);
            //getOverLedgeTimer += Time.deltaTime;
            distanceBackedUp += Mathf.Abs(transform.forward.z) * Time.deltaTime;
            controller.Move(-transform.forward * Time.deltaTime);
        }
        else
        {
            distanceBackedUp = 0;
            dList.RemoveAt(0);
        }
    }
}