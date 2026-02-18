using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
            }

            return instance;
        }
    }
    #endregion

    [HideInInspector] public MealData[] applied_Meals = new MealData[2];
    [HideInInspector] public DungeonData current_Dungeon;
    [Header("유닛 업그레이드")]
    public List<UnitUpgradeData> unitUpgradeDatas;
    [Header("스타 달성 기준 시간(1,2,3성)")]
    public List<float> clearTimes;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
            Destroy(gameObject);
    }

    public bool CheckAppliedMeal(int meal_Code, out MealData md)
    {
        md = null;

        for (int i = 0; i < applied_Meals.Length; i++)
        {
            if (applied_Meals[i]?.code == meal_Code)
            {
                md = applied_Meals[i];
                break;
            }
        }

        return md != null;
    }

    public void ClearAppliedMeal()
    {
        for (int i = 0; i < applied_Meals.Length; i++)
            applied_Meals[i] = null;
    }
}
