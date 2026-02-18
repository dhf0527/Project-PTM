using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullGiant : Unit
{
    [Header("방어력 증가량 (순서대로 hpCut1%이하, hpCut2%이하, hpCut2%이상)")]
    [SerializeField] List<int> buffValues;
    [SerializeField] int hpCut1;
    [SerializeField] int hpCut2;

    [Header("유닛 사망 감지 범위(반지름)")]
    [SerializeField] float detectionRadius;
    List<Unit> detectedUnits = new();

    [Header("시체 수집 회복량(고정 수치)")]
    [SerializeField] float healValue;

    public override void Init()
    {
        base.Init();
        SkullShell skullShell = gameObject.AddComponent<SkullShell>();
        skullShell.hpCut1 = hpCut1;
        skullShell.hpCut2 = hpCut2;
        skullShell.buffValues = buffValues;
        skullShell.buff_Time = int.MaxValue;
    }

    private void Update()
    {
        base.Update();

        if (!isDead)
        {
            ScanUnit();
            RemoveScanUnit();
        }
    }

    void ScanUnit()
    {
        int targetLayer = 1 << LayerMask.NameToLayer(IsTeam ? "Team" : "Enemy");
        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, targetLayer);

        foreach (var item in detectedColliders)
        {
            if (item.gameObject == gameObject)
                continue;

            if(item.TryGetComponent(out Unit detectedUnit))
                if(!detectedUnits.Contains(detectedUnit))
                {
                    detectedUnits.Add(detectedUnit);
                    detectedUnit.deadEvent += CorpseCollect;
                }
        }
    }

    void RemoveScanUnit()
    {
        for (int i = detectedUnits.Count - 1; i >= 0; i--)
        {
            Unit checkUnit = detectedUnits[i];
            if (checkUnit == null || Vector3.Distance(transform.position, checkUnit.transform.position) > detectionRadius)
            {
                if(checkUnit != null)
                    detectedUnits[i].deadEvent -= CorpseCollect;
                detectedUnits.RemoveAt(i);
            }
        }
    }

    //시체 수집
    void CorpseCollect()
    {
        GetHp(healValue);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public override void Dead()
    {
        base.Dead();

        foreach (var item in detectedUnits)
            item.deadEvent -= CorpseCollect;

        detectedUnits.Clear();
    }
}
