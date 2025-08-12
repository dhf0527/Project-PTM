using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    public Button upgradeButton;
    public TMP_Text cost_Text;
    public List<Image> statCell_Images;
    [Header("0:확정 1:임시")]
    public List<Sprite> cell_Sprites;

    //확정 전 임시 레벨
    int tmpGrade = 0;
    //확정된 레벨
    int grade = 0;
     public int Grade
    {
        get { return grade; }
        set
        {
            grade = value;
            tmpGrade = grade;
        }
    }
    //업그레이드 시 사용될 비용
    int cost = 500;

    //레벨에 따라 표시하는 함수
    public void SetCell()
    {
        for (int i = 0; i < statCell_Images.Count; i++)
        {
            if(i < Grade)
            {
                statCell_Images[i].gameObject.SetActive(true);
                statCell_Images[i].sprite = cell_Sprites[0];
            }
            else if(i < tmpGrade)
            {
                statCell_Images[i].gameObject.SetActive(true);
                statCell_Images[i].sprite = cell_Sprites[1];
            }
            else
            {
                statCell_Images[i].gameObject.SetActive(false);

            }
        }
    }

    //업그레이드 버튼을 눌렀을 때 호출
    public void OnUpgrade()
    {
        UpGrade.instance.AddTotalCost(cost);

        tmpGrade++;
        SetCostTextAndButton();
        SetCell();

        AudioManager.Instance.PlayerSfx(SFX_Enum.Touch);
    }

    //레벨에 따라 코스트를 표시
    public void SetCostTextAndButton()
    {
        SetCell();

        if (tmpGrade >= statCell_Images.Count)
        {
            upgradeButton.interactable = false;
            cost_Text.text = "MAX";
            //AudioManager.Instance.PlayerSfx(SFX_Enum.Touch);
        }
        else
        {
            upgradeButton.interactable = true;
            cost = ((int)Mathf.Pow(2, tmpGrade) * 500);
            cost_Text.text = cost.ToString();
        }
    }

    //업그레이드 확정 버튼을 눌렀을 때 호출
    public void ConfirmUpgrade()
    {
        Grade = tmpGrade;
        SetCell();
        cost = ((int)Mathf.Pow(2, tmpGrade) * 500);
    }

    //초기화 버튼을 눌렀을 때 호출
    public void ReturnUpgrade()
    {
        tmpGrade = Grade;
        SetCell();
        cost = 0;
    }
}
