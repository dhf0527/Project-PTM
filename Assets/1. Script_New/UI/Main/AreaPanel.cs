using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AreaPanel : MonoBehaviour
{
    public Image boss_Image;
    public TMP_Text stage_Text;
    public Image star_Image;

    public void SetData(DungeonData dd)
    {
        stage_Text.text = $"{dd.stage}-{dd.number}";
        boss_Image.sprite = dd.bossUnit.unit_Sprite;
    }
}
