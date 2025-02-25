using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    //������ ������ ��ġ
    [SerializeField] Transform spawn_Trans;
    //���� ������Ʈ�� �� �θ�
    [SerializeField] Transform enemy_Unit_Parent;

    // spawn_Units[m,n] -> ���ֹ�ȣ m-n(m���̺� n��°) 
    BaseUnit[,] spawn_Units = new BaseUnit[3,3];
    //���� �ð�
    float[] spawn_Time = { 10, 20, 30 };
    //���� �ð� ī����
    float[] spawn_Time_Count = new float[3];



    [SerializeField] BaseUnit[] test_units = new BaseUnit[3];
    private void Start()
    {
        //������ ���� ������ ����(�ӽ�)
        for (int i = 0; i < test_units.Length; i++)
        {
            spawn_Units[0, i] = test_units[i];
        }

        Spawn_Unit(spawn_Units[0, 0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Spawn_Unit(spawn_Units[0,0]);
        }

        /*
        for (int i = 0; i < spawn_Time_Count.Length; i++)
        {
            //�ش� ���� ��ȣ�� ��������� ���� �ð� ī��Ʈ���� ����
            if (spawn_Units[0,i] == null)
                continue;

            //���� �ð����� ���� ����
            spawn_Time_Count[i] += Time.deltaTime;
            if (spawn_Time_Count[i] >= spawn_Time[i])
            {
                Spawn_Unit(spawn_Units[0, i]);
                spawn_Time_Count[i] = 0;
            }
        }
        */
    }

    //���� ���� �Լ�
    void Spawn_Unit(BaseUnit unit)
    {
        //���� ����
        BaseUnit baseUnit = Instantiate(unit, spawn_Trans);
        baseUnit.transform.position += Vector3.up * Random.Range(-0.25f, 0.25f);
        //�θ� ����(�� ���ֵ鸸 ��Ƴ��� Gameobject)
        baseUnit.transform.parent = enemy_Unit_Parent;
        //�� ����
        baseUnit.IsTeam = false;
    }
}
