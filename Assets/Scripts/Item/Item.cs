using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item",menuName = "Custom/Data/Create Item")]
public class Item : ScriptableObject
{
    public int itemID;
    public string itemName;
    public Sprite itemIcon;
    [TextArea]
    public string itemDescription;
}
