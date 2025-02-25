using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Object/UnitData")]

public class UnitData : ScriptableObject
{
    //��������Ʈ
    public Sprite unit_Sprite;
    //�̸�
    public string unit_Name;
    //�ڵ�
    public int unit_Code;
    //���� �ڵ�
    public Faction faction;
    //������
    public int size;
    //������ �رݵǴ� ����
    public int level;
    //���� ���
    public int cost;
    //ü��
    public float hp;
    //�̵� �ӵ�
    public float move_Speed;
    //���ݷ�
    public float damage;
    //���ݼӵ�
    public float attack_Speed;
    //���� ����(��,��)
    public AttackRange attack_RangeType;
    //���ݹ���
    public float attack_Range;
    //�ִ� ���� ��
    public int multi_Hit;
    //���� ����(��,��)
    public AttackType attack_Type;
    //���߷�
    public float accuracy;
    //ȸ����
    public float avoidance;
    //��
    public float armor;
    //���� ���� ����
    public AttackType resistance_Type;
    //��� ���� ����
    public AttackType weak_Type;
}
