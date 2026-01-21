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
    [SerializeField] Image panel_Image;
    public TMP_Text itemName_Text;
   

    [Header("0°í±Þ, 1Èñ±Í, 2Àü¼³")]
    [SerializeField] List<Sprite> rarityPanel_Sprites;

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        item_Image.sprite = md.mealIcon;
        panel_Image.sprite = rarityPanel_Sprites[(int)md.mealRarity];

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
