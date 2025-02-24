using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    //���� ���� �ٲ� �� �ִ� �ɷ�ġ���� ���� ����ü
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

    //�Ʊ� �������� ���� �������� �Ǻ��ϴ� ����
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

    #region �̵� ����
    //�̵� ������ �Ǻ��ϴ� ����
    protected bool isMoving = false;
    //�̵� �ӵ�(���� ����)
    protected Vector3 moveDir = Vector3.zero;
    //�̵� ������ ��輱
    protected float boundary_Min_x;
    protected float boundary_Max_x;
    #endregion
    #region ���� ����
    //�ν��� �Ÿ�(���� ���� - 0.1(20px))
    float rayCast_dis = 0.8f - 0.1f;

    //������ �� �ִ��� �Ǻ��ϴ� ����
    bool canAttack = true;
    //���������� �Ǻ��ϴ� ����
    bool isAttacking = false;
    #endregion
    #region �ִϸ��̼� ����

    enum AnimState {idle,move,attack,hit,die }
    //���� �ִϸ��̼��� ����
    AnimState cur_State;
    #endregion
    #region ������Ʈ
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

    #region �ʱ�ȭ
    public virtual void Init()
    {
        unitData_st.moveSpeed = ud.move_Speed;
        unitData_st.damage = ud.damage;
        unitData_st.attackSpeed = ud.attack_Speed;
        unitData_st.accuracy = ud.accuracy;
        unitData_st.avoidance = ud.avoidance;
        unitData_st.armor = ud.armor;
    }
    
    //�� ����
    public void SetTeam()
    {
        //�±� ����
        gameObject.tag = IsTeam ? TeamTag : EnemyTag;
        //���̾� ����
        gameObject.layer = LayerMask.NameToLayer(IsTeam ? TeamLayer : EnemyLayer);
    }
    #endregion

    #region �̵� �Լ�
    //������ �̵��ϴ� �Լ�
    protected virtual void Move()
    {
        //�̵����� ������ �̵��� �� ��
        if (moveDir.x == 0)
        {
            isMoving = false;

            //�������� �ƴ� �� idle �ִϸ��̼� ���
            if (!isAttacking)
            {
                SetAnim(AnimState.idle);
            }
            return;
        }
        isMoving = true;

        //�̵��� ������ �ٶ󺸱�
        SetDir();

        //�ӽ� �̵�
        Vector3 tmp_vec = transform.position + moveDir * Time.deltaTime * unitData_st.moveSpeed;
        //��輱�� ���� �ʵ��� ����
        SetBoundary();
        float clamped_x = Mathf.Clamp(tmp_vec.x, boundary_Min_x, boundary_Max_x);
        tmp_vec.x = clamped_x;
        //���� �̵�
        transform.position = tmp_vec;

        //�̵� �ִϸ��̼�
        SetAnim(AnimState.move);
    }

    //�̵� ������ �ٶ󺸰� �ϴ� �Լ�
    protected void SetDir()
    {
        //������ �ٶ󺸸� ��������Ʈ �¿����
        sr.flipX = moveDir.x < 0;
    }

    //��輱 ���� �Լ�
    void SetBoundary()
    {
        //�������� �ʾ��� ��쿡�� ����
        if (boundary_Max_x == 0 && boundary_Min_x == 0)
        {
            boundary_Max_x = DunGeonManager_New.instance.boundary_Max_x;
            boundary_Min_x = DunGeonManager_New.instance.boundary_Min_x;
        }
    }
    #endregion

    #region ����
    //���� ���� ���� ���� �ִ��� ��ĵ
    protected void ScanEnemy()
    {
        //���� ����
        Vector2 rayDir = IsTeam ? Vector2.right : Vector2.left;
        //��ĵ�� ���̾� ����
        string target_Layer = IsTeam ? EnemyLayer : TeamLayer; 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, rayCast_dis, LayerMask.GetMask(target_Layer));
        //rayCast ����ȭ(�����)
        Debug.DrawRay(transform.position, rayDir * rayCast_dis, IsTeam ? Color.blue : Color.red, Time.deltaTime);
        if (hit.collider != null)
        {
            Attack();
        }
    }

    //����
    protected void Attack()
    {
        //�̵��� ����
        moveDir = Vector3.zero;

        //������ �� ������ ���� ����
        if (canAttack)
        {
            isAttacking = true;
            StartCoroutine(C_AttackCoolDown());
            SetAnim(AnimState.attack);
        }
        
    }

    //���� �����̸� �����ϴ� �Լ�
    IEnumerator C_AttackCoolDown()
    {
        canAttack = false;
        yield return new WaitForSeconds(10f / unitData_st.attackSpeed);
        canAttack = true;
    }
    #endregion

    #region �ִϸ��̼� �Լ�
    //�ִϸ��̼��� ����ϴ� �Լ�
    void SetAnim(AnimState animState)
    {
        //�̹� �ش� �ִϸ��̼� ������̸� �������� ����
        if (cur_State == animState)
            return;

        //���� �ִϸ��̼� ���¸� ����
        cur_State = animState;
        //�ش� �ִϸ��̼� ���
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
