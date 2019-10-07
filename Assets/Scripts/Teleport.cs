using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Teleport : MonoBehaviour
{
    public int code;
    float disabletimer = 0;
    CharacterMovement cMove;
    
    private void Update()
    {
        if (disabletimer > 0)
        {
            disabletimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Player" || collider.name == "Dog" && disabletimer <= 0)
        {
            cMove = collider.gameObject.GetComponent<CharacterMovement>();
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
        }
    }
}