using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : Singleton<UIManager>
{
    [Header("Root")]
    public Item rootData;
    [SerializeField] private Image rootIcon;
    [SerializeField] private Slider rootGuage;
    [SerializeField] private TextMeshProUGUI rootText;

    protected override void Awake()
    {
        base.Awake();
        SetupRoot();
    }

    private void Start()
    {
        GameManager.Instance.OnRootChange += UpdateRoot;
        GameManager.Instance.OnRootUpdate += UpdateGuage;
    }

    private void SetupRoot()
    {
        rootIcon.sprite = rootData.itemIcon;
    }

    private void UpdateRoot(int amount)
    {
        rootText.text = amount.ToString();
    }

    private void UpdateGuage(float percentage)
    {
        rootGuage.value = percentage;
    }
}
