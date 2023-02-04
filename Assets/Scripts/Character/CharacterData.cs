using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character",menuName = "Custom/Data/Create Character(Profile)")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite characterIcon;

    [Header("Basic Stat")]
    public int health = 1; //max health
    public float speed = 1; // m/s
    public float attackRate = 1; // attack per second
}
