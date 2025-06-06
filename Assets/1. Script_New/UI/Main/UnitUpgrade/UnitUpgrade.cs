using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgrade : MonoBehaviour
{
    public List<UnitUpgradeContent> uucs;

    public GameObject selectedUpgrade_Go;
    public Button upgrade_Button;

    public Image upgrade_Icon;
    public TMP_Text upgrade_Name;
    public TMP_Text upgrade_Cost;
    public TMP_Text upgrade_Detail;
    public List<Image> level_Images;

    public List<Sprite> star_Sprites;

    UnitUpgradeContent selected_UnitUpgradeContent;

    private void Start()
    {
        SetActiveSelectedUpgrade(false);
    }

    //선택한 업그레이드의 정보를 띄우는 창을 제어하는 함수
    public void SetActiveSelectedUpgrade(bool isActive)
    {
        selectedUpgrade_Go.SetActive(isActive);
        upgrade_Button.interactable = isActive;
    }

    //선택한 업그레이드의 정보를 표시하는 함수
    public void SetUpgrade(UnitUpgradeContent uuc)
    {
        selected_UnitUpgradeContent = uuc;
        UnitUpgradeData uud = uuc.unitUpgradeData;
        int uuc_level = PlayerPrefs.GetInt(ReadOnlyData.unitUpgrade + uud.code);

        upgrade_Icon.sprite = uud.upgradeIcon;
        upgrade_Name.text = uud.upgradeName;

        string replace_Word = string.Empty;
        for (int i = 0; i < uud.upgradeValue.Count; i++)
        {
            replace_Word += uud.upgradeValue[i].ToString();
            if (i != uud.upgradeValue.Count - 1)
                replace_Word += "/";
        }
        upgrade_Detail.text = uud.upgradeDescription.Replace("{value}", $"({replace_Word})" );

        //레벨 표시
        for (int i = 0; i < level_Images.Count; i++)
        {
            if(i < uud.upgradeValue.Count)
            {
                level_Images[i].gameObject.SetActive(true);
                if (i < uuc_level)
                    level_Images[i].sprite = star_Sprites[1];
                else
                    level_Images[i].sprite = star_Sprites[0];
            }
            else
                level_Images[i].gameObject.SetActive(false);
        }

        upgrade_Button.interactable = uuc_level != uud.upgradeValue.Count;
    }

    public void OnUpgrade()
    {
        selected_UnitUpgradeContent.Level++;
        SetUpgrade(selected_UnitUpgradeContent);
    }

    public void OnResetUpgrade()
    {
        foreach (var item in uucs)
        {
            item.ResetLevel();
        }
        SetActiveSelectedUpgrade(false);
    }
}
