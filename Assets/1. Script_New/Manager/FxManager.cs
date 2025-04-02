using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    public static FxManager Instance;

    public Hit hit_prf;
    public Transform fxParent;

    Queue<Hit> q_hit = new Queue<Hit>();

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
}
