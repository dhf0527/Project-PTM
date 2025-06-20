using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MealPanel : MonoBehaviour
{
    public Image meal_Icon;

    private void Start()
    {
        if (GameManager.Instance.current_Meal == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        meal_Icon.sprite = GameManager.Instance.current_Meal.mealIcon;
    }
}
