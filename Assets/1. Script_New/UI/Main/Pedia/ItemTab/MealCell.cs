using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MealCell : MonoBehaviour
{
    public MealData md;

    public Image item_Image;
    [SerializeField] GameObject lockImage_go;
    public TMP_Text itemName_Text;

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        item_Image.sprite = md.mealIcon;

        if (md.isHardMode)
        {
            if (PlayerPrefs.GetInt(ConstData.hardMode_Unlock) != 0)
            {
                itemName_Text.text = md.mealName;
                lockImage_go.SetActive(false);
            }
            else
            {
                itemName_Text.text = "???";
                lockImage_go.SetActive(true);
            }
        }
        else
            itemName_Text.text = md.mealName;
    }

    public void OnClick()
    {
        PediaManager.instance.SetData(md);
    }
}
