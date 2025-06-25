using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Meal", menuName = "Scriptable Object/MealData")]
public class MealData : ScriptableObject
{
    public int code;
    public string mealName;
    public Sprite mealIcon;
    public float mealValue;
    public float mealValue2;
    public string mealDescription;
    public MealRarity mealRarity;
}
