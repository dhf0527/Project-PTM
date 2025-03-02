using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    //���� ���� �ٲ� �� �ִ� �ɷ�ġ���� ���� ����ü
    public struct UnitData_Struct
    {
        public float max_Hp;
        public float moveSpeed;
        public float attackDamage;
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

    #region �̵� ����
    //�̵� ������ �Ǻ��ϴ� ����
    protected bool isMoving = true;
    //�̵� ����
    protected Vector3 moveDir = Vector3.zero;
    //�̵� ������ ��輱
    protected float boundary_Min_x;
    protected float boundary_Max_x;
    #endregion
    #region ���� ����
    [Header("ranged: ���Ÿ� ���ָ� �ʿ�� �ϴ� ����")]
    [SerializeField] Transform ranged_Projectile_Pos;
    [SerializeField] Projectile ranged_Projectile_Prefabs;


    //������ �� �ִ��� �Ǻ��ϴ� ����
    protected bool canAttack = true;
    //���������� �Ǻ��ϴ� ����
    protected bool isAttacking = false;

    //��ĵ�� ���� �޾ƿ� hit
    protected RaycastHit2D hit;
    #endregion
    #region �ǰ� ����
    //���� ü��
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
            //��� üũ
            if (cur_Hp <= 0)
            {
                cur_Hp = 0;
                Dead();
            }
            //������ ��� ���� ü�¹� ����
            if (ud.unit_Code == 0)
                DunGeonManager_New.instance.princessHpPanel.SetHpBar(this);

            //ü�¹� ����
            hpBar.SetHpBar();
        }
    }

    protected HpBar_new hpBar;

    //ü������ ���� �˹��� ���� �� �ִ� Ƚ��
    protected int knockBack_Count = 3;
    protected bool canKnockBack = true;
    protected bool canKnockBack_By_Hp = true;
    protected bool isKnockBacking = false;
    protected bool isDead = false;
    #endregion
    #region �ִϸ��̼� ����

    protected enum AnimState {idle,move,attack,hit,die }
    //���� �ִϸ��̼��� ����
    protected AnimState cur_State;
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
        if (isDead)
            return;

        if (isKnockBacking)
            return;
        ScanEnemy();
        Move();
    }

    #region �ʱ�ȭ
    public virtual void Init()
    {
        //���� ���� ����/����� ���� ���� ����
        if (ud.attack_RangeType == AttackRange.Melee)
            ud.attack_Range = ud.size == Unit_Size.Small ? 0.8f : ud.size == Unit_Size.Medium ? 1f : 1.2f;
        else
            ud.attack_Range = ud.size == Unit_Size.Small ? 2f : ud.size == Unit_Size.Medium ? 2.5f : 3f;

        unitData_st.max_Hp = ud.hp;
        unitData_st.moveSpeed = ud.move_Speed;
        unitData_st.attackDamage = ud.damage;
        unitData_st.attackSpeed = ud.attack_Speed;
        unitData_st.accuracy = ud.accuracy;
        unitData_st.avoidance = ud.avoidance;
        unitData_st.armor = ud.armor;
        SetHpBar();
    }

    //ü�¹� ���� �� ����
    public virtual void SetHpBar()
    {
        //ü�¹ٸ� world canvas�� ����
        hpBar = Instantiate(WorldCanavsManager.instance.hpBar_Prf, WorldCanavsManager.instance.worldCanvas_Trans);
        //ü�¹� ����
        hpBar.unit = this;
        //ü�¹� ��ġ ����
        hpBar.SetHpPos(ud.size == Unit_Size.Small ? 1.2f : ud.size == Unit_Size.Medium ? 1.2f : 1.5f);

        //ü�� ����
        Cur_Hp = unitData_st.max_Hp;
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
        if (!isMoving || moveDir.x == 0)
        {
            //��ĵ�� ���� �ִٸ� ����
            if (hit.collider != null)
            {
                Attack();
            }

            //�������� �ƴ� �� idle �ִϸ��̼� ���
            if (!isAttacking)
            {
                SetAnim(AnimState.idle);
            }
            return;
        }

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
    protected void SetBoundary()
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
    //��ĵ ���� ���� ���� �ִ��� ��ĵ
    protected void ScanEnemy()
    {
        //����ĳ��Ʈ ��ġ
        Vector3 rayPos = transform.position;
        rayPos.y = 0;
        //���� ����
        Vector2 rayDir = IsTeam ? Vector2.right : Vector2.left;
        //��ĵ�� ���̾� ����
        string target_Layer = IsTeam ? EnemyLayer : TeamLayer; 
        //����ĳ��Ʈ �߻�
        hit = Physics2D.Raycast(rayPos, rayDir, ud.attack_Range, LayerMask.GetMask(target_Layer));
        //rayCast ����ȭ(�����)
        Debug.DrawRay(rayPos + (IsTeam ? Vector3.up * 0.5f : Vector3.zero), rayDir * ud.attack_Range, IsTeam ? Color.blue : Color.red, Time.deltaTime);

        //��ĵ�� ���� �ְ�, ���� �Ÿ� ���� ������ ����
        if (hit.collider != null && (transform.position.x - hit.transform.position.x) < ud.attack_Range + 0.4f)
            isMoving = false;
        //��ĵ�� ���� ���� �������� �ƴϸ� �ٽ� ������
        else if (!isAttacking)
            isMoving = true;
    }

    //����
    protected void Attack()
    {
        //������ �� ������ ���� ����
        if (canAttack)
        {
            isAttacking = true;
            StartCoroutine(C_AttackCoolDown());
            SetAnim(AnimState.attack);
        }   
    }

    //attack�ִϸ��̼ǿ��� ȣ���� �Լ�
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
                Debug.LogError($"{name}:�߸��� RangeType");
                break;
        }
    }

    void MeleeAttack()
    {
        //���� ����
        Vector2 rayDir = IsTeam ? Vector2.right : Vector2.left;
        //��ĵ�� ���̾� ����
        string target_Layer = IsTeam ? EnemyLayer : TeamLayer;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayDir, ud.attack_Range, LayerMask.GetMask(target_Layer));

        //������ ����� ��
        int target_Count = hits.Length < ud.target_Count ? hits.Length : ud.target_Count;
        for (int i = 0; i < target_Count; i++)
        {
            Unit target_Unit = hits[i].collider.GetComponent<Unit>();
            if (TryAttack(target_Unit))
            {
                ApplyAttack(target_Unit);
            }
        }
    }

    void RangedAttack()
    {
        //�ٶ󺸴� ���⿡ ���� ����ü ���� ��ġ ����
        Vector3 spawn_Pos = transform.position + (sr.flipX ? -ranged_Projectile_Pos.localPosition : ranged_Projectile_Pos.localPosition);
        spawn_Pos.y = ranged_Projectile_Pos.position.y;
        //����ü ����
        Projectile projectile = Instantiate(ranged_Projectile_Prefabs);
        //����ü �θ� �� ��ġ ����
        projectile.transform.SetParent(DunGeonManager_New.instance.projectile_Parent);
        projectile.transform.position = spawn_Pos;

        //����ü ������ ����
        projectile.SetData(this);
    }

    //���� ���� ������ ��ȯ
    bool TryAttack(Unit target_Unit)
    {
        //���� Ȯ��
        float pro = unitData_st.accuracy - target_Unit.unitData_st.avoidance + 50f;
        //�ּ� Ȯ�� 5%
        pro = pro > 5 ? pro : 5;
        return Random.Range(0, 100) < pro;
    }

    //���� ���� �� ���ظ� �ִ� �Լ�
    protected virtual void ApplyAttack(Unit target_Unit)
    {
        float type_res = ud.attack_Type == target_Unit.ud.resistance_Type ? 0.5f : 1;
        float type_weak = ud.attack_Type == target_Unit.ud.weak_Type ? 2f : 1;

        //���� ���ط�
        float damage = (unitData_st.attackDamage - target_Unit.unitData_st.armor) * (type_res * type_weak);
        //�ּ� ���ط� 1
        damage = damage < 1 ? 1 : damage;
        target_Unit.TakeDamage(damage);
    }

    //���� �����̸� �����ϴ� �Լ�
    IEnumerator C_AttackCoolDown()
    {
        canAttack = false;
        yield return new WaitForSeconds(10f / unitData_st.attackSpeed);
        canAttack = true;
    }

    //������ ������ ���� ó���� �ϴ� �Լ�(�ִϸ��̼� ������-1�����ӿ� ����)
    public void OnEndAttack()
    {
        isMoving = true;
        isAttacking = false;
    }
    #endregion

    #region �ǰ�
    public void TakeDamage(float damage)
    {
        Cur_Hp -= damage;
        //ü�� ���ҷ� ���� �˹�
        if (canKnockBack && damage >= (unitData_st.max_Hp / 5) && canKnockBack_By_Hp && knockBack_Count > 0) 
        {
            StartCoroutine(KnockBack());
            knockBack_Count--;
            StartCoroutine(C_KnockBack_CoolDown());
        }
    }

    //���� ��� ��� ȣ��
    public virtual void Dead()
    {
        canKnockBack = false;
        SetAnim(AnimState.die);
        hpBar.gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        isDead = true;
    }

    //���� ��� �ִϸ��̼� ���� �� ȣ��
    public virtual void OnDead()
    {
        Destroy(gameObject);
    }

    //�˹� �Լ�
    IEnumerator KnockBack()
    {
        //���� ���̾��� ��� ���� ���� ó��
        if (cur_State == AnimState.attack)
            OnEndAttack();

        //0.75�ʵ��� �˹�
        SetAnim(AnimState.hit);
        isKnockBacking = true;

        float knockTime = 0;
        float knockSpeed = 0;
        while (knockTime < 0.75f)
        {
            knockTime += Time.deltaTime;
            //���ӵ� ����
            knockSpeed = Mathf.Lerp((1.5f / 0.75f), 0, (knockTime / 0.75f));
            //��輱 ����
            SetBoundary();
            Vector3 tmp_Vec = transform.position + (-moveDir.normalized) * knockSpeed * Time.deltaTime;
            tmp_Vec.x = Mathf.Clamp(tmp_Vec.x, boundary_Min_x, boundary_Max_x);
            //�̵�
            transform.position = tmp_Vec;
            yield return new WaitForEndOfFrame();
        }

        isKnockBacking = false;
    }

    //ü�� ���ҷ� ���� �˹��� ��ٿ�
    IEnumerator C_KnockBack_CoolDown()
    {
        canKnockBack_By_Hp = false;
        yield return new WaitForSeconds(2);
        canKnockBack_By_Hp = true;
    }
    #endregion

    #region �ִϸ��̼� �Լ�
    //�ִϸ��̼��� ����ϴ� �Լ�
    protected void SetAnim(AnimState animState)
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
