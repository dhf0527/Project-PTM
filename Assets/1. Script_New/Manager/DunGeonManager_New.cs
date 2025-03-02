using System;
using System.Collections;
using UnityEngine;

public class DunGeonManager_New : MonoBehaviour
{
    //경계선 오브젝트
    [SerializeField] BoxCollider2D boundary;
    //경계선 양끝 좌표
    [HideInInspector] public float boundary_Min_x;
    [HideInInspector] public float boundary_Max_x;

    #region UI 변수
    [Header("UI 변수")]
    //유닛 생산 버튼
    [SerializeField] UnitSpawnButton[] unitSpawnButton = new UnitSpawnButton[3];
    //생산할 유닛(임시)
    [SerializeField] Unit[] spawnUnits = new Unit[3];

    //카메라
    public CameraMove cameraMove;
    //공주 체력 패널
    public PrincessHpPanel princessHpPanel;
    #endregion
    #region 유닛 생산 변수
    [Header("유닛 생산 변수")]
    //유닛을 생산할 위치
    [SerializeField] Transform spawn_Trans;
    //유닛(팀)의 부모
    [SerializeField] Transform unit_Parent;

    [Header("유닛간 최소 Y축 차이")]
    public float spawn_Y = 0.03f;
    #endregion
    //투사체 부모
    public Transform projectile_Parent;

    [HideInInspector] public Princess princess;

    //싱글톤
    public static DunGeonManager_New instance;
    private void Awake()
    {
        instance = this;

        //공주 찾아서 저장
        princess = FindAnyObjectByType<Princess>();

        //버튼과 유닛 연동(임시)
        for (int i = 0; i < unitSpawnButton.Length; i++)
        {
            unitSpawnButton[i].unit = spawnUnits[i];
        }
    }

    private void Start()
    {
        //경계선 양끝 좌표 가져오기
        boundary_Min_x = boundary.bounds.min.x;
        boundary_Max_x = boundary.bounds.max.x;
    }

    #region 유닛 생산 함수
    //생산 버튼을 눌렀을 때 호출될 함수
    public void OnSpawnUnit(int index)
    {
        StartCoroutine(C_SpawnUnit(index));
    }

    //실제로 생산을 하는 함수
    IEnumerator C_SpawnUnit(int index)
    {
        //생산 수만큼 반복
        for (int i = 0; i < spawnUnits[index].ud.spawn_Count; i++)
        {
            //유닛 하나 생성 및 설정
            Unit unit = Instantiate(spawnUnits[index], spawn_Trans);
            unit.transform.position += SpawnY();
            unit.transform.parent = unit_Parent;
            unit.IsTeam = true;
            //생산 딜레이
            yield return new WaitForSeconds(0.5f);
        }
    }

    //유닛이 생산될 Y축 벡터를 반환하는 함수
    public Vector3 SpawnY()
    {
        int rand = UnityEngine.Random.Range(-5, 6);
        Vector3 return_Vec = Vector3.up * rand * spawn_Y + Vector3.forward * rand * spawn_Y;
        return return_Vec;
    }
    #endregion

    #region 공주 관련 함수
    //공주 부활을 쿨타임 설정하는 함수
    public void PrincessCoolDown()
    {
        princessHpPanel.rest_Time = 3f;
    }

    //공주 부활
    public void PrincessRivive()
    {
        //카메라 설정
        cameraMove.isPrincessDead = false;
        cameraMove.isChasePrincess = true;
        //스폰 위치로 이동
        princess.transform.position = spawn_Trans.position;
        princess.Rivive();
    }
    #endregion
}
