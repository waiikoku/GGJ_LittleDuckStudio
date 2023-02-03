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
}
