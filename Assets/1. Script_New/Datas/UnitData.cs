using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Object/UnitData")]

public class UnitData : ScriptableObject
{
    //스프라이트
    public Sprite unit_Sprite;
    //코드
    public int unit_Code;
    //유닛이 해금되는 레벨
    public int level;
    //이름
    public string unit_Name;
    public string unit_SubName;
    //세력 코드
    public Faction faction;
    //이동 속도
    public float move_Speed;
    //생명력
    public float hp;
    //공격력
    public float damage;
    //공격속도
    public float attack_Speed;
    //명중률
    public int accuracy;
    //회피율
    public int avoidance;
    //방어도
    public int armor;
    //공격 유형(원,근)
    public AttackRangeType attack_RangeType;
    //최대 공격 수
    public int target_Count;
    //피해 유형(물,마)
    public AttackType attack_Type;
    //취약 피해 유형
    public AttackType weak_Type;
    //저항 피해 유형
    public AttackType resistance_Type;
    //생산 수
    public int spawn_Count;
    //사이즈
    public Unit_Size size;
    //생산 비용
    public int cost;
    //고유 특성1 이름
    public string passive1;
    //고유 특성1 설명
    [TextArea]
    public string passive1_Detail;
    //고유 특성2 이름
    public string passive2;
    //고유 특성2 설명\
    [TextArea]
    public string passive2_Detail;

    //공격범위
    public float attack_Range;

    public bool isHardMode;
}
