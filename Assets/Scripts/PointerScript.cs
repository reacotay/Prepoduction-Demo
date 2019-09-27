using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerScript : MonoBehaviour
{
    private float m;
    private float cos;
    private float sin;
    private float angle;

    public Transform dogTransform;
    private Vector3 screenPos;
    private Vector3 screenCenter;
    private Vector3 screenBounds;

    void LateUpdate()
    {
        screenPos = Camera.main.WorldToScreenPoint(dogTransform.position);

        if (screenPos.z < 0)
            screenPos *= -1;

        screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
        screenPos -= screenCenter;

        angle = Mathf.Atan2(screenPos.y, screenPos.x);
        angle -= 90 * Mathf.Deg2Rad;

        cos = Mathf.Cos(angle);
        sin = -Mathf.Sin(angle);

        screenPos = screenCenter + new Vector3(sin * 150, cos * 150, 0);

        m = cos / sin;

        screenBounds = screenCenter * 0.9f;

        if (cos > 0)
        {
            screenPos = new Vector3(screenBounds.y / m, screenBounds.y, 0);
            Debug.Log("Upp");
        }
        else
        {
            screenPos = new Vector3(-screenBounds.y / m, -screenBounds.y, 0);
            Debug.Log("Ner");
        }

        if (screenPos.x > screenBounds.x)
        {
            screenPos = new Vector3(screenBounds.x, screenBounds.x * m, 0);
            Debug.Log("Höger");
        }
        else if (screenPos.x < -screenBounds.x)
        {
            screenPos = new Vector3(-screenBounds.x, -screenBounds.x * m, 0);
            Debug.Log("Vänster");
        }

        screenPos += screenCenter;

        transform.position = screenPos;
        transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
    }
}
