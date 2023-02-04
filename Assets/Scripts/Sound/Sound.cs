using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public int id;
    public string soundName;
    public AudioClip clip;
    public float modifyVolume = 1;

    [Header("License Information")]
    public bool noLicense;
    [TextArea]
    public string sourceURL;
}
