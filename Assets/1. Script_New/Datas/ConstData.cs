using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstData
{
    //PlayerPrefs에서 사용될 이름 ("이름" + "번호")
    
    public const string statGrade = "statGrade";
    public const string unitUpgrade = "unitUpgrade";
    public const string dungeonClearTime = "dungeonClear";  //ConstData.dungeonClearTime + $"{dd.stage},{dd.number}"
    public const string mealCompleteTime = "mealCompleteTime";
    public const string inActiveDamageText = "inActiveDamageText";

    public const string unitItem_Unlock = "unitItem_Unlock";
    public const string heroUpgrade_Unlock = "heroUpgrade_Unlock";
    public const string meal_Unlock = "meal_Unlock";
    public const string unitUpgrade_Unlock = "unitUpgrade_Unlock";
    public const string pedia_Unlock = "pedia_Unlock";
    public const string new_Unlock = "new_Unlock";
    public const string tutorialReady = "tutorialRready";    //ConstData.tutorialReady + TutorialKey.Dungeon_1

    public const string skillCount1 = "skillCount1";    //ConstData.skillCount1 + ud.unit_Code
    public const string skillCount2 = "skillCount2";
}
