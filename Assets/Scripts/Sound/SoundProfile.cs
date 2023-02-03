using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound",menuName = "Custom/Sound/Create Sound(Profile)")]
public class SoundProfile : ScriptableObject
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
