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

    [SerializeField] PassivePanel passivePanel_Prefab;
    [SerializeField] Transform passive_Parent;

    [Header("0근접 1원거리")]
    [SerializeField] List<Sprite> attackRangeType_Sprites = new List<Sprite>();
    [Header("0중앙 1요정 2마왕 3묘지기")]
    [SerializeField] List<Sprite> faction_Sprites = new List<Sprite>();
    [Header("0투명 1물리 2마법 3화염")]
    [SerializeField] List<Sprite> attackType_Sprites = new List<Sprite>();


    public void SetDetail(Card_new selected_card)
    {
        unit = selected_card.unit;
        ItemData item = selected_card.item;

        attackType_Image.sprite = attackType_Sprites[(int)unit.ud.attack_Type];
        weakType_Image.sprite = attackType_Sprites[(int)unit.ud.weak_Type];
        resistType_Image.sprite = attackType_Sprites[(int)unit.ud.resistance_Type];
        unit_Image.sprite = unit.ud.unit_Sprite;
        attackRangeType_Image.sprite = attackRangeType_Sprites[(int)unit.ud.attack_RangeType];
        spawnCount_Text.text = $"X {unit.ud.spawn_Count}";
        level_Text.text = $"{unit.ud.level}";
        faction_Text.text = unit.ud.faction == Faction.Guild ? "중앙 왕국"
            : unit.ud.faction == Faction.Fairy ? "요정 숲"
            : unit.ud.faction == Faction.Demon ? "마왕군"
            : unit.ud.faction == Faction.Graveyard ? "묘지기" : "";

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
        size_Text.text = unit.ud.size == Unit_Size.Small ? "소형" :
            unit.ud.size == Unit_Size.Medium ? "중형"
            : unit.ud.size == Unit_Size.Large ? "대형" : "";

        detail_SpawnCount_Text.text = $"{unit.ud.spawn_Count}";

        //아이템에 따라 표기 변경
        switch (item?.ItemCode)
        {
            case 1:
            case 101:
                attack_Text.text = $"<color=green>{unit.ud.damage + item.itemValue * unit.ud.level}</color>";
                break;
            case 2:
            case 102:
                attackSpeed_Text.text = $"<color=green>{unit.ud.attack_Speed + item.itemValue}</color>";
                break;
            case 3:
            case 103:
                armor_Text.text = $"<color=green>{unit.ud.armor + item.itemValue}</color>";
                break;
            case 4:
            case 104:
                hp_Text.text = $"<color=green>{unit.ud.hp + item.itemValue}</color>";
                break;
            case 5:
            case 105:
                cost_Text.text = $"<color=green>{(int)(unit.ud.cost * (1 - item.itemValue * 0.01f))}</color>";
                break;
        }

        //원래 있던 패시브 설명창 삭제
        foreach (Transform child in passive_Parent)
            Destroy(child.gameObject);

        if (unit.ud.passive1 != "")
            MakeNewDetail(unit.ud.passive1, unit.ud.passive1_Detail);

        if (unit.ud.passive2 != "")
            MakeNewDetail(unit.ud.passive2, unit.ud.passive2_Detail);
    }
    
    public void MakeNewDetail(string passiveName, string passiveDetail)
    {
        PassivePanel newPd = Instantiate(passivePanel_Prefab, passive_Parent);
        newPd.SetNameText(passiveName);
        newPd.SetDetailText(passiveDetail);
    }
}
