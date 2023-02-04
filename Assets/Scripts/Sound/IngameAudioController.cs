using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameAudioController : MonoBehaviour
{
    [SerializeField] private string bgmTheme;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(bgmTheme);
    }

    public void PlayBGM(string name)
    {
        SoundManager.Instance.PlayBGM(name);
    }

    public void PlaySFX(string name)
    {
        SoundManager.Instance.PlaySFX(name);
    }
}
