using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DunGeonManager_New : MonoBehaviour
{
    //��輱 ������Ʈ
    [SerializeField] BoxCollider2D boundary;
    //��輱 �糡 ��ǥ
    [HideInInspector] public float boundary_Min_x;
    [HideInInspector] public float boundary_Max_x;

    #region UI ����
    [SerializeField] UnitSpawnButton[] unitSpawnButton = new UnitSpawnButton[3];
    #endregion

    //�̱���
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
