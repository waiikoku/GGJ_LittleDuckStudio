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

    [Header("Panel")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private GameObject victoryPanel;

    [Header("Button")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button returnToMenuButton;

    [Header("Slider")]
    [SerializeField] private Slider musicSlide;
    [SerializeField] private Slider sfxSlide;
    [SerializeField] private Slider healthBar;

    [Header("Image")]
    [SerializeField] private Image loadingFill;
    protected override void Awake()
    {
        base.Awake();
        //SetupRoot();
    }

    private void Start()
    {
        GameManager.Instance.OnRootChange += UpdateRoot;
        GameManager.Instance.OnRootUpdate += UpdateGuage;
        GameManager.Instance.OnPause += ActivatePause;
        resumeButton.onClick.AddListener(delegate { GameManager.Instance.Pause(false); });
        returnToMenuButton.onClick.AddListener(delegate { SceneLoader.Instance.LoadMenu(); });
        musicSlide.onValueChanged.AddListener(value => SoundManager.Instance.BGM_ChangeVolume(value));
        DataPersistenceManager.Instance.OnConfig += data => SetVolume(data.musicVolume);
        DataPersistenceManager.Instance.RequestData();

        SceneLoader.Instance.OnLoadProgress += delegate (float value) { loadingFill.fillAmount = value; };
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

    public void ActivatePause(bool pause)
    {
        pausePanel.SetActive(pause);
    }

    public void SetVolume(float volume)
    {
        musicSlide.value = volume;
    }

    public void UpdateHealth(float percentage)
    {
        healthBar.value = percentage;
    }

    public void LoadScene(string name)
    {
        loadingScreen.SetActive(true);
        StartCoroutine(SceneLoader.Instance.LoadSceneAsync(name, delegate { loadingScreen.SetActive(false); }));
    }
}
