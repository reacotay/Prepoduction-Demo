using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //Camera cam;

    public float WalkSpeed = 12;
    public float Gravity = -12;
    [Range(0,1)]
    public float AirControlPercent;

    public float TurnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float SpeedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;

    float velocityY;

    //Animator animator;
    Transform cameraT;
    CharacterController controller;
    public Camera MainCamera;

    private void Start()
    {
        //animator = GetComponent<Animator>;
        cameraT = MainCamera.transform;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        PlayerMovement(inputDir);

        // animator
        // float animationSpeedPercent = ((running) ? currentSpeed / RunSpeed : currentSpeed / WalkSpeed * 0.5f);
        // animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
    }

    void PlayerMovement(Vector2 inputDir)
    {
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(TurnSmoothTime));
        }

        float targetSpeed = WalkSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(SpeedSmoothTime));

        velocityY += Time.deltaTime * Gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);

        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.y).magnitude;

        if (controller.isGrounded)
        {
            velocityY = 0;
        }


        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");

        //Vector3 playerMovement = new Vector3(horizontal, 0f, vertical) * WalkSpeed * Time.deltaTime;

        //transform.Translate(playerMovement, Space.Self);
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded)
        {
            return smoothTime;
        }
        if (AirControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / AirControlPercent;
    }

}