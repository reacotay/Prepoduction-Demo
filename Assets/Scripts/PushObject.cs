using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public float currentSpeed = 5f;
    public float gravity = -9.8f;

    private float distance;
    private float friction = 0.96f;
    private bool pushed = false;

    public Transform PlayerTransform;
    public Transform DogTransform;

    private Transform currentTransform;
    private Transform followerTransform;
    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 direction;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentTransform = PlayerTransform;
        followerTransform = DogTransform;
    }

    void Update()
    {
        distance = Vector3.Distance(currentTransform.position, transform.position);

        if (!controller.isGrounded)
        {
            controller.Move(new Vector3(0, gravity * Time.deltaTime, 0));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CheckSide())
            {
                pushed = true;
            }
        }

        if (pushed)
        {
            Move();
        }
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

        if (velocity.magnitude < 0.3f)
        {
            pushed = false;
        }
    }

    private bool CheckSide()
    {
        currentTransform = Camera.main.GetComponent<ThirdPersonView>().GetCurrentTarget().transform;

        Debug.DrawRay(currentTransform.position, currentTransform.forward, Color.green);
        RaycastHit hit;
        LayerMask layerMask = 1 << 0;
        if (Physics.Raycast(currentTransform.position, currentTransform.forward, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.name == "Hitbox")
            {
                direction = -hit.normal;
                velocity = direction * currentSpeed;
                Debug.Log(direction.ToString());
                return true;
            }

        }
       

        return false;
    }
}