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
    [SerializeField] UnitSpawnButton[] unitSpawnButton = new UnitSpawnButton[3];
    #endregion

    //싱글톤
    public static DunGeonManager_New instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        boundary_Min_x = boundary.bounds.min.x;
        boundary_Max_x = boundary.bounds.max.x;
    }


}
