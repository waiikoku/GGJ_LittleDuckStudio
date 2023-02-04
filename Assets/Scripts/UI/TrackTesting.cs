using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTesting : MonoBehaviour
{
    public RectTransform ui;
    public Transform world;
    public Camera cam;
    private void LateUpdate()
    {
        Vector3 screenPoint = cam.WorldToViewportPoint(world.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        ui.gameObject.SetActive(onScreen);
        if (onScreen)
        {
            ui.anchoredPosition = cam.ViewportToScreenPoint(screenPoint);
        }
    }
}
