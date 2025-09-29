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

    public Image itemNamePanel_Image;
    public List<Sprite> itemNamePanel_Sprites;

    public void SetItemData(ItemData id)
    {
        name_Text.text = id.itemName;
        item_Image.sprite = id.itemIcon;

        itemNamePanel_Image.sprite = itemNamePanel_Sprites[(int)id.itemRarity];

        //{value}를 item.itemValue로 변환
        string fixed_Text = Regex.Replace(id.itemDescription, @"\{value\}", id.itemValue.ToString());
        description_Text.text = fixed_Text;

    }
}
