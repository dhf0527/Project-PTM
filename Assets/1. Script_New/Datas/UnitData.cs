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
    public float accuracy;
    //회피율
    public float avoidance;
    //방어도
    public float armor;
    //공격 유형(원,근)
    public AttackRange attack_RangeType;
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
   
    //공격범위
    [HideInInspector] public float attack_Range;
}
