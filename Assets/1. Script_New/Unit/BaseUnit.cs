using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    //전투 도중 바뀔 수 있는 능력치들을 담은 구조체
    public struct UnitData_Struct
    {
        public float moveSpeed;
        public float attackDamage;
        public float attackSpeed;
        public float accuracy;
        public float avoidance;
        public float armor;
    }
    public UnitData_Struct unitData_st;

    //아군 유닛인지 적군 유닛인지 판별하는 변수
    bool isTeam;
    public bool IsTeam
    {
        get {return isTeam; }
        set
        {
            isTeam = value;
            moveDir.x = isTeam ? 1 : -1;
            SetTeam();
            hp_bar?.SetHpBarSprite(isTeam);
        }
    }

    [Header("scriptable object")]
    public UnitData ud;
    public HpBar_new hp_bar;

    #region readOnly
    protected static readonly int DoMove = Animator.StringToHash("doMove");
    protected static readonly int DoAttack = Animator.StringToHash("doAttack");
    protected static readonly int DoHit = Animator.StringToHash("doHit");
    protected static readonly int DoDie = Animator.StringToHash("doDie");
    protected static readonly int DoStop = Animator.StringToHash("doStop");
    public static readonly string EnemyTag = "Enemy";
    public static readonly string EnemyLayer = "Enemy";
    public static readonly string TeamTag = "Team";
    public static readonly string TeamLayer = "Team";
    #endregion

    #region 이동 변수
    //이동 중인지 판별하는 변수
    protected bool isMoving = true;
    //이동 속도(방향 포함)
    protected Vector3 moveDir = Vector3.zero;
    //이동 가능한 경계선
    protected float boundary_Min_x;
    protected float boundary_Max_x;
    #endregion
    #region 공격 변수
    //공격할 수 있는지 판별하는 변수
    protected bool canAttack = true;
    //공격중인지 판별하는 변수
    protected bool isAttacking = false;

    //스캔한 적을 받아올 hit
    protected RaycastHit2D hit;
    #endregion
    #region 피격 변수
    //현재 체력
    float cur_Hp;
    public float Cur_Hp
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
            //공주일 경우 전용 체력바 갱신
            if (ud.unit_Code == 0)
                DunGeonManager_New.instance.princessHpPanel.SetHpText(this);
            //체력바 갱신
            hp_bar.SetHpBar();
        }
    }

    //체력으로 인한 넉백을 당할 수 있는 횟수
    protected int knockBack_Count = 3;
    protected bool canKnockBack = true;
    protected bool canKnockBack_By_Hp = true;
    protected bool isKnockBacking = false;
    protected bool isDead = false;
    #endregion
    #region 애니메이션 변수

    protected enum AnimState {idle,move,attack,hit,die }
    //현재 애니메이션의 상태
    protected AnimState cur_State;
    #endregion
    #region 컴포넌트
    SpriteRenderer sr;
    Animator animator;
    #endregion

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Init();
    }

    protected void Update()
    {
        if (isDead)
            return;

        if (isKnockBacking)
            return;
        ScanEnemy();
        Move();
    }

    #region 초기화
    public virtual void Init()
    {
        //유닛 사이즈별 공격 범위 설정
        ud.attack_Range = ud.size == 1 ? 0.8f : ud.size == 2 ? 1f : 1.2f;

        //체력바와 연동
        hp_bar.unit = this;
        //체력 설정
        Cur_Hp = ud.hp;

        unitData_st.moveSpeed = ud.move_Speed;
        unitData_st.attackDamage = ud.damage;
        unitData_st.attackSpeed = ud.attack_Speed;
        unitData_st.accuracy = ud.accuracy;
        unitData_st.avoidance = ud.avoidance;
        unitData_st.armor = ud.armor;
    }
    
    //팀 설정
    public void SetTeam()
    {
        //태그 설정
        gameObject.tag = IsTeam ? TeamTag : EnemyTag;
        //레이어 설정
        gameObject.layer = LayerMask.NameToLayer(IsTeam ? TeamLayer : EnemyLayer);
    }
    #endregion

    #region 이동 함수
    //실제로 이동하는 함수
    protected virtual void Move()
    {
        //이동량이 없으면 이동을 안 함
        if (!isMoving || moveDir.x == 0)
        {
            //스캔된 적이 있다면 공격
            if (hit.collider != null)
            {
                Attack();
            }

            //공격중이 아닐 때 idle 애니메이션 재생
            if (!isAttacking)
            {
                SetAnim(AnimState.idle);
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
        SetAnim(AnimState.move);
    }

    //이동 방향을 바라보게 하는 함수
    protected void SetDir()
    {
        //왼쪽을 바라보면 스프라이트 좌우반전
        sr.flipX = moveDir.x < 0;
    }

    //경계선 설정 함수
    protected void SetBoundary()
    {
        //설정되지 않았을 경우에만 설정
        if (boundary_Max_x == 0 && boundary_Min_x == 0)
        {
            boundary_Max_x = DunGeonManager_New.instance.boundary_Max_x;
            boundary_Min_x = DunGeonManager_New.instance.boundary_Min_x;
        }
    }
    #endregion

    #region 공격
    //스캔 범위 내에 적이 있는지 스캔
    protected void ScanEnemy()
    {
        //레이캐스트 위치
        Vector3 rayPos = transform.position;
        rayPos.y = 0;
        //방향 설정
        Vector2 rayDir = IsTeam ? Vector2.right : Vector2.left;
        //스캔할 레이어 설정
        string target_Layer = IsTeam ? EnemyLayer : TeamLayer; 
        //레이캐스트 발사
        hit = Physics2D.Raycast(rayPos, rayDir, ud.attack_Range, LayerMask.GetMask(target_Layer));
        //rayCast 가시화(디버깅)
        Debug.DrawRay(rayPos + (IsTeam ? Vector3.up * 0.5f : Vector3.zero), rayDir * ud.attack_Range, IsTeam ? Color.blue : Color.red, Time.deltaTime);

        //스캔된 적이 있고, 일정 거리 내에 있으면 멈춤
        if (hit.collider != null && (transform.position.x - hit.transform.position.x) < ud.attack_Range + 0.4f)
            isMoving = false;
        //스캔된 적이 없고 공격중이 아니면 다시 움직임
        else if (!isAttacking)
            isMoving = true;
    }

    //공격
    protected void Attack()
    {
        //공격할 수 있으면 공격 실행
        if (canAttack)
        {
            isAttacking = true;
            StartCoroutine(C_AttackCoolDown());
            SetAnim(AnimState.attack);
        }   
    }

    //attack애니메이션에서 호출할 함수
    public void OnAttack()
    {
        switch (ud.attack_RangeType)
        {
            case AttackRange.Melee:
                MeleeAttack();
                break;
            case AttackRange.Ranged:
                RangedAttack();
                break;
            default:
                Debug.LogError($"{name}:잘못된 RangeType");
                break;
        }
    }

    void MeleeAttack()
    {
        //방향 설정
        Vector2 rayDir = IsTeam ? Vector2.right : Vector2.left;
        //스캔할 레이어 설정
        string target_Layer = IsTeam ? EnemyLayer : TeamLayer;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayDir, ud.attack_Range, LayerMask.GetMask(target_Layer));

        //공격할 대상의 수
        int target_Count = hits.Length < ud.target_Count ? hits.Length : ud.target_Count;
        for (int i = 0; i < target_Count; i++)
        {
            BaseUnit target_Unit = hits[i].collider.GetComponent<BaseUnit>();
            if (TryAttack(target_Unit))
            {
                ApplyAttack(target_Unit);
            }
        }
    }

    void RangedAttack()
    {

    }

    //공격 전달 판정을 반환
    bool TryAttack(BaseUnit target_Unit)
    {
        //명중 확률
        float pro = unitData_st.accuracy - target_Unit.unitData_st.avoidance + 50f;
        //최소 확률 5%
        pro = pro > 5 ? pro : 5;
        return Random.Range(0, 100) < pro;
    }

    //공격 명중 시 피해를 주는 함수
    protected virtual void ApplyAttack(BaseUnit target_Unit)
    {
        float type_res = ud.attack_Type == target_Unit.ud.resistance_Type ? 0.5f : 1;
        float type_weak = ud.attack_Type == target_Unit.ud.weak_Type ? 2f : 1;

        //최종 피해량
        float damage = (unitData_st.attackDamage - target_Unit.unitData_st.armor) * (type_res * type_weak);
        target_Unit.TakeDamage(damage);
    }

    //공격 딜레이를 구현하는 함수
    IEnumerator C_AttackCoolDown()
    {
        canAttack = false;
        yield return new WaitForSeconds(10f / unitData_st.attackSpeed);
        canAttack = true;
    }

    //공격이 끝났을 때의 처리를 하는 함수(애니메이션 마지막-1프레임에 실행)
    public void OnEndAttack()
    {
        isMoving = true;
        isAttacking = false;
    }
    #endregion

    #region 피격
    public void TakeDamage(float damage)
    {
        Cur_Hp -= damage;
        //체력 감소로 인한 넉백
        if (canKnockBack && damage >= (ud.hp / 5) && canKnockBack_By_Hp && knockBack_Count > 0) 
        {
            StartCoroutine(KnockBack());
            knockBack_Count--;
            StartCoroutine(C_KnockBack_CoolDown());
        }
    }

    //유닛 사망 즉시 호출
    public virtual void Dead()
    {
        canKnockBack = false;
        SetAnim(AnimState.die);
        hp_bar.gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        isDead = true;
    }

    //유닛 사망 애니메이션 끝날 때 호출
    public virtual void OnDead()
    {
        Destroy(gameObject);
    }

    //넉백 함수
    IEnumerator KnockBack()
    {
        //공격 중이었을 경우 공격 종료 처리
        if (cur_State == AnimState.attack)
            OnEndAttack();

        //0.75초동안 넉백
        SetAnim(AnimState.hit);
        isKnockBacking = true;

        float knockTime = 0;
        float knockSpeed = 0;
        while (knockTime < 0.75f)
        {
            knockTime += Time.deltaTime;
            //가속도 보정
            knockSpeed = Mathf.Lerp((1.5f / 0.75f), 0, (knockTime / 0.75f));
            transform.position += (-moveDir.normalized) * knockSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        isKnockBacking = false;
    }

    //체력 감소로 인한 넉백의 쿨다운
    IEnumerator C_KnockBack_CoolDown()
    {
        canKnockBack_By_Hp = false;
        yield return new WaitForSeconds(2);
        canKnockBack_By_Hp = true;
    }
    #endregion

    #region 애니메이션 함수
    //애니메이션을 재생하는 함수
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
            case AnimState.idle:
                animator.SetTrigger(DoStop);
                break;
            case AnimState.move:
                animator.SetTrigger(DoMove);
                break;
            case AnimState.attack:
                animator.SetTrigger(DoAttack);
                break;
            case AnimState.hit:
                animator.SetTrigger(DoHit);
                break;
            case AnimState.die:
                animator.SetTrigger(DoDie);
                break;
            default:
                break;
        }
    }
    #endregion


}
