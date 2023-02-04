using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private Vector2 inputAxis;
    public Vector2 mousePos;
    public Action<Vector2> OnAxis;
    public Action<bool> OnLMB;
    public Action<bool> OnRMB;
    public Action<bool> OnSprinkle;
    public Action OnESC;

    [Header("Keybinds")]
    public KeyCode sprinkleKey;
    private void Update()
    {
        inputAxis.x = Input.GetAxis("Horizontal");
        inputAxis.y = Input.GetAxis("Vertical");
        OnAxis?.Invoke(inputAxis);
        mousePos = Input.mousePosition;

        if(Input.GetMouseButtonDown(0))
        {
            OnLMB?.Invoke(true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnLMB?.Invoke(false);
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnRMB?.Invoke(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            OnRMB?.Invoke(false);
        }
        if (Input.GetKeyDown(sprinkleKey))
        {
            OnSprinkle?.Invoke(true);
        }
        if (Input.GetKeyUp(sprinkleKey))
        {
            OnSprinkle?.Invoke(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.TogglePause();
            OnESC?.Invoke();
        }
    }
}
