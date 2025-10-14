using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MealCell : MonoBehaviour
{
    public MealData md;

    public Image item_Image;
    public TMP_Text itemName_Text;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        item_Image.sprite = md.mealIcon;
        itemName_Text.text = md.mealName;
    }

    public void OnClick()
    {
        PediaManager.instance.SetData(md);
    }
}
