using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar_new : MonoBehaviour
{
    [HideInInspector] public Unit unit;

    //ü�¹� sprite
    [Header("0: �Ʊ� ü�¹� 1: ���� ü�¹�")]
    [SerializeField] Sprite[] hpBar_Sprite = new Sprite[2];
    //ü�¹�
    [SerializeField] Image fill_Image;
    //ü�� �ؽ�Ʈ
    [SerializeField] TMP_Text hp_Text;

    //ü�¹ٰ� ��ġ�� ����
    float up_Y = 1.2f;

    //ü�°� ü�¹ٸ� �����ϴ� �Լ� (BaseUnit.CurHp���� ȣ��)
    public void SetHpBar()
    {
        fill_Image.fillAmount = (unit.Cur_Hp / unit.ud.hp);
        if (unit.isHpText)
        {
            hp_Text.text = $"{unit.Cur_Hp / 1} / {unit.ud.hp / 1}";
        }
    }

    //���� ���� ü�¹� sprite ���� (BaseUnit.IsTeam���� ȣ��)
    public void SetHpBarSprite(bool isTeam)
    {
        fill_Image.sprite = isTeam ? hpBar_Sprite[0]:hpBar_Sprite[1];
    }

    //ü�¹� ��ġ�� �����ϴ� �Լ�
    public void SetHpPos(float height)
    {
        up_Y = height;
        //ü�¹� ��ġ ����
        transform.position = unit.transform.position + Vector3.up * up_Y;
    }

    private void Start()
    {
        //ü�¹� �ؽ�Ʈ ����
        if (unit.isHpText)
            hp_Text.gameObject.SetActive(true);
    }

    private void Update()
    {
        SetHpPos(up_Y);
    }
}
