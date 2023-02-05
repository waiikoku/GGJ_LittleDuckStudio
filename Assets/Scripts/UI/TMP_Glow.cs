using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMP_Glow : MonoBehaviour
{
    private TextMeshProUGUI text;

    public Color normalColor = Color.white;
    public Color targetColor = Color.white;
    public float value;
    public float omega;
    public float amplitutde;
    public bool normalize = false;
    public bool absolute = false;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        value = amplitutde * Mathf.Cos(omega * Time.time);
        if (absolute)
        {
            value = Mathf.Abs(value);
        }
        text.color = Color.Lerp(normalColor, targetColor, value);
    }
}
