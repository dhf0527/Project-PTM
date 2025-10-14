using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour
{
    public TMP_Text name_Text;
    public Image item_Image;
    public TMP_Text description_Text;

    //public Image itemNamePanel_Image;
    //public List<Sprite> itemNamePanel_Sprites;

    public void SetData(ItemData id)
    {
        name_Text.text = id.itemName;
        item_Image.sprite = id.itemIcon;

        //itemNamePanel_Image.sprite = itemNamePanel_Sprites[(int)id.itemRarity];

        //{value}를 item.itemValue로 변환
        string fixed_Text = Regex.Replace(id.itemDescription, @"\{value\}", id.itemValue.ToString());
        description_Text.text = fixed_Text;

    }

    public void SetData(MealData md)
    {
        name_Text.text = md.mealName;
        item_Image.sprite = md.mealIcon;

        //itemNamePanel_Image.sprite = itemNamePanel_Sprites[(int)md.mealRarity];

        //{value}를 meal.mealValue로 변환
        string fixed_Text = Regex.Replace(md.mealDescription, @"\{value\}", md.mealValue.ToString());
        fixed_Text = Regex.Replace(fixed_Text, @"\{value2\}", md.mealValue2.ToString());
        description_Text.text = fixed_Text;
    }
}
