using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Teleport : MonoBehaviour
{
    public int code;
    float disabletimer = 2;
    bool available = false;
    CharacterMovement cMove;
    Collider collider;
    
    private void Update()
    {
        if (disabletimer > 0)
        {
            disabletimer -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if (available)
        {
            if (Input.GetKey(KeyCode.B) && disabletimer <= 0)
            {
                cMove = collider.gameObject.GetComponent<CharacterMovement>();
                disabletimer = 2;
                cMove.Locked = true;

                foreach (Teleport tp in FindObjectsOfType<Teleport>())
                {
                    if (tp.code == code && tp != this)
                    {

                        tp.disabletimer = 2;
                        Vector3 position = tp.gameObject.transform.position;
                        position.y += 1;
                        collider.gameObject.transform.position = position;
                    }
                }

                cMove.Locked = true;
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Player" || collider.name == "Dog")
        {
            this.collider = collider;
            available = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.name == "Player" || collider.name == "Dog")
        {
            available = false;
        }
    }
}