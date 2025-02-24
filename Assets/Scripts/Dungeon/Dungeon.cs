using UnityEngine;


[CreateAssetMenu(fileName = "Dungeon", menuName = "Scriptable Object/Dungeon")]
public class Dungeon : ScriptableObject
{
    public int dungeonNum;
    public bool isClear = false;
    public ItemData[] dungeonUnits;
    public ItemData dungeonBoss;
}
