using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public float currentSpeed = 5f;
    CharacterController controller;
    public float gravity = -12;
    float velocityY;
    float distance;
    public Transform PlayerTransform;
    bool previous, pushed = false, hasHit = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        distance = Vector3.Distance(PlayerTransform.position, transform.position);
        if (!controller.isGrounded)
        {
            controller.Move(new Vector3(0, -currentSpeed * Time.deltaTime, 0));
        }  
        if (pushed)
        {
                Move();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(InRange())
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
        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = Vector3.zero * currentSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            controller.Move(new Vector3(-currentSpeed * Time.deltaTime, 0, 0));
        }
    }

    void Fallen()
    {
        if (hasHit)
        { 
            if (!previous)
            {
                if (controller.isGrounded)
                {
                   Debug.Log("hasFallen");
                   currentSpeed = 0;
                }
            }
        }
        previous = controller.isGrounded;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Krockkudde")
        {
            hasHit = true;
            pushed = false;
        }
    }
}