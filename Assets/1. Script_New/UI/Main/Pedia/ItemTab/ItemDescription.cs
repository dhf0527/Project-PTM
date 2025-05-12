using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemDescription : MonoBehaviour
{
    public TMP_Text name_Text;
    public Image item_Image;
    public TMP_Text description_Text;

    public void SetItemData(ItemData id)
    {
        name_Text.text = id.itemName;
        item_Image.sprite = id.itemIcon;

        //{value}를 item.itemValue로 변환
        string fixed_Text = Regex.Replace(id.itemDescription, @"\{value\}", id.itemValue.ToString());
        description_Text.text = fixed_Text;

    }
}
