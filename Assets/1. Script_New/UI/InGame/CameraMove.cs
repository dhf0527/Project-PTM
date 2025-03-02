using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] float move_Speed;

    [HideInInspector] public bool isPrincessDead;
    [HideInInspector] public bool isChasePrincess;

    Princess princess;
    float min_x;
    float max_x;

    private void Start()
    {
        isChasePrincess = true;

        princess = DunGeonManager_New.instance.princess;

        min_x = DunGeonManager_New.instance.boundary_Min_x / 2.18f;
        max_x = DunGeonManager_New.instance.boundary_Max_x / 2.18f;
    }

    private void Update()
    {
        if (!isPrincessDead && isChasePrincess)
        {
            //공주의 x좌표를 따라감(경계선을 넘지 않도록)
            MoveCamera(princess.transform.position.x);
        }
    }

    //카메라를 move_Pos로 이동시키는 함수
    public void MoveCamera(float move_Pos_X)
    {
        Vector3 tmp_Pos = transform.position;
        //경계선 보정
        tmp_Pos.x = Mathf.Clamp(move_Pos_X, min_x, max_x);
        transform.position = tmp_Pos;
    }
}
