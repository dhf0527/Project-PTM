using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase_Unit : Unit
{
    private void Start()
    {
        SetHpBar();
        IsTeam = false;
    }

    private void Update()
    {

    }

    public override void Init()
    {
        //���� ���� ����/����� ���� ���� ����
        if (ud.attack_RangeType == AttackRange.Melee)
            ud.attack_Range = ud.size == Unit_Size.Small ? 0.8f : ud.size == Unit_Size.Medium ? 1f : 1.2f;
        else
            ud.attack_Range = ud.size == Unit_Size.Small ? 2f : ud.size == Unit_Size.Medium ? 2.5f : 3f;

        unitData_st.moveSpeed = ud.move_Speed;
        unitData_st.attackDamage = ud.damage;
        unitData_st.attackSpeed = ud.attack_Speed;
        unitData_st.accuracy = ud.accuracy;
        unitData_st.avoidance = ud.avoidance;
        unitData_st.armor = ud.armor;

        canKnockBack = false;
    }

    //ü�¹� ���� �� ����
    public override void SetHpBar()
    {
        //ü�¹ٸ� world canvas�� ����
        hpBar = Instantiate(WorldCanavsManager.instance.hpBar_Prf, WorldCanavsManager.instance.worldCanvas_Trans);
        //ü�¹� ����
        hpBar.unit = this;
        //ü�¹� ��ġ ����
        hpBar.SetHpPos(2);
        //ü�¹� ũ�� ����
        hpBar.transform.localScale *= 2;

        //ü�� ����
        Cur_Hp = ud.hp;
    }

    public override void Dead()
    {
        Time.timeScale = 0;
        Debug.Log("�¸�");
    }

}
