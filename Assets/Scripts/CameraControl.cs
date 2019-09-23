using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform[] views;
    public float transitionSpeed;
    Transform currentView;
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            currentView = views[0];

        if (Input.GetKeyDown(KeyCode.V))
            currentView = views[1];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);
        Vector3 currentAngle = new Vector3(
            Mathf.LerpAngle(transform.rotation.eulerAngles.x,
            currentView.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
        Mathf.LerpAngle(transform.rotation.eulerAngles.y,
            currentView.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
        Mathf.LerpAngle(transform.rotation.eulerAngles.z,
            currentView.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

        transform.eulerAngles = currentAngle;
    }
}
