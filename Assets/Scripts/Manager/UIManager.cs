using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Button = UnityEngine.UI.Button;
using System;

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
    [SerializeField] private Button quitButton;
    [SerializeField] private Button restartGame;
    [SerializeField] private Button newGame;
    [SerializeField] private Button pauseButton;

    [Header("Slider")]
    [SerializeField] private Slider musicSlide;
    [SerializeField] private Slider sfxSlide;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider progressBar;

    [Header("Skills")]
    [SerializeField] private Image rootSkill;
    [SerializeField] private Image healSkill;

    protected override void Awake()
    {
        base.Awake();
        //SetupRoot();
    }

    private void Start()
    {
        Setup();
        resumeButton.onClick.AddListener(delegate { GameManager.Instance.Pause(false); });
        pauseButton.onClick.AddListener(delegate { GameManager.Instance.Pause(true); });
        returnToMenuButton.onClick.AddListener(delegate { SceneLoader.Instance.LoadMenu(); });
        restartGame.onClick.AddListener(delegate { SceneLoader.Instance.LoadMenu(); });
        newGame.onClick.AddListener(ResetGame);
        musicSlide.onValueChanged.AddListener(SoundManager.Instance.BGM_ChangeVolume);
        SoundManager.Instance.OnBgmVolume += SetVolumeBGM;
        sfxSlide.onValueChanged.AddListener(SoundManager.Instance.SFX_ChangeVolume);
        SoundManager.Instance.OnSfxVolume += SetVolumeSFX;
        DataPersistenceManager.Instance.OnConfig += data => SetVolumeBGM(data.musicVolume);
        DataPersistenceManager.Instance.OnConfig += data => SetVolumeSFX(data.sfxVolume);
        DataPersistenceManager.Instance.RequestData();
        SceneLoader.Instance.OnLoadProgress += UpdateLoadingProgress;
        quitButton.onClick.AddListener(QuitGame);
    }

    public void Setup()
    {
        GameManager.Instance.OnRootUpdate += UpdateGuage;
        GameManager.Instance.OnPause += ActivatePause;
        GameManager.Instance.OnRootSkillUpdate += UpdateRoot;
        GameManager.Instance.OnHealSkillUpdate += UpdateHeal;
        rootSkill.fillAmount = 0;
        healSkill.fillAmount = 0;
    }

    private void SetupRoot()
    {
        rootIcon.sprite = rootData.itemIcon;
    }

    private void UpdateGuage(float percentage)
    {
        rootGuage.value = percentage;
    }

    private void UpdateRoot(float[] minMax)
    {
        rootSkill.fillAmount = 1 - (minMax[0] / minMax[1]);
    }

    private void UpdateHeal(float[] minMax)
    {
        healSkill.fillAmount = 1 - (minMax[0] / minMax[1]);
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

    public void SetGameover(bool value)
    {
        gameoverPanel.SetActive(value);
    }

    public void SetGUI(bool value)
    {
        gameplayPanel.SetActive(value);
    }

    public void LoadScene(string name)
    {
        loadingScreen.SetActive(true);
        StartCoroutine(SceneLoader.Instance.LoadSceneAsync(name, delegate { loadingScreen.SetActive(false); }));
    }

    public void SetVictory(bool v)
    {
        victoryPanel.SetActive(v);
    }

    private void ResetGame()
    {
        UpdateHealth(1);
        UpdateGuage(0);
        print("Reset UI!");
        GameManager.Instance.gameStarted = true;
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
