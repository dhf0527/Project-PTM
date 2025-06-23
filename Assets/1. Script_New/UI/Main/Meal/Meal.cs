using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Meal : MonoBehaviour
{
    public List<MealData> mealDatas;

    [Header("panel, select, eating, complete순")]
    public List<GameObject> gameObjects;
    public List<Meal_Card> meal_Cards;
    public TMP_Text eat_Meal_Text;
    public Meal_Card eat_Meal_Card;
    public Button selectConfirm_Button;

    MealData selected_md;
    public void OpenGo(GameObject go)
    {
        foreach (var item in gameObjects)
        {
            item.SetActive(false);
        }
        go.SetActive(true);
    }

    //식사 선택지를 불러올 때 호출
    public void SetMealData()
    {
        int k = meal_Cards.Count;
        List<MealData> rand_Mds = GetRandomMealData(mealDatas, k);

        for (int i = 0; i < k; i++)
            meal_Cards[i].Md = rand_Mds[i];
            

        //마스크 초기화
        foreach (var item in meal_Cards)
            item.OnMask(false);
    }

    //새로고침 버튼을 눌렀을 때 호출
    public void OnResetMealData()
    {
        List<MealData> inputMealDatas = new List<MealData>(mealDatas);
        int k = meal_Cards.Count;

        //중복 제거
        for (int i = 0; i < k; i++)
            inputMealDatas.Remove(meal_Cards[i].Md);

        List<MealData> rand_Mds = GetRandomMealData(inputMealDatas, k);
        for (int i = 0; i < k; i++)
            meal_Cards[i].Md = rand_Mds[i];
    }

    //식사 k개 무작위로 뽑기 - FisherYates알고리즘
    List<MealData> GetRandomMealData(List<MealData> inputMds, int k)
    {
        List<MealData> mds = new List<MealData>(inputMds);
        for (int i = inputMds.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            MealData tmp_md = mds[j];
            mds[j] = mds[i];
            mds[i] = tmp_md;
        }
        return mds.GetRange(0, k);
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

        GameManager.Instance.current_Meal = selected_md;
    }
}

