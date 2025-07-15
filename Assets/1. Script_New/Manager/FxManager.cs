using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    public static FxManager Instance;

    public Hit hit_prf;
    public Transform fxParent;

    public DamageText damagText_prf;
    public Transform worldCanvas;

    Queue<Hit> q_hit = new();
    Queue<DamageText> q_damageText = new();

    private void Awake()
    {
        Instance = this;
    }

    public void Hit(Vector3 pos)
    {
        //오브젝트 풀링

        Hit cur_Hit;
        //대기중 hit 없을 때 생성
        if(q_hit.Count == 0)
        {
            cur_Hit = Instantiate(hit_prf, fxParent);
        }
        //있을 때 풀링
        else
        {
            cur_Hit = q_hit.Dequeue();
            cur_Hit.gameObject.SetActive(true);
        }

        cur_Hit.transform.position = pos;
    }

    public void DisableHit(Hit hit)
    {
        hit.gameObject.SetActive(false);
        q_hit.Enqueue(hit);
    }

    //오브젝트 풀링
    public void DamageText(Vector3 pos , float damage, AttackType attackType)
    {
        if (!damagText_prf)
            return;

        DamageText cur_DamageText;
        //대기중 damageText 없을 때 생성
        if (q_damageText.Count == 0)
        {
            cur_DamageText = Instantiate(damagText_prf, worldCanvas);
        }
        //있을 때 풀링
        else
        {
            cur_DamageText = q_damageText.Dequeue();
            cur_DamageText.gameObject.SetActive(true);
        }

        cur_DamageText.transform.position = pos;

        cur_DamageText.SetText(damage, attackType);
    }

    public void DisableDamageText(DamageText damageText)
    {
        damageText.gameObject.SetActive(false);
        q_damageText.Enqueue(damageText);
    }
}
