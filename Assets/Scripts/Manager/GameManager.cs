using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int rootAmount;
    [SerializeField] private int desireRoot = 5;
    public Action<int> OnRootChange;
    public Action<float> OnRootUpdate;

    [Header("Debug")]
    public Item rootItem;

    public void AddItem(int id)
    {
        if (rootItem.itemID == id)
        {
            AddRoot();
        }
    }

    public void AddRoot(int amount = 1)
    {
        rootAmount = Mathf.Clamp(rootAmount + amount, 0, desireRoot);
        OnRootChange?.Invoke(rootAmount);
        OnRootUpdate?.Invoke((float)rootAmount / (float)desireRoot);
    }

    #region PauseSystem
    public bool isPause = false;
    public Action<bool> OnPause;

    public void Pause(bool pause)
    {
        isPause = pause;
        SetTimescale(isPause ? 0 : 1);
        OnPause?.Invoke(isPause);
    }

    public void TogglePause()
    {
        Pause(!isPause);
    }

    private void SetTimescale(float value)
    {
        Time.timeScale = Mathf.Clamp(value, 0f, 1f);
    }

    public void UseRoot(int v)
    {
        rootAmount -= v;
    }
    #endregion

    public void Gameover()
    {
        UIManager.Instance.SetGUI(false);
        UIManager.Instance.SetGameover(true);
    }

    public void Victory()
    {
        UIManager.Instance.SetVictory(true);
    }
}
