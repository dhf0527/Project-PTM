using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitUpgrade", menuName = "Scriptable Object/UnitUpgradeData")]
public class UnitUpgradeData : ScriptableObject
{
    public string upgradeName;
    public Sprite upgradeIcon;
    public List<float> upgradeValue;
    public string upgradeDescription;
}
