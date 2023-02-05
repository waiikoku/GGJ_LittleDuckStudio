using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Button = UnityEngine.UI.Button;

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
    [SerializeField] private GameObject menuPanel;

    [Header("Button")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button returnToMenuButton;

    [Header("Slider")]
    [SerializeField] private Slider musicSlide;
    [SerializeField] private Slider sfxSlide;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider progressBar;
    protected override void Awake()
    {
        base.Awake();
        //SetupRoot();
    }

    private void Start()
    {
        GameManager.Instance.OnRootUpdate += UpdateGuage;
        GameManager.Instance.OnPause += ActivatePause;
        resumeButton.onClick.AddListener(delegate { GameManager.Instance.Pause(false); });
        returnToMenuButton.onClick.AddListener(delegate { SceneLoader.Instance.LoadMenu(); });
        musicSlide.onValueChanged.AddListener(SoundManager.Instance.BGM_ChangeVolume);
        SoundManager.Instance.OnBgmVolume += SetVolumeBGM;
        sfxSlide.onValueChanged.AddListener(SoundManager.Instance.SFX_ChangeVolume);
        SoundManager.Instance.OnSfxVolume += SetVolumeSFX;
        DataPersistenceManager.Instance.OnConfig += data => SetVolumeBGM(data.musicVolume);
        DataPersistenceManager.Instance.OnConfig += data => SetVolumeSFX(data.sfxVolume);
        DataPersistenceManager.Instance.RequestData();
        SceneLoader.Instance.OnLoadProgress += UpdateLoadingProgress;
    }

    private void SetupRoot()
    {
        rootIcon.sprite = rootData.itemIcon;
    }

    private void UpdateGuage(float percentage)
    {
        rootGuage.value = percentage;
    }

    public void ActivatePause(bool pause)
    {
        pausePanel.SetActive(pause);
    }

    public void SetVolumeBGM(float volume)
    {
        musicSlide.value = volume;
    }

    public void SetVolumeSFX(float volume)
    {
        sfxSlide.value = volume;
    }

    public void UpdateHealth(float percentage)
    {
        healthBar.value = percentage;
    }

    public void ActivateMenu(bool value)
    {
        menuPanel.SetActive(value);
    }

    private void UpdateLoadingProgress(float progress)
    {
        progressBar.value = progress;
    }

    public void LoadScene(string name)
    {
        loadingScreen.SetActive(true);
        StartCoroutine(SceneLoader.Instance.LoadSceneAsync(name, delegate { loadingScreen.SetActive(false); }));
    }
}
