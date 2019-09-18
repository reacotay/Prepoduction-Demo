using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerAI : MonoBehaviour
{
    public Transform target;
    public float moveSpeed;
    public float minDistance;
    public float maxDistance;
    public bool moveTowards;
    

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) > maxDistance)
            moveTowards = true;

        if (Vector3.Distance(target.position, transform.position) < minDistance)
            moveTowards = false;

        if(moveTowards)
        {
            transform.LookAt(target.transform);
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed);
        }
        
    }
}
