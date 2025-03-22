using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]

public class ItemData : ScriptableObject
{
    public int ItemCode;
    public Sprite itemIcon;
    public string itemName;
    public string itemDescription;
    public float itemValue;
    public ItemRarity itemRarity;
}
