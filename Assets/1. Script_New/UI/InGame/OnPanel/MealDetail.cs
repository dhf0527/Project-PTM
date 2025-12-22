using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MealDetail : MonoBehaviour
{
    [SerializeField] Meal_Card meal_Card;

    public void SetMealDataByIndex(int index)
    {
        meal_Card.Md = GameManager.Instance.applied_Meals[index];
    }
}
