using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonPanel : MonoBehaviour
{
    public Image boss_Image;
    public Image[] unit_Images;
    public TMP_Text stage_Text;
    public Image star_Image;
    public TMP_Text record_Text;

    public void SetData(DungeonData dd)
    {
        boss_Image.sprite = dd.bossUnit.unit_Sprite;
        stage_Text.text = $"{dd.stage}-{dd.number}";

        for (int i = 0; i < 3; i++)
        {
            if (!dd.units[i])
            {
                unit_Images[i].sprite = null;
                continue;
            }

            unit_Images[i].sprite = dd.units[i].unit_Sprite;
        }
    }
}
