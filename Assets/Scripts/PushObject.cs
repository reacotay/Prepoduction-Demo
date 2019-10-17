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

    public ThirdPersonView TPV;

    private Transform currentTransform;
    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 direction;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        currentTransform = TPV.target.transform;
    }

    void Update()
    {
        currentTransform = TPV.target.transform;
        distance = Vector3.Distance(currentTransform.position, transform.position);

        if (!controller.isGrounded)
        {
            controller.Move(new Vector3(0, gravity * Time.deltaTime, 0));
        }

        if (Input.GetKeyDown(KeyCode.E) && InRange())
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
        if (distance < 3)
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

        if (Physics.Raycast(currentTransform.position, currentTransform.forward, out hit, Mathf.Infinity))
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