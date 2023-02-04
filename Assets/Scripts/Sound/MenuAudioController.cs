using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioController : MonoBehaviour
{
    public string bgmName;


    private void Start()
    {
        SoundManager.Instance.PlayBGM(bgmName);
    }

    public void PlayBGM(int id)
    {
        SoundManager.Instance.PlayBGM(id);
    }

    public void PlayBGM(string name)
    {
        SoundManager.Instance.PlayBGM(name);
    }

    public void PlaySFX(int id)
    {
        SoundManager.Instance.PlaySFX(id);
    }

    public void PlaySFX(string name)
    {
        SoundManager.Instance.PlaySFX(name);
    }

}
