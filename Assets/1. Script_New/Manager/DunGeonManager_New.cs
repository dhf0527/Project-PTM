using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] BaseUnit[] spawnUnits = new BaseUnit[3];

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

    [Header("유닛간 Y축 차이")]
    public float spawn_Y = 0.03f;
    #endregion
    [HideInInspector] public Princess princess;

    //싱글톤
    public static DunGeonManager_New instance;
    private void Awake()
    {
        instance = this;

        princess = FindAnyObjectByType<Princess>();

        //버튼과 유닛 연동(임시)
        for (int i = 0; i < unitSpawnButton.Length; i++)
        {
            unitSpawnButton[i].unit = spawnUnits[i];
        }
    }

    private void Start()
    {
        boundary_Min_x = boundary.bounds.min.x;
        boundary_Max_x = boundary.bounds.max.x;
    }

    //생산 버튼을 눌렀을 때 호출될 함수
    public void OnSpawnUnit(int i)
    {
        BaseUnit unit = Instantiate(spawnUnits[i], spawn_Trans);
        unit.transform.position += SpawnY();
        unit.transform.parent = unit_Parent;
        unit.IsTeam = true;
    }

    public Vector3 SpawnY()
    {
        int rand = Random.Range(-5, 6);
        Vector3 return_Vec = Vector3.up * rand * spawn_Y + Vector3.forward * rand * spawn_Y;
        return return_Vec;
    }

    //공주 부활 쿨타임
    public void PrincessCoolDown()
    {
        princessHpPanel.rest_Time = 3f;
    }

    //공주 부활
    public void PrincessRivive()
    {
        //카메라 설정
        cameraMove.isPrincessDead = false;
        //스폰 위치로 이동
        princess.transform.position = spawn_Trans.position;
        princess.Rivive();
    }
}
