using System;
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

    public List<Unit> stageUnits = new();
    Unit bossUnit;
    public DetailPanel detail_Panel;

    #region 던전 별 변경 사항
    [Header("던전 별 변경")]
    [SerializeField] Image top_Image;
    [SerializeField] List<Image> side1_Images;
    [SerializeField] List<Image> side2_Images;
    [SerializeField] TMP_Text title_Text;
    [SerializeField] List<Image> sideScroll_Images;
    [SerializeField] Image panel_Image;

    [Header("길드, 요정, 마계, 묘지")]
    [SerializeField] List<Sprite> top_Sprites;
    [SerializeField] List<Sprite> side1_Sprites;
    [SerializeField] List<Sprite> side2_Sprites;

    [Header("번호 별 변경")]
    [SerializeField] List<Sprite> sideScroll_Sprites;
    [SerializeField] List<Sprite> panel_Sprites;
    
    #endregion

    public void SetData(DungeonData dd)
    {
        boss_Image.sprite = dd.bossUnit.ud.unit_Sprite;
        stage_Text.text = $"{dd.stage}-{dd.number}";

        stageUnits.Clear();

        foreach (var item in dd.units_Wave1)
            if (item)
                stageUnits.Add(item);
        foreach (var item in dd.units_Wave2)
            if (item)
                stageUnits.Add(item);
        foreach (var item in dd.units_Wave3)
            if (item)
                stageUnits.Add(item);
        bossUnit = dd.bossUnit;

        //중복 제거
        stageUnits = stageUnits.Distinct().ToList();
        //유닛코드 오름차순으로 정렬
        stageUnits.Sort((Unit a, Unit b) => { return a.ud.unit_Code > b.ud.unit_Code ? 1 : 0; });

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

        stage_Text.text = $"{dd.stage}-{dd.number}";
        boss_Image.sprite = bossUnit.ud.unit_Sprite;

        //별 이미지 설정
        int clear_Time = PlayerPrefs.GetInt(ConstData.dungeonClearTime + $"{dd.stage},{dd.number}");
        int clear_Rank = clear_Time == 0 ? 0 : clear_Time < 300 ? 3 : clear_Time < 480 ? 2 : 1;

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

        SetDungeonPanelSprite(dd.stage_Faction);
        SetDungeonPanelSpriteByNumber(dd.number);
        GameManager.Instance.current_Dungeon = dd;
    }

    public void OnSetStageUnitData(int index)
    {
        detail_Panel.SetDetail(stageUnits[index].ud);
    }

    public void OnSetStageBossUnitData()
    {
        detail_Panel.SetDetail(bossUnit.ud);
    }

    void SetDungeonPanelSprite(Faction factionIndex)
    {
        top_Image.sprite = top_Sprites[(int)factionIndex];
        foreach (var item in side1_Images)
            item.sprite = side1_Sprites[(int)factionIndex];
        foreach (var item in side2_Images)
            item.sprite = side2_Sprites[(int)factionIndex];

        switch (factionIndex)
        {
            case Faction.Guild:
                title_Text.text = "중앙 왕국";
                break;
            case Faction.Fairy:
                title_Text.text = "요정 숲";
                break;
            case Faction.Demon:
                title_Text.text = "마계";
                break;
            case Faction.Graveyard:
                title_Text.text = "버려진 묘지";
                break;
            default:
                Debug.Log("세력 오류");
                break;
        }
    }

    void SetDungeonPanelSpriteByNumber(int number)
    {
        foreach (var item in sideScroll_Images)
            item.sprite = sideScroll_Sprites[number - 1];
        panel_Image.sprite = panel_Sprites[number - 1];
    }
}
