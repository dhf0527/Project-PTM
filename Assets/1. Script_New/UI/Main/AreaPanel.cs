using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class AreaPanel : MonoBehaviour
{
    public Image boss_Image;
    public TMP_Text stage_Text;
    public Image star_Image;
    public List<Sprite> star_Sprites;

    public void SetData(DungeonData dd)
    {
        stage_Text.text = $"{dd.stage}-{dd.number}";
        boss_Image.sprite = dd.bossUnit.ud.unit_Sprite;

        int clear_Time = PlayerPrefs.GetInt(ConstData.dungeonClearTime + $"{dd.stage},{dd.number}");
        int clear_Rank = clear_Time == 0 ? 0 : clear_Time < 300 ? 3 : clear_Time < 480 ? 2 : 1;
        
        if(clear_Rank == 0)
            star_Image.gameObject.SetActive(false);
        else
        { 
            star_Image.gameObject.SetActive(true);
            star_Image.sprite = star_Sprites[clear_Rank - 1];
        }
    }
}
