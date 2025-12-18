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

    [SerializeField] Image main_Image;
    [SerializeField] Image up_Image;
    [SerializeField] Image down_Image;
    [SerializeField] List<Image> panel_Images;
    [SerializeField] Image rarity_Panel;
    [SerializeField] Image rarity_Icon;
    [SerializeField] TMP_Text rarity_Text;

    [Header("0°í±Þ, 1Èñ±Í")]
    [SerializeField] List<Sprite> main_Sprites;
    [SerializeField] List<Sprite> side_Sprites;
    [SerializeField] List<Sprite> panel_Sprites;
    [SerializeField] List<Sprite> rarityPanel_Sprites;
    [SerializeField] List<Sprite> rarityIcon_Sprites;

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
        mealDsc = GetComponent<ReplaceWord>().ReplaceWordColor(mealDsc);
        mealDescription_Text.text = mealDsc;

        int rarityIndex = (int)Md.mealRarity;
        main_Image.sprite = main_Sprites[rarityIndex];
        up_Image.sprite = side_Sprites[rarityIndex];
        down_Image.sprite = side_Sprites[rarityIndex];

        if(rarity_Panel)
        {
            rarity_Panel.sprite = rarityPanel_Sprites[rarityIndex];
            rarity_Icon.sprite = rarityIcon_Sprites[rarityIndex];
            rarity_Text.text = rarityIndex == 0 ? "°í±Þ" : "Èñ±Í";
        }

        foreach (var item in panel_Images)
            item.sprite = panel_Sprites[rarityIndex];
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
