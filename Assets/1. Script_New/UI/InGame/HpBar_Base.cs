using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class HpBar_Base : MonoBehaviour
{
    [HideInInspector] public Unit unit;

    //체력바 sprite
    [Header("0: 아군 체력바 1: 적군 체력바")]
    [SerializeField] protected Sprite[] hpBar_Sprite = new Sprite[2];
    //체력바
    [SerializeField] protected Image fill_Image;
    //체력 텍스트
    [SerializeField] protected TMP_Text hp_Text;
    [Header("버프 아이콘 위에서부터 0")]
    [SerializeField] protected List<GameObject> buffIcons;

    //체력바가 위치할 높이
    protected float up_Y;

    //체력과 체력바를 연동하는 함수 (BaseUnit.CurHp에서 호출)
    public void SetHpBar()
    {
        fill_Image.fillAmount = (unit.Cur_Hp / unit.Max_Hp);
        if (unit.isHpText)
        {
            hp_Text.text = $"{(int)unit.Cur_Hp} / {(int)unit.Max_Hp}";
        }
    }

    //팀에 따라 체력바 sprite 변경 (BaseUnit.IsTeam에서 호출)
    public void SetHpBarSprite(bool isTeam)
    {
        fill_Image.sprite = isTeam ? hpBar_Sprite[0] : hpBar_Sprite[1];
    }

    //체력바 위치를 설정하는 함수
    public virtual void SetHpPos(float height = 0)
    {
        if (height != 0)
            up_Y = height;
        //체력바 위치 설정
        Vector3 pos = unit.transform.position + Vector3.up * up_Y;
        transform.position = pos;
        //transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
    }

    //버프 아이콘을 활성/비활성화하는 함수
    public void SetBuffIcon(int index, bool isActive)
    {
        buffIcons[index].SetActive(isActive);

        if (isActive)
            unit.DisplayHpBar_Buff();
    }

    protected void Start()
    {
        //체력바 텍스트 설정
        if (unit.isHpText)
            hp_Text.gameObject.SetActive(true);
        BoxCollider2D bc2d = unit.GetComponent<BoxCollider2D>();
        up_Y = (bc2d.offset.y + bc2d.size.y / 2f) * unit.transform.localScale.y + 0.2f;
    }

    protected void Update()
    {
        SetHpPos(up_Y);
    }
}
