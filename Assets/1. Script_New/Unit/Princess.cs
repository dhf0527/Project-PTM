using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Princess : Unit
{
    [Header("방패 강타 데미지 비율(%)")]
    [SerializeField] float skill1_Damage;
    [SerializeField] float skill1_coolTime;
    [SerializeField] float skill1_buff_Time;
    [SerializeField] float skill1_move_Decrease;
    [SerializeField] float skill1_attackSpeed_Decrease;

    [SerializeField] float skill2_coolTime;
    [SerializeField] float skill2_buff_Time;
    [SerializeField] int skill2_accuracy_increase;
    [SerializeField] PlayerSkillIcon[] skillIcons = new PlayerSkillIcon[2];

    float nonCombatTime = 0;

    bool canSkill1 = true;
    bool canSkill2 = true;

    bool isSkilling;

    bool test_Is1;
    bool test_Is2;
    bool test_Is3;
    bool test_Is4;

    private void Update()
    {
        Test();

        if (isDead || isSkilling || isKnockBacking)
        {
            return;
        }

        Move();

        nonCombatTime += Time.deltaTime;
        if (nonCombatTime > 4f)
        {
            nonCombatTime -= 1;
            GetHp((4 + (EnemySpawnManager.instance.cur_Wave + 1) * 4) / 100f * Max_Hp);
        }

        animator.SetFloat("MoveSpeed", MoveSpeed / 200);
    }

    //테스트 함수
    void Test()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            unitStatData_st.moveSpeed_PlusPercent += !test_Is1 ? 400 : -400f;
            test_Is1 = !test_Is1;
            Debug.Log("이동 속도 증가" + (test_Is1 ? "On" : "Off"));
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            unitStatData_st.accuracy_Plus += !test_Is2 ? 1000 : -1000;
            test_Is2 = !test_Is2;
            Debug.Log("명중률 증가" + (test_Is2 ? "On" : "Off"));
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            unitStatData_st.avoidance_Plus += !test_Is3 ? 1000 : -1000;
            test_Is3 = !test_Is3;
            Debug.Log("회피율 증가" + (test_Is3 ? "On" : "Off"));
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            unitStatData_st.attack_Plus += !test_Is4 ? 1000 : - 1000;
            test_Is4 = !test_Is4;
            Debug.Log("공격력 증가" + (test_Is4 ? "On" : "Off"));
        }
        if (Input.GetKeyDown(KeyCode.F5))
            DunGeonManager_New.instance.Cur_Gold = DunGeonManager_New.instance.Max_Gold;
        if (Input.GetKeyDown(KeyCode.F6))
            DunGeonManager_New.instance.OpenGameClearPanel();
        if (Input.GetKeyDown(KeyCode.F7))
            DunGeonManager_New.instance.OpenGameOverPanel();
    }

    public override void Init()
    {
        base.Init();
        IsTeam = true;
        moveDir = Vector3.zero;
        knockBack_Count = 0;

        unitStatData_st.armor_Plus += (2 * PlayerPrefs.GetInt(ConstData.statGrade + "0"));
        unitStatData_st.max_Hp_Plus += (ud.hp * 0.1f * PlayerPrefs.GetInt(ConstData.statGrade + "1"));
        unitStatData_st.attack_Plus += (ud.damage * 0.1f * PlayerPrefs.GetInt(ConstData.statGrade + "2"));
        unitStatData_st.attackSpeed_Plus += (ud.attack_Speed * 0.1f * PlayerPrefs.GetInt(ConstData.statGrade + "3"));
        unitStatData_st.moveSpeed_Plus += (ud.move_Speed * 0.1f * PlayerPrefs.GetInt(ConstData.statGrade + "4"));
        unitStatData_st.accuracy_Plus += ((int)(ud.accuracy * 0.1f) * PlayerPrefs.GetInt(ConstData.statGrade + "5"));
        unitStatData_st.avoidance_Plus += ((int)(ud.avoidance * 0.1f) * PlayerPrefs.GetInt(ConstData.statGrade + "6"));

        #region 식사 효과
        //불사조 닭발
        if (GameManager.Instance.current_Meal?.code == 100)
            unitStatData_st.attack_PlusPercent += GameManager.Instance.current_Meal.mealValue;
        //든든 국밥
        else if (GameManager.Instance.current_Meal?.code == 101)
        {
            unitStatData_st.avoidance_Plus += (int)GameManager.Instance.current_Meal.mealValue;
            unitStatData_st.accuracy_Plus += (int)GameManager.Instance.current_Meal.mealValue2;
        }
        //크라운 스테이크
        else if (GameManager.Instance.current_Meal?.code == 103)
        {
            unitStatData_st.totalDamage_PlusPercent += 100;
            unitStatData_st.totalDamageReduction_PlusPercent += 50;
        }
        #endregion
    }

    #region readOnly
    protected static readonly int DoSkill1 = Animator.StringToHash("doSkill1");
    protected static readonly int DoSkill2 = Animator.StringToHash("doSkill2");

    #endregion

    #region 공격
    protected override void Attack()
    {
        nonCombatTime = 0;
        base.Attack();
    }
    #endregion

    #region 피격
    //넉백 함수
    public override IEnumerator KnockBack()
    {
        nonCombatTime = 0;
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        //공격 중이었을 경우 공격 종료 처리
        if (cur_State == AnimState.Attack)
            OnEndAttack();

        //스킬 종료 처리
        OnEndSkill();

        //0.75초동안 넉백
        SetAnim(AnimState.Hit);
        isKnockBacking = true;

        float knockTime = 0;
        float knockSpeed;
        while (knockTime < 0.75f)
        {
            knockTime += Time.deltaTime;
            //가속도 보정
            knockSpeed = Mathf.Lerp((1.5f / 0.75f), 0, (knockTime / 0.75f));
            //경계선 보정
            SetBoundary();
            Vector3 tmp_Vec = transform.position + (Vector3.left) * knockSpeed * Time.deltaTime;
            tmp_Vec.x = Mathf.Clamp(tmp_Vec.x, boundary_Min_x, boundary_Max_x);
            //이동
            transform.position = tmp_Vec;
            yield return new WaitForEndOfFrame();
        }

        isKnockBacking = false;
    }

    public override void TakeDamage(float damage, bool isCanKnockBackDamage = true)
    {
        nonCombatTime = 0;
        base.TakeDamage(damage, isCanKnockBackDamage);
    }
    #endregion

    #region 이동 함수
    //버튼을 눌렀을 때 이동 방향/속도 설정
    public void OnMove(int move_dir)
    {
        if (isDead || isSkilling)
            return;

        DunGeonManager_New.instance.cameraMove.isChasePrincess = true;
        moveDir = Vector3.right * move_dir;
        SetDir();
        OnEndAttack();
    }

    protected override void Move()
    {
        //이동량이 없으면 이동을 안 함
        if (!isMoving || moveDir.x == 0)
        {
            ScanEnemy();
            //스캔된 적이 있다면 공격
            if (hit.collider != null)
            {
                Attack();
            }

            //공격중이 아닐 때 idle 애니메이션 재생
            if (!isAttacking)
            {
                SetAnim(AnimState.Idle);
            }
            return;
        }

        //이동할 방향을 바라보기
        SetDir();

        //임시 이동
        Vector3 tmp_vec = transform.position + moveDir * Time.deltaTime * (MoveSpeed / 200f);
        //경계선을 넘지 않도록 보정
        SetBoundary();
        float clamped_x = Mathf.Clamp(tmp_vec.x, boundary_Min_x, boundary_Max_x);
        tmp_vec.x = clamped_x;
        //실제 이동
        transform.position = tmp_vec;

        //이동 애니메이션
        SetAnim(AnimState.Move);
    }
    #endregion

    #region 사망 처리 함수
    public override void Dead()
    {
        SetAnim(AnimState.Die);
        GetComponent<Collider2D>().enabled = false;
        isDead = true;
        isSkilling = false;

        moveDir.x = 0;

        //버프 제거
        Buff[] buffs = GetComponents<Buff>();
        foreach (var buff in buffs)
        {
            if (buff.unit)
                buff.BuffEnd();
            else
                Destroy(buff);
        }

        AudioManager.Instance.PlayerSfx(SFX_Enum.HeroDie);
    }

    public override void OnDead()
    {
        DunGeonManager_New.instance.PrincessCoolDown();
        DunGeonManager_New.instance.cameraMove.isPrincessDead = true;

        //안보이는 곳으로 옮기기
        transform.position = new Vector3(-15, 0, 0);
    }

    public void Rivive()
    {
        Cur_Hp = Max_Hp;
        isDead = false;
        GetComponent<Collider2D>().enabled = true;
        SetAnim(AnimState.Idle);

        AudioManager.Instance.PlayerSfx(SFX_Enum.HeroRevive);
    }
    #endregion

    #region 스킬 함수

    //스킬1 버튼을 누르면 호출
    public void OnTrySkill1()
    {
        if (isDead)
            return;

        if (canSkill1 && !isSkilling)
        {
            isSkilling = true;
            SetAnim(AnimState.Skill1);
        }
    }

    //공주 스킬1: 방패 강타
    public void OnSkill1()
    {
        nonCombatTime = 0;
        float skillRange = 1.5f;

        //스캔할 레이어 설정
        string target_Layer = EnemyLayer;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, skillRange, LayerMask.GetMask(target_Layer));

        foreach (var item in hits)
        {
            Unit target_Unit = item.collider.GetComponent<Unit>();
            //넉백
            target_Unit.OnStartKnockBack();
            //데미지 부여
            float dmg = (skill1_Damage * 0.01f) * AttackDamage * (1+ unitStatData_st.attackBoost_PlusPercent * 0.01f);
            ApplyAttack(target_Unit, dmg, AttackType.Physical);
            //디버프 처리
            ShieldSmiteDebuff shieldSmiteDebuff = target_Unit.AddComponent<ShieldSmiteDebuff>();
            shieldSmiteDebuff.buff_Time = skill1_buff_Time;
            shieldSmiteDebuff.attackSpeed_Decrease = skill1_attackSpeed_Decrease;
            shieldSmiteDebuff.move_Decrease = skill1_move_Decrease;
        }
        StartCoroutine(C_Skill1_CoolDown());

        AudioManager.Instance.PlayerSfx(SFX_Enum.ShieldSmite);
    }

    IEnumerator C_Skill1_CoolDown()
    {
        canSkill1 = false;
        float cur_Time = skill1_coolTime;

        while(cur_Time > 0)
        {
            cur_Time -= Time.deltaTime;
            skillIcons[0].SetDelayImage(cur_Time / skill1_coolTime);
            yield return new WaitForEndOfFrame();
        }

        canSkill1 = true;
    }

    public void OnEndSkill()
    {
        isSkilling = false;
    }

    //스킬1 버튼을 누르면 호출
    public void OnTrySkill2()
    {
        if (isDead)
            return;

        if (canSkill2 && !isSkilling)
        {
            isSkilling = true;
            SetAnim(AnimState.Skill2);
        }
    }

    //공주 스킬2: 부러진 영웅검
    public void OnSkill2()
    {
        nonCombatTime = 0;
        float skillRange = 1f;

        //스캔할 레이어 설정
        string target_Layer = TeamLayer;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + Vector3.left * skillRange, Vector2.right, skillRange * 2, LayerMask.GetMask(target_Layer));

        foreach (var item in hits)
        {
            if (item.collider.GetComponent<TeamBase_Unit>())
                continue;

            Unit target_Unit = item.collider.GetComponent<Unit>();
            //버프 처리
            BrokenHeroSword brokenHeroSword = target_Unit.AddComponent<BrokenHeroSword>();
            brokenHeroSword.buff_Time = skill2_buff_Time;
            brokenHeroSword.accuracy_increase = skill2_accuracy_increase;
        }
        StartCoroutine(C_Skill2_CoolDown());

        AudioManager.Instance.PlayerSfx(SFX_Enum.BrokenHeroSword);
    }

    IEnumerator C_Skill2_CoolDown()
    {
        canSkill2 = false;
        float cur_Time = skill2_coolTime;

        while (cur_Time > 0)
        {
            cur_Time -= Time.deltaTime;
            skillIcons[1].SetDelayImage(cur_Time / skill2_coolTime);
            yield return new WaitForEndOfFrame();
        }

        canSkill2 = true;
    }
    #endregion

    protected void SetAnim(AnimState animState)
    {
        //이미 해당 애니메이션 재생중이면 실행하지 않음
        if (cur_State == animState)
            return;

        //현재 애니메이션 상태를 설정
        cur_State = animState;
        //해당 애니메이션 재생
        switch (animState)
        {
            case AnimState.Idle:
                animator.SetTrigger(DoStop);
                break;
            case AnimState.Move:
                animator.SetTrigger(DoMove);
                break;
            case AnimState.Attack:
                animator.SetTrigger(DoAttack);
                break;
            case AnimState.Hit:
                animator.SetTrigger(DoHit);
                break;
            case AnimState.Die:
                animator.SetTrigger(DoDie);
                break;
            case AnimState.Skill1:
                animator.SetTrigger(DoSkill1);
                break;
            case AnimState.Skill2:
                animator.SetTrigger(DoSkill2);
                break;
            default:
                break;
        }
    }
}
