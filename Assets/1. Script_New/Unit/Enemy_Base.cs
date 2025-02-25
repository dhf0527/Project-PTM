using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    //유닛이 생성될 위치
    [SerializeField] Transform spawn_Trans;
    //유닛 오브젝트가 들어갈 부모
    [SerializeField] Transform enemy_Unit_Parent;

    // spawn_Units[m,n] -> 유닛번호 m-n(m웨이브 n번째) 
    BaseUnit[,] spawn_Units = new BaseUnit[3,3];
    //스폰 시간
    float[] spawn_Time = { 10, 20, 30 };
    //스폰 시간 카운터
    float[] spawn_Time_Count = new float[3];



    [SerializeField] BaseUnit[] test_units = new BaseUnit[3];
    private void Start()
    {
        //스폰할 유닛 데이터 삽입(임시)
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
            //해당 유닛 번호가 비어있으면 스폰 시간 카운트하지 않음
            if (spawn_Units[0,i] == null)
                continue;

            //스폰 시간마다 유닛 생산
            spawn_Time_Count[i] += Time.deltaTime;
            if (spawn_Time_Count[i] >= spawn_Time[i])
            {
                Spawn_Unit(spawn_Units[0, i]);
                spawn_Time_Count[i] = 0;
            }
        }
        */
    }

    //유닛 생산 함수
    void Spawn_Unit(BaseUnit unit)
    {
        //유닛 생산
        BaseUnit baseUnit = Instantiate(unit, spawn_Trans);
        baseUnit.transform.position += Vector3.up * Random.Range(-0.25f, 0.25f);
        //부모 설정(적 유닛들만 모아놓은 Gameobject)
        baseUnit.transform.parent = enemy_Unit_Parent;
        //팀 설정
        baseUnit.IsTeam = false;
    }
}
