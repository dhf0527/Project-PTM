using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    //전투 도중 바뀔 수 있는 능력치들을 담은 구조체
    public struct UnitData_Struct
    {
        public float moveSpeed;
        public float damage;
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
        }
    }

    [Header("scriptable object")]
    [SerializeField] UnitData ud;
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
    protected bool isMoving = false;
    //이동 속도(방향 포함)
    protected Vector3 moveDir = Vector3.zero;
    //이동 가능한 경계선
    protected float boundary_Min_x;
    protected float boundary_Max_x;
    #endregion
    #region 공격 변수
    //인식할 거리(공격 범위 - 0.1(20px))
    float rayCast_dis = 0.8f - 0.1f;

    //공격할 수 있는지 판별하는 변수
    bool canAttack = true;
    //공격중인지 판별하는 변수
    bool isAttacking = false;
    #endregion
    #region 애니메이션 변수

    enum AnimState {idle,move,attack,hit,die }
    //현재 애니메이션의 상태
    AnimState cur_State;
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
        Move();
        ScanEnemy();
    }

    #region 초기화
    public virtual void Init()
    {
        unitData_st.moveSpeed = ud.move_Speed;
        unitData_st.damage = ud.damage;
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
        if (moveDir.x == 0)
        {
            isMoving = false;

            //공격중이 아닐 때 idle 애니메이션 재생
            if (!isAttacking)
            {
                SetAnim(AnimState.idle);
            }
            return;
        }
        isMoving = true;

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
    void SetBoundary()
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
    //공격 범위 내에 적이 있는지 스캔
    protected void ScanEnemy()
    {
        //방향 설정
        Vector2 rayDir = IsTeam ? Vector2.right : Vector2.left;
        //스캔할 레이어 설정
        string target_Layer = IsTeam ? EnemyLayer : TeamLayer; 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, rayCast_dis, LayerMask.GetMask(target_Layer));
        //rayCast 가시화(디버깅)
        Debug.DrawRay(transform.position, rayDir * rayCast_dis, IsTeam ? Color.blue : Color.red, Time.deltaTime);
        if (hit.collider != null)
        {
            Attack();
        }
    }

    //공격
    protected void Attack()
    {
        //이동을 멈춤
        moveDir = Vector3.zero;

        //공격할 수 있으면 공격 실행
        if (canAttack)
        {
            isAttacking = true;
            StartCoroutine(C_AttackCoolDown());
            SetAnim(AnimState.attack);
        }
        
    }

    //공격 딜레이를 구현하는 함수
    IEnumerator C_AttackCoolDown()
    {
        canAttack = false;
        yield return new WaitForSeconds(10f / unitData_st.attackSpeed);
        canAttack = true;
    }
    #endregion

    #region 애니메이션 함수
    //애니메이션을 재생하는 함수
    void SetAnim(AnimState animState)
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
