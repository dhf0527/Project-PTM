using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailPanel : MonoBehaviour
{
    Unit unit;

    [SerializeField] Image attackType_Image;
    [SerializeField] Image weakType_Image;
    [SerializeField] Image resistType_Image;
    [SerializeField] Image unit_Image;
    [SerializeField] Image attackRangeType_Image;
    [SerializeField] TMP_Text spawnCount_Text;
    [SerializeField] TMP_Text level_Text;
    [SerializeField] TMP_Text faction_Text;
    [SerializeField] Image faction_Image;
    [SerializeField] TMP_Text name_Text;
    [SerializeField] TMP_Text cost_Text;
    [SerializeField] TMP_Text armor_Text;
    [SerializeField] TMP_Text hp_Text;
    [SerializeField] TMP_Text attack_Text;
    [SerializeField] TMP_Text attackSpeed_Text;
    [SerializeField] TMP_Text targetCount_Text;
    [SerializeField] TMP_Text speed_Text;
    [SerializeField] TMP_Text accuracy_Text;
    [SerializeField] TMP_Text avoidance_Text;
    [SerializeField] TMP_Text size_Text;
    [SerializeField] TMP_Text detail_SpawnCount_Text;
    [SerializeField] GameObject[] passivePanels;
    [SerializeField] TMP_Text[] passiveName_Texts;
    [SerializeField] TMP_Text[] passiveDetail_Texts;

    [Header("0±ÙÁ¢ 1¿ø°Å¸®")]
    [SerializeField] List<Sprite> attackRangeType_Sprites = new List<Sprite>();
    [Header("0Áß¾Ó 1¿äÁ¤ 2¸¶¿Õ 3¹¦Áö±â")]
    [SerializeField] List<Sprite> faction_Sprites = new List<Sprite>();
    [Header("0Åõ¸í 1¹°¸® 2¸¶¹ý 3È­¿°")]
    [SerializeField] List<Sprite> attackType_Sprites = new List<Sprite>();


    public void SetDetail(Unit setUnit)
    {
        unit = setUnit;

        attackType_Image.sprite = attackType_Sprites[(int)unit.ud.attack_Type];
        weakType_Image.sprite = attackType_Sprites[(int)unit.ud.weak_Type];
        resistType_Image.sprite = attackType_Sprites[(int)unit.ud.resistance_Type];
        unit_Image.sprite = unit.ud.unit_Sprite;
        attackRangeType_Image.sprite = attackRangeType_Sprites[(int)unit.ud.attack_RangeType];
        spawnCount_Text.text = $"X {unit.ud.spawn_Count}";
        level_Text.text = $"{unit.ud.level}";
        faction_Text.text = unit.ud.faction == Faction.Guild ? "Áß¾Ó ¿Õ±¹"
            : unit.ud.faction == Faction.Fairy ? "¿äÁ¤ ½£"
            : unit.ud.faction == Faction.Demon ? "¸¶¿Õ±º"
            : unit.ud.faction == Faction.Graveyard ? "¹¦Áö±â" : "";

        faction_Image.sprite = faction_Sprites[(int)unit.ud.faction];
        name_Text.text = $"{unit.ud.unit_Name}";
        cost_Text.text = $"{unit.ud.cost}";
        armor_Text.text = $"{unit.ud.armor}";
        hp_Text.text = $"{unit.ud.hp}";
        attack_Text.text = $"{unit.ud.damage}";
        attackSpeed_Text.text = $"{unit.ud.attack_Speed}";
        targetCount_Text.text = $"{unit.ud.target_Count}";
        speed_Text.text = $"{unit.ud.move_Speed}";
        accuracy_Text.text = $"{unit.ud.accuracy}";
        avoidance_Text.text = $"{unit.ud.avoidance}";
        size_Text.text = unit.ud.size == Unit_Size.Small ? "¼ÒÇü":
            unit.ud.size == Unit_Size.Medium ? "ÁßÇü"
            : unit.ud.size == Unit_Size.Large ? "´ëÇü" : "";

        detail_SpawnCount_Text.text = $"{unit.ud.spawn_Count}";

        if (unit.ud.passive1 != "")
        {
            passivePanels[0].SetActive(true);
            passiveName_Texts[0].text = unit.ud.passive1;
            passiveDetail_Texts[0].text = unit.ud.passive1_Detail;
        }
        else
            passivePanels[0].SetActive(false);

        if (unit.ud.passive2 != "")
        {
            passivePanels[1].SetActive(true);
            passiveName_Texts[1].text = unit.ud.passive2;
            passiveDetail_Texts[1].text = unit.ud.passive2_Detail;
        }
        else
            passivePanels[1].SetActive(false);
    }

}
