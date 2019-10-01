using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScriptHandler : MonoBehaviour
{
    public FollowerAI follower;
    public CharacterMovement movement;
    public CharacterController CC;
    public NavMeshAgent nav;
    public Transform other;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && Vector3.Distance(transform.position, other.transform.position) < 6)
        {
            follower.enabled = !follower.enabled;
            movement.enabled = !movement.enabled;
            CC.enabled = !CC.enabled;
            nav.enabled = !nav.enabled;

        }
    }
}
