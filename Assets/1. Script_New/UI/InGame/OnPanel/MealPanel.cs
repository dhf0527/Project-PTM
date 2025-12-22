using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MealPanel : MonoBehaviour
{
    [SerializeField] MealDetail mealDetail;
    public int meal_Index;
    public Image meal_Icon;

    private void Start()
    {
        if (GameManager.Instance.applied_Meals[meal_Index] == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        meal_Icon.sprite = GameManager.Instance.applied_Meals[meal_Index].mealIcon;
    }

    public void OnButtonClick()
    {
        mealDetail.SetMealDataByIndex(meal_Index);
    }
}
