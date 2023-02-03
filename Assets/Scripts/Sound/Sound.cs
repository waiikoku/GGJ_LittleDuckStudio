using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public int id;
    public string name;
    public AudioClip clip;
    public float modifyVolume;

    [Header("License Information")]
    public bool noLicense;
    [TextArea]
    public string sourceURL;
}
