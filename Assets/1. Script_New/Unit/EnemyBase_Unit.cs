using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase_Unit : Unit
{
    bool isBossSpawn = false;
    public List<float> hp_By_Stage;
    public List<int> armor_By_Stage;

    public override float Cur_Hp
    {
        get
        {
            return cur_Hp;
        }
        set
        {
            cur_Hp = value;
            //사망 체크
            if (cur_Hp <= 0)
            {
                cur_Hp = 0;
                Dead();
            }
            //체력에 따라 다음 웨이브로 넘어감
            else if (Cur_Hp <= Max_Hp * EnemySpawnManager.instance.wave_BaseHps[1] * 0.01f)
                EnemySpawnManager.instance.ToNextWave(2);
            else if (Cur_Hp <= Max_Hp * EnemySpawnManager.instance.wave_BaseHps[0] * 0.01f)
                EnemySpawnManager.instance.ToNextWave(1);

            //체력이 30% 이하가 되면 보스 스폰
            if (cur_Hp <= EnemySpawnManager.instance.boss_BaseHp * 0.01f * Max_Hp)
                EnemySpawnManager.instance.OnBossSpawn();

            //공주일 경우 전용 체력바 갱신
            if (ud.unit_Code == 0)
                DunGeonManager_New.instance.princessHpPanel.SetHpBar(this);

            //체력바 갱신
            hpBar?.SetHpBar();
        }
    }

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
        //유닛 공격 유형/사이즈별 공격 범위 설정
        if (ud.attack_RangeType == AttackRangeType.Melee)
            ud.attack_Range = ud.size == Unit_Size.Small ? 0.8f : ud.size == Unit_Size.Medium ? 1f : 1.2f;
        else
            ud.attack_Range = ud.size == Unit_Size.Small ? 2f : ud.size == Unit_Size.Medium ? 2.5f : 3f;

        canKnockBack = false;
        alwaysDisplayHpbar = true;

        switch (GameManager.Instance.current_Dungeon.stage)
        {
            case 1:
                unitStatData_st.max_Hp_Plus = hp_By_Stage[0] - ud.hp;
                unitStatData_st.armor_Plus = armor_By_Stage[0] - ud.armor;
                break;
            case 2:
                unitStatData_st.max_Hp_Plus = hp_By_Stage[1] - ud.hp;
                unitStatData_st.armor_Plus = armor_By_Stage[1] - ud.armor;
                break;
            case 3:
                unitStatData_st.max_Hp_Plus = hp_By_Stage[2] - ud.hp;
                unitStatData_st.armor_Plus = armor_By_Stage[2] - ud.armor;
                break;
            case 4:
                unitStatData_st.max_Hp_Plus = hp_By_Stage[3] - ud.hp;
                unitStatData_st.armor_Plus = armor_By_Stage[3] - ud.armor;
                break;
            default:
                break;
        }
        Cur_Hp = Max_Hp;
    }

    //체력바 생성 및 설정
    public override void SetHpBar()
    {
        //체력바를 world canvas에 생성
        hpBar = Instantiate(WorldCanavsManager.instance.hpBar_Prf, WorldCanavsManager.instance.worldCanvas_Trans);
        //체력바 연동
        hpBar.unit = this;
        //체력바 위치 설정
        hpBar.SetHpPos(2);
        //체력바 크기 설정
        hpBar.transform.localScale *= 1.5f;

        //체력 설정
        Cur_Hp = Max_Hp;
    }

    public override void Dead()
    {
        GetComponent<Collider2D>().enabled = false;
        DunGeonManager_New.instance.OpenGameClearPanel();
    }

}
