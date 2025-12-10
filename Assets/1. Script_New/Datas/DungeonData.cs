using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dungeon", menuName = "Scriptable Object/DungeonData")]
public class DungeonData : ScriptableObject
{
    public int stage;
    public int number;
    public Unit bossUnit;   
    public Unit[] units_Wave1;
    public Unit[] units_Wave2;
    public Unit[] units_Wave3;
    public Faction stage_Faction;
    public bool isHard;
}
