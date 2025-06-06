using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpGrade : MonoBehaviour
{
    public static UpGrade instance;

    public List<StatBar> statBars;
    public TMP_Text totalCost_Text;

    

    int totalCost;
    public int TotalCost
    {
        get
        {
            return totalCost;
        }
        set
        {
            totalCost = value;
            //비용 표시
            SetTotalCostText();
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < statBars.Count; i++)
        {
            //강화 수치 로드
            statBars[i].Grade = PlayerPrefs.GetInt(ReadOnlyData.statGrade + i.ToString());
            //비용 설정 및 버튼 활성화
            statBars[i].SetCostTextAndButton();
        }

        TotalCost = 0;
    }

    //확정 버튼
    public void OnConfirmButton()
    {
        if (MainManager.instance.Soul < totalCost)
        {
            MainManager.instance.FloatMessage("소울이 부족합니다.");

            AudioManager.instance.PlayerSfx(SFX_Enum.Touch);
            return;
        }

        for (int i = 0; i < statBars.Count; i++)
        {
            //업그레이드 확정
            statBars[i].ConfirmUpgrade();
            //강화 수치 저장
            PlayerPrefs.SetInt(ReadOnlyData.statGrade + i.ToString(), statBars[i].Grade);
        }

        if(totalCost == 0)
            AudioManager.instance.PlayerSfx(SFX_Enum.Touch);
        else
            AudioManager.instance.PlayerSfx(SFX_Enum.HeroUpgrade);

        MainManager.instance.Soul -= totalCost;
        TotalCost = 0;
    }

    //되돌리기 버튼
    public void OnReturnButton()
    {
        foreach (StatBar item in statBars)
        {
            item.ReturnUpgrade();
            item.SetCostTextAndButton();
        }
        TotalCost = 0;
    }

    public void ResetUpgrade()
    {
        for (int i = 0; i < statBars.Count; i++)
        {
            //강화 수치 초기화
            PlayerPrefs.SetInt(ReadOnlyData.statGrade + i.ToString(), 0);

            //비용 반환
            MainManager.instance.Soul += (((int)Mathf.Pow(2, statBars[i].Grade) - 1) * 500);
           
            statBars[i].Grade = 0;
            statBars[i].SetCostTextAndButton();
            statBars[i].SetCell();
        }
        TotalCost = 0;
    }

    public void AddTotalCost(int cost)
    {
        TotalCost += cost;
    }
    
    public void SetTotalCostText()
    {
        totalCost_Text.text = TotalCost.ToString();
    }

    
}
