using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Unit : MonoBehaviour
{
    public float AttackDamage { get { return Mathf.Max((ud.damage * (1 + unitStatData_st.attack_PlusPercent * 0.01f)) + unitStatData_st.attack_Plus, 0); }}
    public float AttackSpeed { get { return Mathf.Max(ud.attack_Speed * (1 + unitStatData_st.attackSpeed_PlusPercent * 0.01f) + unitStatData_st.attackSpeed_Plus, 0.001f); } }
    public int Accuracy { get { return Mathf.Max(ud.accuracy + unitStatData_st.accuracy_Plus, 0);} }
    public int TargetCount { get { return Mathf.Max(ud.target_Count + unitStatData_st.targetCount_Plus, 1); } }
    public float Max_Hp { get { return Mathf.Max(ud.hp + unitStatData_st.max_Hp_Plus, 1); }}
    public int Armor { get { return Mathf.Max((int)(ud.armor * (1  + unitStatData_st.armor_PlusPercent * 0.01f)) + unitStatData_st.armor_Plus, 0); } }
    public int Avoidance { get { return Mathf.Max(ud.avoidance + unitStatData_st.avoidance_Plus, 0); } }
    public float MoveSpeed { get { return Mathf.Max(ud.move_Speed * (1 + unitStatData_st.moveSpeed_PlusPercent * 0.01f) + unitStatData_st.moveSpeed_Plus, ud.move_Speed * 0.1f); } }
    public int Cost { get { return Mathf.Max((int)(ud.cost * (1 - unitStatData_st.cost_MinusPercent * 0.01f)), 0);  } }
    public int SpawnCount { get { return Mathf.Max(ud.spawn_Count + unitStatData_st.spawnCount_Plus, 1); } }
    public float SpawnCoolDown { get { return DunGeonManager_New.instance.spawnCoolTimesByLevel[ud.level - 1] * (1 - unitStatData_st.spawnCoolDown_MinusPercent * 0.01f); } }
    public float AttackRange { get { return ud.attack_Range == 0 ? GetAttackRange() : ud.attack_Range; } }

    [HideInInspector] public Unit_Size size;

    //스탯의 증감치를 담는 구조체
    public struct UnitStatData_Struct
    {
        //최종 데미지 증감(비율)
        public float totalDamage_PlusPercent;
        //공격력 증감(절대값)
        public float attack_Plus;
        //공격력 증감(비율)
        public float attack_PlusPercent;
        //공격속도 증감(절대값)
        public float attackSpeed_Plus;
        //공격속도 증감(비율)
        public float attackSpeed_PlusPercent;
        //주는 피해량 증감
        public float attackBoost_PlusPercent;
        //명중률 증감
        public int accuracy_Plus;
        //타겟 수 증감
        public int targetCount_Plus;

        //최종 피해량 증감
        public float totalDamageReduction_PlusPercent;
        //최대체력 증감
        public float max_Hp_Plus;
        //방어력 증감(절대값)
        public int armor_Plus;
        //방어력 증감(비율)
        public float armor_PlusPercent;
        //회피율 증감
        public int avoidance_Plus;
        //받는 피해량 증감
        public float damageReduction_PlusPercent;

        //이동속도 증감(절대값)
        public float moveSpeed_Plus;
        //이동속도 증감(비율)
        public float moveSpeed_PlusPercent;

        //비용 증감(비율)
        public float cost_MinusPercent;
        //스폰 수 증감
        public int spawnCount_Plus;
        //고용 쿨타임 증감
        public float spawnCoolDown_MinusPercent;
    }
    public UnitStatData_Struct unitStatData_st;

    //아군 유닛인지 적군 유닛인지 판별하는 변수
    bool isTeam;
    public bool IsTeam
    {
        get {return isTeam; }
        set
        {
            isTeam = value;
            moveDir.x = isTeam ? 1 : -1;
            SetDir();
            SetTeam();
            hpBar?.SetHpBarSprite(isTeam);
        }
    }

    [Header("scriptable object")]
    public UnitData ud;
    public bool isHpText;
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
    //이동 방향
    protected Vector3 moveDir = Vector3.zero;
    //이동 가능한 경계선
    protected float boundary_Min_x;
    protected float boundary_Max_x;
    #endregion
    #region 공격 변수
    [SerializeField] Transform hitParent;
    [Header("ranged: 원거리 유닛만 필요로 하는 변수")]
    [SerializeField] Transform ranged_Projectile_Pos;
    [SerializeField] Projectile ranged_Projectile_Prefabs;


    //공격할 수 있는지 판별하는 변수
    protected bool canAttack = true;
    //공격중인지 판별하는 변수
    protected bool isAttacking = false;

    //스캔한 적을 받아올 hit
    protected RaycastHit2D hit;
    //관통 공격 여부
    [HideInInspector] public bool isPenetration;
    //고정 공격 여부
    [HideInInspector] public bool isTrueDamage;
    #endregion
    #region 피격 변수
    //현재 체력
    protected float cur_Hp;
    public virtual float Cur_Hp
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
                if(!isDead)
                    Dead();
            }
            //공주일 경우 전용 체력바 갱신
            if (ud.unit_Code == 0)
                DunGeonManager_New.instance.princessHpPanel.SetHpBar(this);

            //체력바 갱신
            hpBar?.SetHpBar();

            DisplayHpBar();
        }
    }
    [HideInInspector]public HpBar_Base hpBar;
    protected bool alwaysDisplayHpbar;
    Coroutine cor_HpBarInActive;

    //체력으로 인한 넉백을 당할 수 있는 횟수
    protected int knockBack_Count = 2;
    protected float knockBack_Hp = 0;
    [HideInInspector] public bool canKnockBack = true;
    [HideInInspector] public bool isImmune = false;
    protected bool canKnockBack_By_Hp = true;
    protected bool isKnockBacking = false;
    protected bool isDead = false;
    Coroutine cor_knockBack;

    protected Vector2 originColliderSize;
    protected Vector2 originColliderOffset;
    #endregion
    #region 애니메이션 변수
    [HideInInspector] public float origin_Scale = 1;
    [HideInInspector] public Vector3 scaleVector = Vector3.one;

    protected enum AnimState {Idle,Move,Attack,Hit,Die,Skill1,Skill2 }
    //현재 애니메이션의 상태
    protected AnimState cur_State;
    #endregion
    #region 컴포넌트
    SpriteRenderer sr;
    protected Animator animator;
    BoxCollider2D boxCollider2D;
    #endregion
    [HideInInspector] public int killGold;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Init();
    }

    protected void Update()
    {
        if (isDead || isKnockBacking)
            return;

        ScanEnemy();
        Move();
        animator.SetFloat("AttackSpeed", Mathf.Max(AttackSpeed / ud.attack_Speed, 1));
    }

    private void LateUpdate()
    {
        if (animator)
        {
            transform.localScale = origin_Scale * scaleVector;
            boxCollider2D.size = new Vector2(originColliderSize.x / scaleVector.x, originColliderSize.y / scaleVector.y);
            boxCollider2D.offset = new Vector2(originColliderOffset.x / scaleVector.x, originColliderOffset.y / scaleVector.y);
        }
    }

    #region 초기화
    public virtual void Init()
    {
        if (DunGeonManager_New.instance.isTutorial_1 || DunGeonManager_New.instance.isTutorial_2)
        {
            unitStatData_st.accuracy_Plus += 9999;
            unitStatData_st.avoidance_Plus -= 9999;
        }
        /*
        //유닛 공격 유형/사이즈별 공격 범위 설정
        if (ud.attack_RangeType == AttackRangeType.Melee)
            ud.attack_Range = ud.size == Unit_Size.Small ? 0.8f : ud.size == Unit_Size.Medium ? 1f : 1.2f;
        else
            ud.attack_Range = ud.size == Unit_Size.Small ? 2f : ud.size == Unit_Size.Medium ? 2.5f : 3f;
        */

        size = ud.size;

        SetHpBar();

        origin_Scale = 1;
        scaleVector = Vector3.one;

        boxCollider2D = GetComponent<BoxCollider2D>();
        originColliderSize = boxCollider2D.size;
        originColliderOffset = boxCollider2D.offset;

        knockBack_Hp = Max_Hp * 0.65f;
    }

    //체력바 생성 및 설정
    public virtual void SetHpBar()
    {
        //체력바를 world canvas에 생성
        hpBar = Instantiate(WorldCanavsManager.instance.hpBar_Prf, WorldCanavsManager.instance.worldCanvas_Trans);
        //체력바 연동
        hpBar.unit = this;
        //체력바 위치 설정
        hpBar.SetHpPos();
        //체력바 생성 시 비활성화
        hpBar.gameObject.SetActive(false);

        //체력 설정
        Cur_Hp = Max_Hp;
    }

    //팀 설정
    public void SetTeam()
    {
        //태그 설정
        gameObject.tag = IsTeam ? TeamTag : EnemyTag;
        //레이어 설정
        gameObject.layer = LayerMask.NameToLayer(IsTeam ? TeamLayer : EnemyLayer);
    }

    public float GetAttackRange()
    {
        float returnValue;
        if (ud.attack_RangeType == AttackRangeType.Melee)
            returnValue = DunGeonManager_New.instance.attackRanges_Melee_BySize[(int)size] / 200f;
        else
            returnValue = DunGeonManager_New.instance.attackRanges_Ranged_ByLevel[ud.level - 1] / 200f;

        if (returnValue == 0)
            Debug.LogError("사거리 이상");

        return returnValue;
    }

    public void SetBoss()
    {
        isHpText = true;
        alwaysDisplayHpbar = true;
        StopAllCoroutines();
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
                SetAnim(AnimState.Idle);
            }
            return;
        }

        //이동할 방향을 바라보기
        SetDir();

        //임시 이동
        Vector3 tmp_vec = transform.position + moveDir * Time.deltaTime * MoveSpeed / 200f;
        //경계선을 넘지 않도록 보정
        SetBoundary();
        float clamped_x = Mathf.Clamp(tmp_vec.x, boundary_Min_x, boundary_Max_x);
        tmp_vec.x = clamped_x;
        //실제 이동
        transform.position = tmp_vec;

        //이동 애니메이션
        SetAnim(AnimState.Move);
    }

    //이동 방향을 바라보게 하는 함수
    protected void SetDir()
    {
        //왼쪽을 바라보면 y축 180도 회전
        transform.localRotation = moveDir.x < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
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
        hit = Physics2D.Raycast(rayPos, rayDir, AttackRange, LayerMask.GetMask(target_Layer));
        //rayCast 가시화(디버깅)
        Debug.DrawRay(rayPos + (IsTeam ? Vector3.up * 0.5f : Vector3.zero), rayDir * AttackRange, IsTeam ? Color.blue : Color.red, Time.deltaTime);

        //스캔된 적이 있고, 일정 거리 내에 있으면 멈춤
        if (hit.collider != null && (transform.position.x - hit.transform.position.x) < AttackRange + 0.3f)
            isMoving = false;
        //스캔된 적이 없고 공격중이 아니면 다시 움직임
        else if (!isAttacking)
            isMoving = true;
    }

    //공격
    protected virtual void Attack()
    {
        //공격할 수 있으면 공격 실행
        if (canAttack)
        {
            isAttacking = true;
            StartCoroutine(C_AttackCoolDown());
            SetAnim(AnimState.Attack);
        }   
    }

    //attack애니메이션에서 호출할 함수
    public void OnAttack()
    {
        switch (ud.attack_RangeType)
        {
            case AttackRangeType.Melee:
                MeleeAttack();
                break;
            case AttackRangeType.Ranged:
                RangedAttack();
                break;
            default:
                Debug.LogError($"{name}:잘못된 RangeType");
                break;
        }
    }

    virtual protected void MeleeAttack()
    {
        //방향 설정
        Vector2 rayDir = IsTeam ? Vector2.right : Vector2.left;
        //스캔할 레이어 설정
        string target_Layer = IsTeam ? EnemyLayer : TeamLayer;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayDir, AttackRange, LayerMask.GetMask(target_Layer));

        //공격할 대상의 수
        int target_Count = hits.Length < ud.target_Count ? hits.Length : ud.target_Count;
        for (int i = 0; i < target_Count; i++)
        {
            Unit target_Unit = hits[i].collider.GetComponent<Unit>();
            if (TryAttack(target_Unit))
            {
                //추가데미지 계산
                float dmg = AttackDamage * (1 + (unitStatData_st.attackBoost_PlusPercent + CalculateAttackBoost(target_Unit)) * 0.01f);

                ApplyAttack(target_Unit, dmg, ud.attack_Type);
                switch (ud.attack_Type)
                {
                    case AttackType.None:
                        break;
                    case AttackType.Physical:
                        AudioManager.Instance.PlayerSfx(SFX_Enum.Hit_Physic);
                        break;
                    case AttackType.Magical:
                        AudioManager.Instance.PlayerSfx(SFX_Enum.Hit_Magic);
                        break;
                    case AttackType.Fire:
                        AudioManager.Instance.PlayerSfx(SFX_Enum.Hit_Fire);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                AudioManager.Instance.PlayerSfx(SFX_Enum.Avoid);
            }
        }
    }

    virtual protected void RangedAttack()
    {
        //바라보는 방향에 따라 투사체 생성 위치 조정
        Vector3 spawn_Pos = ranged_Projectile_Pos.position;
        //투사체 생성
        Projectile projectile = Instantiate(ranged_Projectile_Prefabs);
        projectile.unit = this;
        //투사체 부모 및 위치 설정
        projectile.transform.SetParent(DunGeonManager_New.instance.projectile_Parent);
        projectile.transform.position = spawn_Pos;
        //투사체 크기 설정
        projectile.transform.localScale = origin_Scale * Vector3.one;

        //투사체 데이터 전달
        projectile.SetData(this);
    }

    //공격 전달 판정을 반환
    protected bool TryAttack(Unit target_Unit)
    {
        //명중 확률
        float pro;

        string fly = "비행";
        if (target_Unit.ud.passive1 == fly || target_Unit.ud.passive2 == fly && ud.attack_RangeType == AttackRangeType.Melee && ud.passive1 != fly && ud.passive2 != fly)
            pro = Accuracy - (target_Unit.Avoidance * 2) + 50;
        else
            pro = Accuracy - target_Unit.Avoidance + 50f;

        //최소 확률 5%
        pro = pro > 5 ? pro : 5;
        return UnityEngine.Random.Range(0, 100) < pro;
    }

    //공격 명중 시 피해를 주는 함수
    public virtual void ApplyAttack(Unit target_Unit, float damage, AttackType attackType)
    {
        if (target_Unit.Cur_Hp <= 0)
            return;

        float type_res = attackType == target_Unit.ud.resistance_Type ? 0.5f : 1;
        float type_weak = attackType == target_Unit.ud.weak_Type ? 2f : 1;

        //최종 피해량
        float totalDamage;

        //고정공격일 경우
        if (isTrueDamage)
            totalDamage = damage;
        //관통공격일 경우
        else if (isPenetration)
            totalDamage = damage * type_weak;
        else
            totalDamage = (damage - target_Unit.Armor)
                * (type_res * type_weak) * (1 - target_Unit.unitStatData_st.damageReduction_PlusPercent * 0.01f);

        totalDamage *= 1 + unitStatData_st.totalDamage_PlusPercent * 0.01f;
        totalDamage *= 1 - unitStatData_st.totalDamageReduction_PlusPercent * 0.01f;

        //최소 피해량 1
        totalDamage = totalDamage < 1 ? 1 : totalDamage;
        target_Unit.TakeDamage(totalDamage);

        //유닛 처치 골드
        if(target_Unit.Cur_Hp <= 0 && !target_Unit.IsTeam)
        {
            int getGold = target_Unit.killGold;

            if (GameManager.Instance.current_Meal?.code == 1)
                getGold *= (int)GameManager.Instance.current_Meal.mealValue;

            DunGeonManager_New.instance.GetGold(getGold);
        }

        //피격 이펙트 생성
        if(hitParent)
            FxManager.Instance.Hit(hitParent.position);

        //데미지 텍스트 생성
        if(!target_Unit.isImmune)
            FxManager.Instance.DamageText(target_Unit.transform.position + Vector3.up * 1.2f, totalDamage, attackType);
    }

    //데미지 증가량 반환 함수
    virtual public float CalculateAttackBoost(Unit target_Unit)
    {
        float dmgBoost = 0;

        //유닛 업그레이드 효과 보스 추가 데미지
        if (target_Unit.GetComponent<BossGuard>())
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 9);
            if (upgradeLv != 0)
                dmgBoost += DunGeonManager_New.instance.unitUpgradeDatas[9].upgradeValue[upgradeLv - 1];
        }

        return dmgBoost;
    }

    //공격 딜레이를 구현하는 함수
    IEnumerator C_AttackCoolDown()
    {
        canAttack = false;
        float curTime = 0;
        while (curTime < (10f / AttackSpeed))
        {
            curTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
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
    public virtual void TakeDamage(float damage, bool isCanKnockBackDamage = true)
    {
        if (isImmune)
            return;

        Cur_Hp -= damage;
        //체력 감소로 인한 넉백
        if (isCanKnockBackDamage && canKnockBack && Cur_Hp <= knockBack_Hp && canKnockBack_By_Hp && knockBack_Count > 0) 
        {
            OnStartKnockBack();
            knockBack_Count--;
            knockBack_Hp = Cur_Hp;
            StartCoroutine(C_KnockBack_CoolDown());
        }
    }

    //유닛 사망 즉시 호출
    public virtual void Dead()
    {
        canKnockBack = false;
        SetAnim(AnimState.Die);
        Destroy(hpBar.gameObject);
        GetComponent<Collider2D>().enabled = false;

        //버프 제거
        Buff[] buffs = GetComponents<Buff>();

        foreach (var buff in buffs)
            Destroy(buff);

        DunGeonManager_New.instance.onStageUnits_Test.Remove(this);

        isDead = true;
    }

    //유닛 사망 애니메이션 끝날 때 호출
    public virtual void OnDead()
    {
        Destroy(gameObject);
    }

    //특수 넉백
    public void OnStartKnockBack()
    {
        if (!canKnockBack)
            return;

        if (cor_knockBack != null)
            StopCoroutine(cor_knockBack);

        cor_knockBack = StartCoroutine(KnockBack());
    }

    //넉백 함수
    public virtual IEnumerator KnockBack()
    {
        //공격 중이었을 경우 공격 종료 처리
        if (cur_State == AnimState.Attack)
            OnEndAttack();

        //0.75초동안 넉백
        SetAnim(AnimState.Hit);
        isKnockBacking = true;

        float knockTime = 0;
        float knockSpeed;
        while (knockTime < 0.75f)
        {
            knockTime += Time.deltaTime;
            //가속도 보정
            knockSpeed = Mathf.Lerp((2.5f / 0.75f), 0, (knockTime / 0.75f));
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

    //체력 감소로 인한 넉백의 쿨다운
    IEnumerator C_KnockBack_CoolDown()
    {
        canKnockBack_By_Hp = false;
        yield return new WaitForSeconds(4);
        canKnockBack_By_Hp = true;
    }

    public void DisplayHpBar()
    {
        //체력바 표시
        if (!alwaysDisplayHpbar)
        {
            if (cur_Hp != Max_Hp)
            {
                StopCoroutine(cor_HpBarInActive);
                hpBar?.gameObject.SetActive(true);
            }
            else
                cor_HpBarInActive = StartCoroutine(C_HpBarInActive());
        }
    }

    public void DisplayHpBar_Buff()
    {
        //체력바 표시
        if (!alwaysDisplayHpbar)
        {
            hpBar?.gameObject.SetActive(true);

            if (cur_Hp != Max_Hp)
                StopCoroutine(cor_HpBarInActive);
            else
            {
                StopCoroutine(cor_HpBarInActive);
                cor_HpBarInActive = StartCoroutine(C_HpBarInActive());
            }
        }
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
            default:
                break;
        }
    }
    #endregion

    #region 버프
    public void SetBuffIcon(int index, bool isActive)
    {
        hpBar?.SetBuffIcon(index, isActive);
    }

    #endregion

    //체력 회복
    public void GetHp(float amount)
    {
        //식사 효과 (칠면조 바비큐)
        if (GameManager.Instance.current_Meal?.code == 3)
            amount *= (1 + GameManager.Instance.current_Meal.mealValue2 * 0.01f);

        if (Cur_Hp + amount > Max_Hp)
            Cur_Hp = Max_Hp;
        else
            Cur_Hp += amount;
    }

    IEnumerator C_HpBarInActive()
    {
        yield return new WaitForSeconds(3f);
        hpBar?.gameObject.SetActive(false);
    }
}
