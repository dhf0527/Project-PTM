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

    #region �̵� �Լ�
    //��ư�� ������ �� �̵� ����/�ӵ� ����
    public void OnMove(int move_dir)
    {
        moveDir = Vector3.right * move_dir;
        SetDir();
        OnEndAttack();
    }

    protected override void Move()
    {
        //�̵����� ������ �̵��� �� ��
        if (!isMoving || moveDir.x == 0)
        {
            ScanEnemy();
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
    #endregion

}
