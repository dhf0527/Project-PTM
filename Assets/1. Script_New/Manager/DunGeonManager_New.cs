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
    [Header("UI ����")]
    //���� ���� ��ư
    [SerializeField] UnitSpawnButton[] unitSpawnButton = new UnitSpawnButton[3];
    //������ ����(�ӽ�)
    [SerializeField] BaseUnit[] spawnUnits = new BaseUnit[3];
    
    //World Space Canvas
    public Transform worldCanvas_Trans;
    #endregion
    #region ���� ���� ����
    [Header("���� ���� ����")]
    //������ ������ ��ġ
    [SerializeField] Transform spawn_Trans;
    //����(��)�� �θ�
    [SerializeField] Transform unit_Parent;
    #endregion


    //�̱���
    public static DunGeonManager_New instance;
    private void Awake()
    {
        instance = this;

        //��ư�� ���� ����(�ӽ�)
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

    //���� ��ư�� ������ �� ȣ��� �Լ�
    public void OnSpawnUnit(int i)
    {
        BaseUnit unit = Instantiate(spawnUnits[i], spawn_Trans);
        unit.transform.position += Vector3.up * Random.Range(-0.25f, 0.25f);
        unit.transform.parent = unit_Parent;
        unit.IsTeam = true;
    }
}
