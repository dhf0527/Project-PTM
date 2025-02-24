using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Princess : BaseUnit
{
    private void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.F1))
        {
            unitData_st.moveSpeed = 1f;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            unitData_st.moveSpeed = 4f;
        }
    }

    public override void Init()
    {
        base.Init();
        IsTeam = true;
        moveDir = Vector3.zero;
    }

    #region 이동 함수
    //버튼을 눌렀을 때 이동 방향/속도 설정
    public void OnMove(int move_dir)
    {
        moveDir = Vector3.right * move_dir;
        SetDir();
    }

    protected override void Move()
    {
        base.Move();
    }
    #endregion

}
