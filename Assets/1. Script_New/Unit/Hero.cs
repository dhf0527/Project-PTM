using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Hero : Unit
{
    [Header("영웅 업그레이드 수치(방어도 제외 %수치)")]
    [SerializeField] float upgradeValue_moveSpeed;
    [SerializeField] float upgradeValue_hp;
    [SerializeField] float upgradeValue_attack;
    [SerializeField] float upgradeValue_attackSpeed;
    [SerializeField] float upgradeValue_accuracy;
    [SerializeField] float upgradeValue_avoidance;
    [SerializeField] int upgradeValue_armor;

    [Header("비전투 회복 회복 (단위=초, 초당 회복량%)")]
    [SerializeField] float recoveryTime;
    [SerializeField] float recoveryAmount;

    [Header("부활 시간")]
    [SerializeField] float reviveCoolTime;

    [SerializeField] ParticleSystem move_Particle;

    [Header("스킬 쿨타임(초)")]
    [SerializeField] float skill1_coolTime;
    [SerializeField] float skill2_coolTime;

    [SerializeField] protected PlayerSkillIcon[] skillIcons = new PlayerSkillIcon[2];
    [SerializeField] protected string skill1_detail;
    [SerializeField] protected string skill2_detail;

    protected float nonCombatTime = 0;

    bool canSkill1 = true;
    bool canSkill2 = true;

    bool isSkilling;

    int skill_1_Count;
    int skill_2_Count;

    #region readOnly
    protected static readonly int DoSkill1 = Animator.StringToHash("doSkill1");
    protected static readonly int DoSkill2 = Animator.StringToHash("doSkill2");

    #endregion

    private void Update()
    {
        if (isDead || isSkilling || isKnockBacking)
        {
            return;
        }

        Move();

        nonCombatTime += Time.deltaTime;
        if (nonCombatTime > recoveryTime)
        {
            nonCombatTime -= 1;
            GetHp(recoveryAmount / 100f * Max_Hp);
        }

        animator.SetFloat("MoveSpeed", MoveSpeed / 200);
        animator.SetFloat("AttackSpeed", Mathf.Max(AttackSpeed / ud.attack_Speed, 1));
    }


    public override void Init()
    {
        base.Init();
        IsTeam = true;
        moveDir = Vector3.zero;
        knockBack_Count = 0;

        unitStatData_st.armor_Plus += (upgradeValue_armor * PlayerPrefs.GetInt(ConstData.statGrade + "0"));
        unitStatData_st.max_Hp_Plus += (ud.hp * upgradeValue_hp * 0.01f * PlayerPrefs.GetInt(ConstData.statGrade + "1"));
        unitStatData_st.attack_Plus += (ud.damage * upgradeValue_attack * 0.01f * PlayerPrefs.GetInt(ConstData.statGrade + "2"));
        unitStatData_st.attackSpeed_Plus += (ud.attack_Speed * upgradeValue_attackSpeed * 0.01f * PlayerPrefs.GetInt(ConstData.statGrade + "3"));
        unitStatData_st.moveSpeed_Plus += (ud.move_Speed * upgradeValue_moveSpeed * 0.01f * PlayerPrefs.GetInt(ConstData.statGrade + "4"));
        unitStatData_st.accuracy_Plus += ((int)(ud.accuracy * upgradeValue_accuracy * 0.01f) * PlayerPrefs.GetInt(ConstData.statGrade + "5"));
        unitStatData_st.avoidance_Plus += ((int)(ud.avoidance * upgradeValue_avoidance * 0.01f) * PlayerPrefs.GetInt(ConstData.statGrade + "6"));
        Cur_Hp = Max_Hp;

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
            unitStatData_st.max_Hp_Plus += 100;
            unitStatData_st.attack_PlusPercent += 100;
            unitStatData_st.armor_PlusPercent += 100;
            unitStatData_st.attackSpeed_PlusPercent += 100;
        }
        #endregion
    }

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
            move_Particle.Stop();

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

        if (!move_Particle.isPlaying)
            move_Particle.Play();
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
        DunGeonManager_New.instance.PrincessCoolDown(reviveCoolTime);
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

    //스킬1
    public virtual void OnSkill1()
    {
        if (skill_1_Count++ < 2)
        {
            if (EnemySpawnManager.instance.cor_warn != null)
            {
                StopCoroutine(EnemySpawnManager.instance.cor_warn);
                EnemySpawnManager.instance.cor_warn = null;
            }
            EnemySpawnManager.instance.cor_warn = StartCoroutine(EnemySpawnManager.instance.C_SetWarnText(4f));

            EnemySpawnManager.instance.warnText_Text.text = skill1_detail;
        }
    }

    protected IEnumerator C_Skill1_CoolDown()
    {
        canSkill1 = false;
        float cur_Time = skill1_coolTime;

        while (cur_Time > 0)
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

    //스킬2 버튼을 누르면 호출
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
    public virtual void OnSkill2()
    {
        if (skill_2_Count++ < 2)
        {
            if (EnemySpawnManager.instance.cor_warn != null)
            {
                StopCoroutine(EnemySpawnManager.instance.cor_warn);
                EnemySpawnManager.instance.cor_warn = null;
            }
            EnemySpawnManager.instance.cor_warn = StartCoroutine(EnemySpawnManager.instance.C_SetWarnText(4f));
            EnemySpawnManager.instance.warnText_Text.text = skill2_detail;
        }
    }

    protected IEnumerator C_Skill2_CoolDown()
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
