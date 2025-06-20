using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] float move_Speed;

    [HideInInspector] public bool isPrincessDead;
    [HideInInspector] public bool isChasePrincess;
    [HideInInspector] public Vector3 shake_Pos = Vector3.zero;
    Vector3 pre_Shake_Pos = Vector3.zero;

    Princess princess;
    float min_x;
    float max_x;
    Coroutine c_Shake;

    private void Start()
    {
        isChasePrincess = true;

        princess = DunGeonManager_New.instance.princess;

        min_x = DunGeonManager_New.instance.boundary_Min_x / 2f;
        max_x = DunGeonManager_New.instance.boundary_Max_x / 2f;
    }

    private void Update()
    {
        if (!isPrincessDead && isChasePrincess)
        {
            //공주의 x좌표를 따라감(경계선을 넘지 않도록)
            MoveCamera(princess.transform.position.x);
        }
        //진동 누적 방지
        transform.position -= pre_Shake_Pos;
        transform.position += shake_Pos;
    }

    //카메라를 move_Pos로 이동시키는 함수
    public void MoveCamera(float move_Pos_X)
    {
        Vector3 tmp_Pos = transform.position;
        //경계선 보정
        tmp_Pos.x = Mathf.Clamp(move_Pos_X, min_x, max_x);
        transform.position = tmp_Pos;
    }

    //화면 진동
    public void ShakeCamera(float duration, float amount)
    {
        if(c_Shake != null)
        {
            StopCoroutine(c_Shake);
            shake_Pos = Vector3.zero;
        }
        c_Shake = StartCoroutine(C_ShakeCamera(duration, amount));
    }

    IEnumerator C_ShakeCamera(float duration, float amount)
    {
        float curTime = 0;
        while(curTime < duration)
        {
            curTime += Time.deltaTime;
            pre_Shake_Pos = shake_Pos;
            shake_Pos = (Vector3)Random.insideUnitCircle * amount * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        shake_Pos = Vector3.zero;
        pre_Shake_Pos = Vector3.zero;
    }
}
