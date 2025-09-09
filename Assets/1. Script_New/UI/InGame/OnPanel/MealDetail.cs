using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MealDetail : MonoBehaviour
{
    [SerializeField] Meal_Card meal_Card;

    private void Start()
    {
        MealData md = GameManager.Instance.current_Meal;

        meal_Card.Md = md;
    }
}
