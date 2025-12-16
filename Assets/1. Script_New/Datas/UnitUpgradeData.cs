using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitUpgrade", menuName = "Scriptable Object/UnitUpgradeData")]
public class UnitUpgradeData : ScriptableObject
{
    public int code;
    public string upgradeName;
    public Sprite upgradeIcon;
    public int upgradeCost;
    public List<float> upgradeValue;
    public List<float> upgradeValue2;
    [TextArea(3,5)]
    public string upgradeDescription;
}
