using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonPanel : MonoBehaviour
{
    public Image boss_Image;
    public Image[] unit_Images;
    public TMP_Text stage_Text;
    public Image star_Image;
    public List<Sprite> star_Sprites;
    public TMP_Text record_Text;

    public void SetData(DungeonData dd)
    {
        boss_Image.sprite = dd.bossUnit.ud.unit_Sprite;
        stage_Text.text = $"{dd.stage}-{dd.number}";

        List<Unit> stageUnits = new();

        foreach (var item in dd.units_Wave1)
            if (item)
                stageUnits.Add(item);
        foreach (var item in dd.units_Wave2)
            if (item)
                stageUnits.Add(item);
        foreach (var item in dd.units_Wave3)
            if (item)
                stageUnits.Add(item);

        //중복 제거
        stageUnits = stageUnits.Distinct().ToList();

        for (int i = 0; i < 3; i++)
        {
            if (i >= stageUnits.Count)
            {
                unit_Images[i].gameObject.SetActive(false);
                continue;
            }
            unit_Images[i].gameObject.SetActive(true);
            unit_Images[i].sprite = stageUnits[i].ud.unit_Sprite;
        }

        //별 이미지 설정
        stage_Text.text = $"{dd.stage}-{dd.number}";
        boss_Image.sprite = dd.bossUnit.ud.unit_Sprite;

        int clear_Time = PlayerPrefs.GetInt(ReadOnlyData.dungeonClearTime + $"{dd.stage},{dd.number}");
        int clear_Rank = clear_Time == 0 ? 0 : clear_Time < 200 ? 3 : clear_Time < 300 ? 2 : 1;

        if (clear_Rank == 0)
        {
            star_Image.gameObject.SetActive(false);
            record_Text.text = "---";
        }
        else
        {
            star_Image.gameObject.SetActive(true);
            star_Image.sprite = star_Sprites[clear_Rank - 1];
            record_Text.text = $"{clear_Time}sec";
        }

        GameManager.Instance.current_Dungeon = dd;
    }
}
