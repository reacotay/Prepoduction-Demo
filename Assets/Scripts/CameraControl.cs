using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform[] views;
    public float transitionSpeed;
    Transform desiredView, currentView;
    ThirdPersonView thirdPersonView;
    void Start()
    {
        thirdPersonView = GetComponent<ThirdPersonView>();
        desiredView = views[0];
    }

    private void Update()
    {
        currentView = transform;

        if (Input.GetKeyDown(KeyCode.C))
            desiredView = views[0];


        if (Input.GetKeyDown(KeyCode.V))
            desiredView = views[1];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        SwitchView();
    }

    void SwitchView()
    {
        if (currentView != desiredView)
        {
            thirdPersonView.enabled = false;

            transform.position = Vector3.Lerp(transform.position, desiredView.position, Time.deltaTime * transitionSpeed);
            Vector3 currentAngle = new Vector3(
                Mathf.LerpAngle(transform.rotation.eulerAngles.x,
                desiredView.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.y,
                desiredView.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.z,
                desiredView.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

            transform.eulerAngles = currentAngle;
        }
        else
        {
            thirdPersonView.enabled = true;
        }

    }
}