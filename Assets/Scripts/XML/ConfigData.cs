using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Config")]
[System.Serializable]
public class ConfigData : XmlData<ConfigData>
{
    public float musicVolume;
    public float sfxVolume;

    public ConfigData(float music = 1,float sfx = 1)
    {
        musicVolume = music;
        sfxVolume = sfx;
    }

    public ConfigData()
    {
        musicVolume = 1;
        sfxVolume = 1;
    }
}
