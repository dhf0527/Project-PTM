using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Princess : Unit
{
    [SerializeField] float skill1_coolTime;
    [SerializeField] float skill2_coolTime;
    [SerializeField] PlayerSkillIcon[] skillIcons = new PlayerSkillIcon[2];

    bool canSkill1 = true;
    bool canSkill2 = true;

    bool isSkilling;

    private void Update()
    {
        Test();

        if (isDead || isSkilling || isKnockBacking)
            return;

        Move();
        
    }

    //테스트 함수
    void Test()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            unitData_st.moveSpeed = 4f;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            unitData_st.moveSpeed = 1f;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            unitData_st.accuracy = 1000;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            unitData_st.accuracy = 60;
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            unitData_st.avoidance = 1000;
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            unitData_st.avoidance = 40;
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            DunGeonManager_New.instance.Cur_Gold = DunGeonManager_New.instance.Max_Gold;
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            Cur_Hp = 0;
        }
        if (Input.GetKeyDown(KeyCode.F9))
            DunGeonManager_New.instance.OpenGameClearPanel();
        if (Input.GetKeyDown(KeyCode.F10))
            DunGeonManager_New.instance.OpenGameOverPanel();
    }

    public override void Init()
    {
        base.Init();
        IsTeam = true;
        moveDir = Vector3.zero;
    }

    #region readOnly
    protected static readonly int DoSkill1 = Animator.StringToHash("doSkill1");
    protected static readonly int DoSkill2 = Animator.StringToHash("doSkill2");

    #endregion

    #region 피격
    //넉백 함수
    public override IEnumerator KnockBack()
    {
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
            Vector3 tmp_Vec = transform.position + (-moveDir.normalized) * knockSpeed * Time.deltaTime;
            tmp_Vec.x = Mathf.Clamp(tmp_Vec.x, boundary_Min_x, boundary_Max_x);
            //이동
            transform.position = tmp_Vec;
            yield return new WaitForEndOfFrame();
        }

        isKnockBacking = false;
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
        Vector3 tmp_vec = transform.position + moveDir * Time.deltaTime * unitData_st.moveSpeed;
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
        Cur_Hp = unitData_st.max_Hp;
        isDead = false;
        GetComponent<Collider2D>().enabled = true;
        SetAnim(AnimState.Idle);
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
            animator.SetTrigger(DoSkill1);
        }
    }

    //공주 스킬1: 방패 강타
    public void OnSkill1()
    {
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
            ApplyAttack(target_Unit, unitData_st.attackDamage * 3, AttackType.Physical);
            //디버프 처리
            target_Unit.AddComponent<ShieldSmiteDebuff>();
        }
        StartCoroutine(C_Skill1_CoolDown());
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
            animator.SetTrigger(DoSkill2);
        }
    }

    //공주 스킬2: 부러진 영웅검
    public void OnSkill2()
    {
        float skillRange = 1f;

        //스캔할 레이어 설정
        string target_Layer = TeamLayer;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + Vector3.left * skillRange, Vector2.right, skillRange * 2, LayerMask.GetMask(target_Layer));

        foreach (var item in hits)
        {
            if (item.collider.GetComponent<TeamBase_Unit>())
                continue;

            Unit target_Unit = item.collider.GetComponent<Unit>();
            target_Unit.AddComponent<BrokenHeroSword>();
            //버프 처리
        }
        StartCoroutine(C_Skill2_CoolDown());
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
}
