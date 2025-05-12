using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour
{
    public ItemData id;

    public Image item_Image;
    public TMP_Text itemName_Text;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        item_Image.sprite = id.itemIcon;
        itemName_Text.text = id.itemName;
    }

    public void OnClick()
    {
        PediaManager.instance.SetItemData(id);
    }
}
