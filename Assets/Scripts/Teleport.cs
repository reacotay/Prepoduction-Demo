using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Teleport : MonoBehaviour
{
    private float disabletimer = 2;
    private bool available = false;

    public int code;

    private CharacterMovement cMove;
    private Collider collider;

    public GameObject BoundingBox;
    public GameObject FogSphere;

    private void Update()
    {
        if (disabletimer > 0)
        {
            disabletimer -= Time.deltaTime;
        }

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
                        if (tp.code == 1)
                        {
                            BoundingBox.SetActive(false);
                            FogSphere.SetActive(false);
                        }

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