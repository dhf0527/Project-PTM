using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    //아군 투사체인지 적군 투사체인지 판별하는 변수
    bool isTeam;
    public bool IsTeam
    {
        get { return isTeam; }
        set
        {
            isTeam = value;
            //방향 설정
            moveDir.x = isTeam ? 1 : -1;
            sr.flipX = !isTeam;
            //대상 태그 설정
            targetTag = isTeam ? Unit.EnemyTag : Unit.TeamTag;
        }
    }

    //데미지
    protected float damage;
    //명중률
    protected float accuracy;
    //공격 가능 수
    protected int target_Count;
    //최대 이동 거리
    protected float max_range;
    //현재 이동 거리
    protected float cur_range;
    //공격 유형
    protected AttackType attackType;
    //관통 공격 여부
    protected bool isPenetration;

    //이동 속도
    protected float move_Speed = 8f;

    //이동 방향
    protected Vector3 moveDir = Vector3.zero;
    //대상 태그
    protected string targetTag;

    //적중한 대상 List
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
        //투사체 이동
        float moveDistance = move_Speed * Time.deltaTime;
        transform.position += moveDir * moveDistance;
        //현재 이동 거리 기록
        cur_range += moveDistance;

        //최대 사거리 도달 시 삭제
        if (cur_range > max_range)
        {
            DestoryProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag) && target_Count > 0)
        {
            //이미 공격한 대상이면 충돌 처리를 하지 않음
            foreach (var item in hitted_col)
                if (item == collision)
                    return;

            //공격 대상 유닛
            Unit target_Unit = collision.GetComponent<Unit>();
            if (target_Unit == null)
                return;
            //공격한 대상 저장
            hitted_col.Add(collision);
            //공격 전달
            if (TryAttack(target_Unit))
            {
                //공격 명중
                ApplyAttack(target_Unit);
                switch (attackType)
                {
                    case AttackType.None:
                        break;
                    case AttackType.Physical:
                        AudioManager.instance.PlayerSfx(SFX_Enum.Hit_Physic);
                        break;
                    case AttackType.Magical:
                        AudioManager.instance.PlayerSfx(SFX_Enum.Hit_Magic);
                        break;
                    case AttackType.Fire:
                        AudioManager.instance.PlayerSfx(SFX_Enum.Hit_Fire);
                        break;
                    default:
                        break;
                }
            }
            else
                AudioManager.instance.PlayerSfx(SFX_Enum.Avoid);

            //최대 대상 수만큼 공격 전달을 완료했으면 투사체 삭제
            if (--target_Count <= 0)
                DestoryProjectile();
        }
    }

    //공격 전달 판정을 반환
    bool TryAttack(Unit target_Unit)
    {
        //명중 확률
        float pro = accuracy - target_Unit.unitData_st.avoidance + 50f;
        //최소 확률 5%
        pro = pro > 5 ? pro : 5;
        return Random.Range(0, 100) < pro;
    }

    //공격 명중 시 피해를 주는 함수
    protected virtual void ApplyAttack(Unit target_Unit)
    {
        float type_res = attackType== target_Unit.ud.resistance_Type ? 0.5f : 1;
        float type_weak = attackType == target_Unit.ud.weak_Type ? 2f : 1;

        //최종 피해량
        float totalDamage;
        //관통공격일 경우
        if (isPenetration)
            totalDamage = damage * type_weak;
        else
            totalDamage = (damage - target_Unit.unitData_st.armor) * (type_res * type_weak);

        //최소 피해량 1
        totalDamage = totalDamage < 1 ? 1 : totalDamage;
        target_Unit.TakeDamage(totalDamage);

        FxManager.Instance.Hit(transform.position);
    }


    //unit에게 데이터를 받아오는 함수
    public virtual void SetData(Unit unit)
    {
        IsTeam = unit.IsTeam;
        target_Count = unit.ud.target_Count;
        //사거리는 공격한 유닛의 사거리 * 1.2
        max_range = unit.ud.attack_Range * 1.2f;
        damage = unit.ud.damage;
        accuracy = unit.ud.accuracy;
        attackType = unit.ud.attack_Type;
        isPenetration = unit.isPenetration;
    }

    //투사체를 삭제하는 함수
    public void DestoryProjectile()
    {
        Destroy(gameObject);
    }
}
