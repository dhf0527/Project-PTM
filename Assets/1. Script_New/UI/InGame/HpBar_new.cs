using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar_new : MonoBehaviour
{
    [HideInInspector] public BaseUnit unit;

    //ü�¹� sprite
    [Header("0: �Ʊ� ü�¹� 1: ���� ü�¹�")]
    [SerializeField] Sprite[] hpBar_Sprite = new Sprite[2];
    //ü�¹�
    [SerializeField] Image fill_Image;

    //ü�°� ü�¹ٸ� �����ϴ� �Լ� (BaseUnit.CurHp���� ȣ��)
    public void SetHpBar()
    {
        fill_Image.fillAmount = (unit.Cur_Hp / unit.ud.hp);
    }

    //���� ���� ü�¹� sprite ���� (BaseUnit.IsTeam���� ȣ��)
    public void SetHpBarSprite(bool isTeam)
    {
        fill_Image.sprite = isTeam ? hpBar_Sprite[0]:hpBar_Sprite[1];
    }
}
