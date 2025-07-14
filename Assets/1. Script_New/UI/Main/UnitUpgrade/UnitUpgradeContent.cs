using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgradeContent : MonoBehaviour
{
    public UnitUpgradeData unitUpgradeData;
    public UnitUpgrade unitUpgrade;

    public List<Sprite> star_Sprites;
    public Image icon_Image;
    public TMP_Text upgradeName_Text;
    public List<Image> level_Images;

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
        GetComponent<Button>().onClick.AddListener(() => unitUpgrade.SetUpgrade(this));

        Level = PlayerPrefs.GetInt(ReadOnlyData.unitUpgrade + unitUpgradeData.code.ToString());
    }

    public void SetUpgradeData(UnitUpgradeData uud)
    {
        icon_Image.sprite = uud.upgradeIcon;
        upgradeName_Text.text = uud.upgradeName;
    }

    public void SetLevel(int value)
    {
        PlayerPrefs.SetInt(ReadOnlyData.unitUpgrade + unitUpgradeData.code.ToString(), value);

        //레벨 이미지 설정
        for (int i = 0; i < level_Images.Count; i++)
        {
            if(i < Level)
            {
                level_Images[i].sprite = star_Sprites[1];
            }
            else
            {
                level_Images[i].sprite = star_Sprites[0];
            }
        }
    }

    public int ResetLevel()
    {
        Level = PlayerPrefs.GetInt(ReadOnlyData.unitUpgrade + unitUpgradeData.code.ToString());
        int returnValue = Level * 2;
        Level = 0;
        return returnValue;
    }
}
