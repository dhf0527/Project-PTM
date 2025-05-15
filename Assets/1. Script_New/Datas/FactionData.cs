using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Faction", menuName = "Scriptable Object/FactionData")]
public class FactionData : ScriptableObject
{
    public Sprite faction_Sprite;
    public string factionName;
    public string factionDescription;
}
