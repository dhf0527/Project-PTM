using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Meal_Card : MonoBehaviour
{
    public Image icon_Image;
    public TMP_Text mealName_Text;
    public TMP_Text mealDescription_Text;
    public GameObject mask_Go;

    MealData md;
    public MealData Md 
    {
        get { return md; }
        set
        {
            md = value;
            SetData();
        }
    }

    public void SetData()
    {
        icon_Image.sprite = Md.mealIcon;
        mealName_Text.text = Md.mealName;

        string mealDsc = Regex.Replace(Md.mealDescription, @"\{value\}", Md.mealValue.ToString());
        mealDsc = Regex.Replace(mealDsc, @"\{value2\}", Md.mealValue2.ToString());
        mealDescription_Text.text = mealDsc;
    }

    public void OnMask(bool isActive)
    {
        mask_Go.SetActive(isActive);
    }

    public void OnClickSound(bool isOn)
    {
        if(isOn)
            AudioManager.Instance.PlayerSfx(SFX_Enum.Touch);
    }
}
