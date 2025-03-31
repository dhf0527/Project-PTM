using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dungeon", menuName = "Scriptable Object/DungeonData")]
public class DungeonData : ScriptableObject
{
    public int stage;
    public int number;
    public UnitData bossUnit;
    public UnitData[] units;
}
