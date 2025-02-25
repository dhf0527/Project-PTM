using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar_new : MonoBehaviour
{
    [HideInInspector] public BaseUnit unit;

    //체력바 sprite
    [Header("0: 아군 체력바 1: 적군 체력바")]
    [SerializeField] Sprite[] hpBar_Sprite = new Sprite[2];
    //체력바
    [SerializeField] Image fill_Image;

    //체력과 체력바를 연동하는 함수 (BaseUnit.CurHp에서 호출)
    public void SetHpBar()
    {
        fill_Image.fillAmount = (unit.Cur_Hp / unit.ud.hp);
    }

    //팀에 따라 체력바 sprite 변경 (BaseUnit.IsTeam에서 호출)
    public void SetHpBarSprite(bool isTeam)
    {
        fill_Image.sprite = isTeam ? hpBar_Sprite[0]:hpBar_Sprite[1];
    }
}
