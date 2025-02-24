using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public Dungeon[] dungeons;



    public void SetDungeon()
    {
        PlayerPrefs.SetInt("isClear", 0);
    }

}
