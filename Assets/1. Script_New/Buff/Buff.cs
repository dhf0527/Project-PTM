using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    [HideInInspector] public Unit unit;

    //버프 지속 시간
    public float buff_Time;
    //버프 아이콘 인덱스(hpBar 프리팹의 buffIcons의 index와 동일)
    public int buffIcon_Index;

    protected float cur_BuffTime = 0;

    protected void Start()
    {
        //요새 디버프 면역
        if (GetComponent<EnemyBase_Unit>() || GetComponent<TeamBase_Unit>())
        {
            Destroy(this);
            return;
        }

        unit = GetComponent<Unit>();

        //초기화
        Init();

        

        BuffStart();
    }

    private void Awake()
    {
        //버프 중복 방지
        if (PreventStack())
            return;
    }

    protected void Update()
    {
        if(cur_BuffTime < buff_Time)
            cur_BuffTime += Time.deltaTime;
        else
        {
            Destroy(this);
            BuffEnd();
        }
    }

    protected abstract void Init();
    protected virtual void BuffStart()
    {
        if(buffIcon_Index != -1)
            unit.SetBuffIcon(buffIcon_Index, true);
    }
    public virtual void BuffEnd()
    {
        if(buffIcon_Index != -1)
            unit.SetBuffIcon(buffIcon_Index, false);
    }
    protected abstract bool PreventStack();
}
