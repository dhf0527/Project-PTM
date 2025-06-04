using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgradeContent : MonoBehaviour
{
    public UnitUpgradeData unitUpgradeData;

    public Image icon_Image;
    public TMP_Text upgradeName_Text;

    int level;
    public int Level
    {
        get { return level; }
        set 
        { 
            level = value;
            SetLevel(level);
        }
    }

    private void Start()
    {
        SetUpgradeData(unitUpgradeData);
    }

    public void SetUpgradeData(UnitUpgradeData uud)
    {
        icon_Image.sprite = uud.upgradeIcon;
        upgradeName_Text.text = uud.upgradeName;
    }

    public void SetLevel(int value)
    {

    }
}
