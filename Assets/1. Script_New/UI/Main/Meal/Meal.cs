using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Meal : MonoBehaviour
{
    public List<MealData> mealDatas;

    [Header("panel, select, eating, complete순")]
    public List<GameObject> gameObjects;
    public List<Meal_Card> meal_Cards;
    public TMP_Text eat_Meal_Text;
    public Meal_Card eat_Meal_Card;

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
        #region 중복 없이 카드 개수만큼 식사 뽑기
        List<int> numbers = new List<int>();
        for (int i = 0; i < mealDatas.Count; i++)
            numbers.Add(i);

        for (int k = 0; k < meal_Cards.Count; k++)
        {
            int index = Random.Range(0, numbers.Count);
            meal_Cards[k].Md = mealDatas[numbers[index]];
            numbers.RemoveAt(index);
        }
        #endregion

        //마스크 초기화
        foreach (var item in meal_Cards)
            item.OnMask(false);
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
    }

    //식사 선택 완료했을 때 호출
    public void SetEatMealData()
    {
        eat_Meal_Text.text = $"{selected_md.mealName} 식사 완료!";
        eat_Meal_Card.Md = selected_md;

        MainManager.instance.mealData = selected_md;
    }
}

