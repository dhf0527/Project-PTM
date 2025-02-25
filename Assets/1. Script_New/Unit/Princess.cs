using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Princess : BaseUnit
{
    private void Update()
    {
        Move();
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
        OnEndAttack();
    }

    protected override void Move()
    {
        //이동량이 없으면 이동을 안 함
        if (!isMoving || moveDir.x == 0)
        {
            ScanEnemy();
            //스캔된 적이 있다면 공격
            if (hit.collider != null)
            {
                Attack();
            }

            //공격중이 아닐 때 idle 애니메이션 재생
            if (!isAttacking)
            {
                SetAnim(AnimState.idle);
            }
            return;
        }

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
    #endregion

}
