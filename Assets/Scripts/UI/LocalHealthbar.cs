using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalHealthbar : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private Transform root;
    [SerializeField] private RectTransform rt;
    Vector3 screenPoint;
    [SerializeField] private Slider hpBar;
    [SerializeField] private CharacterCombat combat;
    private void Awake()
    {
        cam = Camera.main;
        combat.OnHealthUpdate += UpdateHealth;
    }

    private void LateUpdate()
    {
        screenPoint = cam.WorldToViewportPoint(root.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        rt.gameObject.SetActive(onScreen);
        if (onScreen)
        {
            rt.anchoredPosition = cam.ViewportToScreenPoint(screenPoint);
        }
    }

    private void UpdateHealth(float value)
    {
        hpBar.value = value;
    }
}
