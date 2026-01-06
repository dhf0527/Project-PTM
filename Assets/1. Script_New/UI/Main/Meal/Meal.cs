using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class Meal : MonoBehaviour
{
    public List<MealData> mealDatas_uncommon;
    public List<MealData> mealDatas_rare;
    public List<MealData> mealDatas_legendary;

    public List<MealData> hardMode_mealDatas_uncommon;
    public List<MealData> hardMode_mealDatas_rare;
    public List<MealData> hardMode_mealDatas_legendary;
    [Header("확률(%) -> 희귀 확률 = pro_legendary * rare_multi")]
    [SerializeField] int pro_legendary;
    [SerializeField] int rare_multi;

    [Header("panel, select, eating, complete, full, additionalMeal, full_Hardmode순")]
    public List<GameObject> gameObjects;
    public List<Meal_Card> meal_Cards;
    public TMP_Text eat_Meal_Text;
    public Meal_Card eat_Meal_Card;
    public Button selectConfirm_Button;
    public Button reroll_Button;
    public TMP_Text nextDate_Text;

    MealData selected_md;
    const string LastRerollKey = "LastRerollDate";


    public void OpenGo(GameObject go)
    {
        if (go == gameObjects[0])
        {
            string lastEatTime = PlayerPrefs.GetString(ConstData.mealCompleteTime, "");

            //최초 실행 체크
            if (!string.IsNullOrEmpty(lastEatTime))
            {
                //하드모드 해금, 식사 효과 있을 시
                if (PlayerPrefs.GetInt(ConstData.hardMode_Unlock) != 0 && GameManager.Instance.applied_Meals[0] != null)
                {
                    //추가 식사 가능할 시
                    if (GameManager.Instance.applied_Meals[GameManager.Instance.applied_Meals.Length - 1] == null)
                    {
                        go = gameObjects[5];
                        go.SetActive(true);
                        go.GetComponent<AdditionalMeal>().SetText();
                        go.SetActive(false);
                    }
                    else
                    {
                        go = gameObjects[6];
                        go.SetActive(true);
                        go.GetComponent<Full_HardMode>().SetText();
                        go.SetActive(false);
                    }
                }
                else if(PlayerPrefs.GetInt(ConstData.hardMode_Unlock) == 0)
                {
                    //마지막 식사 완료 시간 체크
                    DateTime lastTime = DateTime.Parse(lastEatTime);
                    DateTime nowTime = DateTime.Now;
                    TimeSpan difference = nowTime - lastTime;
                    if (difference.TotalMinutes < 2)
                        go = gameObjects[4];
                }
            }
        }

        foreach (var item in gameObjects)
            item.SetActive(false);
        go.SetActive(true);
    }

    //식사 선택지를 불러올 때 호출
    public void SetMealData()
    {
        int k = meal_Cards.Count;
        int rand = UnityEngine.Random.Range(0, 100);

        List<MealData> rand_Mds = GetRandomMealData(mealDatas_uncommon, mealDatas_rare, mealDatas_legendary, k);

        for (int i = 0; i < k; i++)
            meal_Cards[i].Md = rand_Mds[i];
            

        //마스크 초기화
        foreach (var item in meal_Cards)
            item.OnMask(false);

        reroll_Button.interactable = CanDailyReroll();
    }

    #region 새로고침
    //새로고침 버튼을 눌렀을 때 호출
    public void OnRerollMealData()
    {
        if(CanDailyReroll())
        {
            RerollMealData();

            //날짜 업데이트
            PlayerPrefs.SetString(LastRerollKey, DateTime.Now.ToString());
            CanDailyReroll();
        }
    }

    //하루 한 번 실행하도록 확인하는 함수
    bool CanDailyReroll()
    {
        bool canReroll;

        string lastRerollDate = PlayerPrefs.GetString(LastRerollKey, "");

        //최초 실행 체크
        if (string.IsNullOrEmpty(lastRerollDate))
            canReroll = true;
        else
        {
            DateTime lastDate = DateTime.Parse(lastRerollDate);
            DateTime currentDate = DateTime.Now;

            canReroll = lastDate.Date != currentDate.Date;
        }

        nextDate_Text.gameObject.SetActive(!canReroll);
        reroll_Button.interactable = canReroll;

        return canReroll;
    }

    //새로고침
    public void RerollMealData()
    {
        List<MealData> inputMealDatas_uncommon = new List<MealData>(mealDatas_uncommon);
        List<MealData> inputMealDatas_rare = new List<MealData>(mealDatas_rare);
        List<MealData> inputMealDatas_legendary = new List<MealData>(mealDatas_legendary);
        int k = meal_Cards.Count;

        //중복 제거
        for (int i = 0; i < k; i++)
        {
            if (meal_Cards[i].Md.mealRarity == MealRarity.Uncommon)
                inputMealDatas_uncommon.Remove(meal_Cards[i].Md);
            else if(meal_Cards[i].Md.mealRarity == MealRarity.Rare)
                inputMealDatas_rare.Remove(meal_Cards[i].Md);
            else
                inputMealDatas_legendary.Remove(meal_Cards[i].Md);
        }

        List<MealData> rand_Mds = GetRandomMealData(inputMealDatas_uncommon, inputMealDatas_rare, inputMealDatas_legendary, k);
        for (int i = 0; i < k; i++)
            meal_Cards[i].Md = rand_Mds[i];
    }

    //테스트 - 리롤 날짜 초기화
    public void TestResetDate()
    {
        PlayerPrefs.SetString(LastRerollKey, "");
        reroll_Button.interactable = CanDailyReroll();
    }
    #endregion

    //식사 k개 무작위로 뽑기
    List<MealData> GetRandomMealData(List<MealData> inputMealDatas_uncommon, List<MealData> inputMealDatas_rare, List<MealData> inputMealDatas_legendary,  int k)
    {
        List<MealData> mds_pool_uncommon = new List<MealData>(inputMealDatas_uncommon);
        List<MealData> mds_pool_rare = new List<MealData>(inputMealDatas_rare);
        List<MealData> mds_pool_legendary = new List<MealData>(inputMealDatas_legendary);

        //유료 요리 추가
        if(PlayerPrefs.GetInt(ConstData.hardMode_Unlock) != 0)
        {
            mds_pool_uncommon.AddRange(hardMode_mealDatas_uncommon);
            mds_pool_rare.AddRange(hardMode_mealDatas_rare);
            mds_pool_legendary.AddRange(hardMode_mealDatas_legendary);
        }

        //이미 적용된 식사 제거
        for (int i = 0; i < GameManager.Instance.applied_Meals.Length; i++)
        {
            if (GameManager.Instance.applied_Meals[i] == null)
            {
                continue;
            }

            MealData tmp_md = GameManager.Instance.applied_Meals[i];
            if (tmp_md.mealRarity == MealRarity.Uncommon)
                mds_pool_uncommon.Remove(tmp_md);
            else if (tmp_md.mealRarity == MealRarity.Rare)
                mds_pool_rare.Remove(tmp_md);
            else
                mds_pool_legendary.Remove(tmp_md);
        }

        List<MealData> rand_Mds = new();
        for (int i = 0; i < k; i++)
        {
            int rand = UnityEngine.Random.Range(0, 100);
            int tmp_pro_legendary = pro_legendary;
            int tmp_pro_rare = pro_legendary * rare_multi;

            //행운의 용병단
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 12);
            if (upgradeLv != 0)
            {
                tmp_pro_rare += (int)DunGeonManager_New.instance.unitUpgradeDatas[12].upgradeValue[upgradeLv - 1];
                tmp_pro_legendary = tmp_pro_rare / rare_multi;
            }

            int pro_uncommon = 100 - (tmp_pro_legendary + tmp_pro_rare);

            //첫번째 요리가 전설일 경우 확률 변경
            if (GameManager.Instance.applied_Meals[0] != null && GameManager.Instance.applied_Meals[0].mealRarity == MealRarity.Legendary)
            {
                tmp_pro_rare = tmp_pro_legendary;
                tmp_pro_legendary = 0;
                pro_uncommon = 100 - (tmp_pro_legendary + tmp_pro_rare);
            }

            if (rand < tmp_pro_legendary && mds_pool_legendary.Count > 0)
            {
                MealData tmp_md = mds_pool_legendary[UnityEngine.Random.Range(0, mds_pool_legendary.Count)];
                rand_Mds.Add(tmp_md);
                mds_pool_legendary.Remove(tmp_md);
            }
            else if (rand < tmp_pro_legendary + tmp_pro_rare)
            {
                MealData tmp_md = mds_pool_rare[UnityEngine.Random.Range(0, mds_pool_rare.Count)];
                rand_Mds.Add(tmp_md);
                mds_pool_rare.Remove(tmp_md);
            }
            else
            {
                MealData tmp_md = mds_pool_uncommon[UnityEngine.Random.Range(0, mds_pool_uncommon.Count)];
                rand_Mds.Add(tmp_md);
                mds_pool_uncommon.Remove(tmp_md);
            }
        }
        return rand_Mds;
    }

    //식사를 클릭했을 때 호출
    public void SelectMeal()
    {
        foreach (var item in meal_Cards)
        {
            if(item.GetComponent<Toggle>().isOn)
            {
                item.OnMask(false);
                selected_md = item.Md;
            }
            else
                item.OnMask(true);
        }
        selectConfirm_Button.interactable = true;
    }

    //식사 선택 완료했을 때 호출
    public void SetEatMealData()
    {
        eat_Meal_Text.text = $"{selected_md.mealName} 식사 완료!";
        eat_Meal_Card.Md = selected_md;

        if (GameManager.Instance.applied_Meals[0] == null)
            GameManager.Instance.applied_Meals[0] = selected_md;
        else
            GameManager.Instance.applied_Meals[1] = selected_md;

        //식사 완료 시간 기록
        PlayerPrefs.SetString(ConstData.mealCompleteTime, DateTime.Now.ToString());
    }

    public void ResetMealData()
    {
        for (int i = 0; i < GameManager.Instance.applied_Meals.Length; i++)
            GameManager.Instance.applied_Meals[i] = null;
        MainManager.instance.OnMeal();
    }
}

