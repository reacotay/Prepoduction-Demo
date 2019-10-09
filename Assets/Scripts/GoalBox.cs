using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBox : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool finished;
    public Transform dog, human;

    Collider m_Collider;

    void Start()
    {
        m_Collider = GetComponent<Collider>();
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Collider.bounds.Contains(dog.transform.position) && m_Collider.bounds.Contains(human.transform.position))
        {
            Debug.Log("Bounds contains both");
            finished = true;
        }

        else if (m_Collider.bounds.Contains(human.transform.position))
            Debug.Log("Bounds contains human");

        else if (m_Collider.bounds.Contains(human.transform.position))
        {
            Debug.Log("Bounds contains doggo");
        }
    }
}
