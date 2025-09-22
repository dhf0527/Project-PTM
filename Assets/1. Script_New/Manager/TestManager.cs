using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    [SerializeField] List<GameObject> testObjects;

    bool test_Is1;
    bool test_Is2;
    bool test_Is3;
    bool test_Is4;

#if UNITY_EDITOR
    private void OnEnable()
    {
        foreach (var testObject in testObjects)
            testObject.SetActive(true);
    }

    private void Update()
    {
        if (DunGeonManager_New.instance)
            Test_Dungeon();
        else if(MainManager.instance)
            Test_Main();
    }
#endif

    #region 튜토리얼
    public void Test_Tutorial(int keyIndex)
    {
        //모두 클리어 처리
        for (int i = 0; i < Enum.GetValues(typeof(TutorialKey)).Length; i++)
            PlayerPrefs.SetInt(ConstData.tutorialComplete + (TutorialKey)i, 1);

        PlayerPrefs.SetInt(ConstData.tutorialComplete + (TutorialKey)keyIndex, 0);
    }

    //튜토리얼 열람 초기화
    public void Test_ResetTutorial()
    {
        for (int i = 0; i < Enum.GetValues(typeof(TutorialKey)).Length; i++)
            PlayerPrefs.SetInt(ConstData.tutorialComplete + (TutorialKey)i, 0);
    }

    //튜토리얼 읽음 처리
    public void Test_TutorialComplete()
    {
        for (int i = 0; i < Enum.GetValues(typeof(TutorialKey)).Length; i++)
            PlayerPrefs.SetInt(ConstData.tutorialComplete + (TutorialKey)i, 1);
    }
    #endregion
    #region 해금
    //기능 해금
    public void TestUnlock(bool isUnlock)
    {
        PlayerPrefs.SetInt(ConstData.unitItem_Unlock, isUnlock ? 1 : 0);
        PlayerPrefs.SetInt(ConstData.heroUpgrade_Unlock, isUnlock ? 1 : 0);
        PlayerPrefs.SetInt(ConstData.meal_Unlock, isUnlock ? 1 : 0);
        PlayerPrefs.SetInt(ConstData.pedia_Unlock, isUnlock ? 1 : 0);
        PlayerPrefs.SetInt(ConstData.unitUpgrade_Unlock, isUnlock ? 4 : 0);

        UnlockManager.instance.CheckUnlock();
    }
    //스테이지 해금
    public void TestOpenStage(bool isOpen)
    {
        MainManager.instance.isOpenStage = isOpen;
    }
    #endregion
    #region 식사
    public void TestResetMealTime()
    {
        PlayerPrefs.SetString(ConstData.mealCompleteTime, "");
    }
    #endregion
    #region 소울
    public void Test_SoulPlus()
    {
        MainManager.instance.Soul += 5000;
    }
    public void Test_SoulReset()
    {
            MainManager.instance.Soul = 0;
    }
    #endregion

    void Test_Dungeon()
    {
        Unit princess = DunGeonManager_New.instance.princess;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            princess.unitStatData_st.moveSpeed_PlusPercent += !test_Is1 ? 400 : -400f;
            test_Is1 = !test_Is1;
            Debug.Log("이동 속도 증가" + (test_Is1 ? "On" : "Off"));
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            princess.unitStatData_st.accuracy_Plus += !test_Is2 ? 1000 : -1000;
            test_Is2 = !test_Is2;
            Debug.Log("명중률 증가" + (test_Is2 ? "On" : "Off"));
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            princess.unitStatData_st.avoidance_Plus += !test_Is3 ? 1000 : -1000;
            test_Is3 = !test_Is3;
            Debug.Log("회피율 증가" + (test_Is3 ? "On" : "Off"));
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            princess.unitStatData_st.attack_Plus += !test_Is4 ? 1000 : -1000;
            test_Is4 = !test_Is4;
            Debug.Log("공격력 증가" + (test_Is4 ? "On" : "Off"));
        }
        //골드 획득
        if (Input.GetKeyDown(KeyCode.F5))
            DunGeonManager_New.instance.Cur_Gold = DunGeonManager_New.instance.Max_Gold;
        //게임 승리
        if (Input.GetKeyDown(KeyCode.F6))
            DunGeonManager_New.instance.OpenGameClearPanel();
        //게임 패배
        if (Input.GetKeyDown(KeyCode.F7))
            DunGeonManager_New.instance.OpenGameOverPanel();

        //웨이브 넘기기
        if (Input.GetKeyDown(KeyCode.F8))
        {
            EnemySpawnManager.instance.ToNextWave();
        }
        //보스 소환
        else if (Input.GetKeyDown(KeyCode.F9))
        {
            EnemySpawnManager.instance.isBossSpawned = false;
            EnemySpawnManager.instance.OnBossSpawn();
        }
    }

    void Test_Main()
    {

    }
}
