using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar_new : MonoBehaviour
{
    [HideInInspector] public Unit unit;

    //체력바 sprite
    [Header("0: 아군 체력바 1: 적군 체력바")]
    [SerializeField] Sprite[] hpBar_Sprite = new Sprite[2];
    //체력바
    [SerializeField] Image fill_Image;
    //체력 텍스트
    [SerializeField] TMP_Text hp_Text;
    [Header("버프 아이콘 위에서부터 0")]
    [SerializeField] List<GameObject> buffIcons;

    //체력바가 위치할 높이
    float up_Y = 1.2f;

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
        fill_Image.sprite = isTeam ? hpBar_Sprite[0]:hpBar_Sprite[1];
    }

    //체력바 위치를 설정하는 함수
    public void SetHpPos(float height)
    {
        up_Y = height;
        //체력바 위치 설정
        transform.position = unit.transform.position + Vector3.up * up_Y;
    }

    //버프 아이콘을 활성/비활성화하는 함수
    public void SetBuffIcon(int index, bool isActive)
    {
        buffIcons[index].SetActive(isActive);
    }

    private void Start()
    {
        //체력바 텍스트 설정
        if (unit.isHpText)
            hp_Text.gameObject.SetActive(true);


        up_Y = unit.GetComponent<BoxCollider2D>().bounds.max.y + 0.2f;
    }

    private void Update()
    {
        SetHpPos(up_Y);
    }
}
