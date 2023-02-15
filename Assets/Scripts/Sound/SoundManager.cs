using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Range(0f,1f)]
    public float masterVolume;
    [Range(0f, 1f)]
    public float bgmVolume;
    [Range(0f, 1f)]
    public float sfxVolume;
    [Range(0f, 1f)]
    public float voiceVolume;
    [Range(0f, 1f)]
    public float ambientVolume;

    public bool muteBGM = false;
    public bool muteSFX = false;

    [Header("Audio Player")]
    public AudioSource bgmPlayer;
    public AudioSource sfxPlayer;

    [Header("Playlist")]
    public Sound[] musicPlaylist;
    public Sound[] sfxPlaylist;

    public Action<float> OnBgmVolume;
    public Action<float> OnSfxVolume;
    private Sound FindMusic(int id)
    {
        return System.Array.Find(musicPlaylist, music => music.id == id);
    }

    private Sound FindSFX(int id)
    {
        return System.Array.Find(sfxPlaylist, sfx => sfx.id == id);
    }


    private Sound FindMusic(string name)
    {
        return System.Array.Find(musicPlaylist, music => music.soundName == name);
    }

    private Sound FindSFX(string name)
    {
        return System.Array.Find(sfxPlaylist, sfx => sfx.soundName == name);
    }

    public void PlayBGM(Sound bgm)
    {
        bgmPlayer.Stop();
        bgmPlayer.clip = bgm.clip;
        bgmPlayer.volume = bgmVolume * bgm.modifyVolume;
        bgmPlayer.Play();
    }

    public void PlayBGM(int id)
    {
        Sound sound = FindMusic(id);
        if (sound == null)
        {
            return;
        }
        else
        {
            bgmPlayer.Stop();
            bgmPlayer.clip = sound.clip;
            bgmPlayer.volume = bgmVolume * sound.modifyVolume;
            bgmPlayer.Play();
        }
    }

    public void PlayBGM(string name)
    {
        Sound sound = FindMusic(name);
        if (sound == null)
        {
            Debug.LogWarning("No BGM Found");
            return;
        }
        else
        {
            bgmPlayer.Stop();
            bgmPlayer.clip = sound.clip;
            bgmPlayer.volume = bgmVolume * sound.modifyVolume;
            bgmPlayer.Play();
        }
    }

    public void StopBGM()
    {
        if (bgmPlayer.isPlaying == false) return;
        bgmPlayer.Stop();
    }

    public void PlaySFX(Sound effect)
    {
        sfxPlayer.PlayOneShot(effect.clip, sfxVolume * effect.modifyVolume);
    }

    public void PlaySFX(int id)
    {
        Sound sound = FindSFX(id);
        if(sound != null)
        {
            PlaySFX(sound);
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = FindSFX(name);
        if (sound != null)
        {
            PlaySFX(sound);
        }
        else
        {
            Debug.LogWarning("No SFX Found");
        }
    }


    public void BGM_ChangeVolume(float volume)
    {
        bgmPlayer.volume -= bgmVolume; //Clear Old volume
        bgmVolume = volume;
        bgmPlayer.volume += bgmVolume; //Add New volume
        OnBgmVolume?.Invoke(bgmVolume);
    }


    public void SFX_ChangeVolume(float volume)
    {
        sfxPlayer.volume -= sfxVolume; //Clear Old volume
        sfxVolume = volume;
        sfxPlayer.volume += sfxVolume; //Add New volume
        OnSfxVolume?.Invoke(sfxVolume);
    }

    public void MuteBGM(bool value)
    {
        muteBGM = value;
        if (value)
        {
            bgmPlayer.volume = 0;
        }
        else
        {
            bgmPlayer.volume = bgmVolume;
        }
    }

    public void ToggleMuteBGM()
    {
        MuteBGM(!muteBGM);
    }

    public void MuteSFX(bool value)
    {
        muteSFX = value;
        if (value)
        {
            sfxPlayer.volume = 0;
        }
        else
        {
            sfxPlayer.volume = sfxVolume;
        }
    }

    public void ToggleMuteSFX()
    {
        MuteSFX(!muteSFX);
    }
}
