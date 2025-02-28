using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Object/UnitData")]

public class UnitData : ScriptableObject
{
    //��������Ʈ
    public Sprite unit_Sprite;
    //�ڵ�
    public int unit_Code;
    //������ �رݵǴ� ����
    public int level;
    //�̸�
    public string unit_Name;
    //���� �ڵ�
    public Faction faction;
    //�̵� �ӵ�
    public float move_Speed;
    //�����
    public float hp;
    //���ݷ�
    public float damage;
    //���ݼӵ�
    public float attack_Speed;
    //���߷�
    public float accuracy;
    //ȸ����
    public float avoidance;
    //��
    public float armor;
    //���� ����(��,��)
    public AttackRange attack_RangeType;
    //�ִ� ���� ��
    public int target_Count;
    //���� ����(��,��)
    public AttackType attack_Type;
    //��� ���� ����
    public AttackType weak_Type;
    //���� ���� ����
    public AttackType resistance_Type;
    //���� ��
    public int spawn_Count;
    //������
    public Unit_Size size;
    //���� ���
    public int cost;
   
    //���ݹ���
    [HideInInspector] public float attack_Range;
}
