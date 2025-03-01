using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    //�Ʊ� ����ü���� ���� ����ü���� �Ǻ��ϴ� ����
    bool isTeam;
    public bool IsTeam
    {
        get { return isTeam; }
        set
        {
            isTeam = value;
            //���� ����
            moveDir.x = isTeam ? 1 : -1;
            sr.flipX = !isTeam;
            //��� �±� ����
            targetTag = isTeam ? Unit.EnemyTag : Unit.TeamTag;
        }
    }

    //������
    float damage;
    //���߷�
    float accuracy;
    //���� ���� ��
    int target_Count;
    //�ִ� �̵� �Ÿ�
    float max_range;
    //���� �̵� �Ÿ�
    float cur_range;
    //���� ����
    AttackType attackType;

    //�̵� �ӵ�
    float move_Speed = 2f;

    //�̵� ����
    Vector3 moveDir = Vector3.zero;
    //��� �±�
    string targetTag;

    //������ ��� List
    List<Collider2D> hitted_col = new List<Collider2D>();

    Collider2D col;
    SpriteRenderer sr;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        //����ü �̵�
        float moveDistance = move_Speed * Time.deltaTime;
        transform.position += moveDir * moveDistance;
        //���� �̵� �Ÿ� ���
        cur_range += moveDistance;

        //�ִ� ��Ÿ� ���� �� ����
        if (cur_range > max_range)
        {
            DestoryProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag) && target_Count > 0)
        {
            //�̹� ������ ����̸� �浹 ó���� ���� ����
            foreach (var item in hitted_col)
                if (item == collision)
                    return;

            //���� ��� ����
            Unit target_Unit = collision.GetComponent<Unit>();
            //������ ��� ����
            hitted_col.Add(collision);
            //���� ����
            if (TryAttack(target_Unit))
            {
                //���� ����
                ApplyAttack(target_Unit);
            }

            //�ִ� ��� ����ŭ ���� ������ �Ϸ������� ����ü ����
            if (--target_Count <= 0)
                DestoryProjectile();
        }
    }

    //���� ���� ������ ��ȯ
    bool TryAttack(Unit target_Unit)
    {
        //���� Ȯ��
        float pro = accuracy - target_Unit.unitData_st.avoidance + 50f;
        //�ּ� Ȯ�� 5%
        pro = pro > 5 ? pro : 5;
        return Random.Range(0, 100) < pro;
    }

    //���� ���� �� ���ظ� �ִ� �Լ�
    protected virtual void ApplyAttack(Unit target_Unit)
    {
        float type_res = attackType== target_Unit.ud.resistance_Type ? 0.5f : 1;
        float type_weak = attackType == target_Unit.ud.weak_Type ? 2f : 1;

        //���� ���ط�
        float total_Damage = (damage - target_Unit.unitData_st.armor) * (type_res * type_weak);
        target_Unit.TakeDamage(total_Damage);
    }


    //unit���� �����͸� �޾ƿ��� �Լ�
    public void SetData(Unit unit)
    {
        IsTeam = unit.IsTeam;
        target_Count = unit.ud.target_Count;
        //��Ÿ��� ������ ������ ��Ÿ� * 1.2
        max_range = unit.ud.attack_Range * 1.2f;
        damage = unit.ud.damage;
        accuracy = unit.ud.accuracy;
        attackType = unit.ud.attack_Type;
    }

    //����ü�� �����ϴ� �Լ�
    public void DestoryProjectile()
    {
        Destroy(gameObject);
    }
}
