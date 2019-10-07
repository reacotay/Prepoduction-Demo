using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public Transform PlayerTransform;
    public Transform DogTransform;
    Transform currentTransform;
    Transform followerTransform;
    CharacterController controller;
    RaycastHit hit;
    Vector3 velocity;
    Vector3 direction;

    public float currentSpeed = 5f;
    public float gravity = -9.8f;
    float distance;
    float friction = 0.96f;
    bool previous;
    bool pushed = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentTransform = PlayerTransform;
        followerTransform = DogTransform;
    }

    void Update()
    {
        //Debug.Log(velocity.magnitude);
        CheckSide();
        distance = Vector3.Distance(currentTransform.position, transform.position);
        if (!controller.isGrounded)
        {
            controller.Move(new Vector3(0, gravity * Time.deltaTime, 0));
        }
        if (pushed)
        {
            Move();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (InRange())
                pushed = true;
        }

        Fallen();
    }

    bool InRange()
    {
        if (distance < 2)
            return true;
        else
            return false;
    }

    void Move()
    {
        velocity *= friction;

        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            controller.Move(direction * Time.deltaTime);
        }
        if (velocity.magnitude < 0.3f)
        {
            pushed = false;
        }
    }

    void Fallen()
    {
        if (!previous)
        {
            if (controller.isGrounded)
            {
                Debug.Log("hasFallen");
                velocity = Vector3.zero;
            }
        }
        previous = controller.isGrounded;
    }

    private void CheckSide()
    {
        Debug.DrawRay(currentTransform.position, currentTransform.forward, Color.green);
        if (InRange() && Input.GetKeyDown(KeyCode.E))
            if (Physics.Raycast(currentTransform.position, currentTransform.forward, out hit))
            {
                direction = -hit.normal;
                velocity = direction * currentSpeed;
            }
    }
}