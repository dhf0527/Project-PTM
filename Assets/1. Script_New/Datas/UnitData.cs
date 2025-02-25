using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Object/UnitData")]

public class UnitData : ScriptableObject
{
    //스프라이트
    public Sprite unit_Sprite;
    //이름
    public string unit_Name;
    //코드
    public int unit_Code;
    //세력 코드
    public Faction faction;
    //사이즈
    public int size;
    //유닛이 해금되는 레벨
    public int level;
    //생산 비용
    public int cost;
    //체력
    public float hp;
    //이동 속도
    public float move_Speed;
    //공격력
    public float damage;
    //공격속도
    public float attack_Speed;
    //공격 유형(원,근)
    public AttackRange attack_RangeType;
    //공격범위
    public float attack_Range;
    //최대 공격 수
    public int multi_Hit;
    //피해 유형(물,마)
    public AttackType attack_Type;
    //명중률
    public float accuracy;
    //회피율
    public float avoidance;
    //방어도
    public float armor;
    //저항 피해 유형
    public AttackType resistance_Type;
    //취약 피해 유형
    public AttackType weak_Type;
}
