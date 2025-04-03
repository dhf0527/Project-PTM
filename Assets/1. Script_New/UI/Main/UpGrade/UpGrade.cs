using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        foreach (StatBar item in statBars)
        {
            item.SetCostTextAndButton();
        }
        TotalCost = 0;
    }

    //확정 버튼
    public void OnConfirmButton()
    {
        if (MainManager.instance.Soul < totalCost)
        {
            Debug.Log("소울 부족");
            return;
        }

        foreach (StatBar item in statBars)
        {
            item.ConfirmUpgrade();
        }

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

    public void AddTotalCost(int cost)
    {
        TotalCost += cost;
    }
    
    public void SetTotalCostText()
    {
        totalCost_Text.text = TotalCost.ToString();
    }
}
