using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgrade : MonoBehaviour
{
    public List<UnitUpgradeContent> uucs;

    public TMP_Text star_Text;
    public Button upgrade_Button;

    public Image upgrade_Icon;
    public TMP_Text upgrade_Name;
    public TMP_Text upgrade_Cost;
    public TMP_Text upgrade_Detail;

    //선택된 업그레이드 요소들
    public GameObject selectedUpgrade_Go;   
    public List<Sprite> star_Sprites;
    public GameObject selectedFrame;

    UnitUpgradeContent selected_UnitUpgradeContent;

    public int Star
    {
        get { return PlayerPrefs.GetInt("Star"); } 
        set 
        { 
            PlayerPrefs.SetInt("Star", value);
            SetStarText();
            if (selected_UnitUpgradeContent)
                SetUpgrade(selected_UnitUpgradeContent);
        }
    }

    private void Start()
    {
        SetActiveSelectedUpgrade(false);
        SetStarText();
    }

    //선택한 업그레이드의 정보를 띄우는 창을 제어하는 함수
    public void SetActiveSelectedUpgrade(bool isActive)
    {
        selectedUpgrade_Go.SetActive(isActive);
        selectedFrame.SetActive(isActive);
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

        //업그레이드 설명의 {value}를 바꾸는 함수
        string replace_Word = string.Empty;
        for (int i = 0; i < uud.upgradeValue.Count; i++)
        {
            replace_Word += uud.upgradeValue[i].ToString();
            if (i != uud.upgradeValue.Count - 1)
                replace_Word += "/";
        }
        upgrade_Detail.text = uud.upgradeDescription.Replace("{value}", $"({replace_Word})" );

        //스타가 부족하거나 업그레이드 최대치일 때 업그레이드 비활성
        upgrade_Button.interactable = Star >= 2 && uuc_level != uud.upgradeValue.Count;

        //선택한 업그레이드에 강조선(테두리)표시
        selectedFrame.transform.position = uuc.transform.GetChild(1).position;
        selectedFrame.GetComponent<RectTransform>().anchoredPosition += new Vector2(-5, 0);
    }

    public void OnUpgrade()
    {
        if(Star >= 2)
        {
            selected_UnitUpgradeContent.Level++;
            SetUpgrade(selected_UnitUpgradeContent);
            Star -= 2;
            AudioManager.instance.PlayerSfx(SFX_Enum.HeroUpgrade);
            upgrade_Button.GetComponent<Animation>().Stop();
            upgrade_Button.GetComponent<Animation>().Play();
        }
    }

    public void OnResetUpgrade()
    {
        int returnStar = 0;
        foreach (var item in uucs)
             returnStar += item.ResetLevel();

        SetActiveSelectedUpgrade(false);
        Star += returnStar;
    }

    public void SetStarText()
    {
        star_Text.text = "X " + Star.ToString();
    }

    public void Debug_StarPlus()
    {
        Star += 5;
    }

    public void Debug_StarReset()
    {
        Star = 0;
    }
}
