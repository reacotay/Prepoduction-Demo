using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private float timer = 4f;
    private float displayTime = 3f;

    public GameObject marker;
    public GameObject pointer;
    private Vector3 screenPos;
    public Transform dogTransform;

    private void Update()
    {
        screenPos = Camera.main.WorldToScreenPoint(dogTransform.position);

        if (Input.GetKeyDown(KeyCode.E))
            ShowDogIcon();

        if (timer < displayTime)
        {
            if (screenPos.z > 0 &&
                screenPos.y > 0 && screenPos.y < Screen.height &&
                screenPos.x > 0 && screenPos.x < Screen.width)
            {
                marker.SetActive(true);
                pointer.SetActive(false);
            }
            else
            {
                marker.SetActive(false);
                pointer.SetActive(true);
            }
        }

        if (marker.activeSelf || pointer.activeSelf)
        {
            timer += Time.deltaTime;

            if (timer > displayTime)
            {
                marker.SetActive(false);
                pointer.SetActive(false);
            }
        }
    }

    public void ShowDogIcon()
    {
        if (!marker.activeSelf && !pointer.activeSelf)
        {
            marker.SetActive(true);
            pointer.SetActive(true);
            timer = 0;
        }
    }
}
