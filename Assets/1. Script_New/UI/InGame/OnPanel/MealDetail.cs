using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MealDetail : MonoBehaviour
{
    public Image mealIcon_Image;
    public TMP_Text mealName_Text;
    public TMP_Text mealDescription_Text;

    private void Start()
    {
        MealData md = GameManager.Instance.current_Meal;

        mealIcon_Image.sprite = md.mealIcon;
        mealName_Text.text = md.mealName;

        string mealDsc = Regex.Replace(md.mealDescription, @"\{value\}", md.mealValue.ToString());
        mealDescription_Text.text = mealDsc;
    }
}
