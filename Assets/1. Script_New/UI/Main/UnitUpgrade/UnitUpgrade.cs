using System.Collections;
using System.Collections.Generic;
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

        upgrade_Icon.sprite = uuc.unitUpgradeData.upgradeIcon;
        upgrade_Name.text = uuc.unitUpgradeData.upgradeName;
        upgrade_Detail.text = uuc.unitUpgradeData.upgradeDescription;
    }

    public void OnUpgrade()
    {
        selected_UnitUpgradeContent.Level++;
    }

    public void OnResetUpgrade()
    {
        foreach (var item in uucs)
        {
            item.ResetLevel();
        }
    }
}
