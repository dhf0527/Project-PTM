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

    [SerializeField] List<Image> upDownFrame_Image;
    [SerializeField] List<Image> backGroundFrame_Image;

    [Header("¼Ò¼Ó º° Frame : ¿Õ±¹, ½£, ¸¶¿Õ, ¹¦Áö±â ¼ø")]
    [SerializeField] List<Sprite> upDownFrameByFaction_Sprite;
    [SerializeField] List<Sprite> backGroundByFaction_Sprite;

    [Header("0±ÙÁ¢ 1¿ø°Å¸®")]
    [SerializeField] List<Sprite> attackRangeType_Sprites = new List<Sprite>();
    [Header("0Áß¾Ó 1¿äÁ¤ 2¸¶¿Õ 3¹¦Áö±â")]
    [SerializeField] List<Sprite> faction_Sprites = new List<Sprite>();
    [Header("0Åõ¸í 1¹°¸® 2¸¶¹ý 3È­¿°")]
    [SerializeField] List<Sprite> attackType_Sprites = new List<Sprite>();


    public void SetDetail(Card_new selected_card)
    {
        unit = selected_card.unit;
        ItemData item = selected_card.item;

        SetFrame();

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
        SetTextColor(cost_Text, unit.Cost, unit.ud.cost, false);
        SetTextColor(armor_Text, unit.Armor, unit.ud.armor);
        SetTextColor(hp_Text, unit.Max_Hp, unit.ud.hp);
        SetTextColor(attack_Text, unit.AttackDamage, unit.ud.damage);
        SetTextColor(attackSpeed_Text, unit.AttackSpeed, unit.ud.attack_Speed);
        targetCount_Text.text = $"{unit.ud.target_Count}";
        SetTextColor(speed_Text, unit.MoveSpeed, unit.ud.move_Speed);
        SetTextColor(accuracy_Text, unit.Accuracy, unit.ud.accuracy);
        SetTextColor(avoidance_Text, unit.Avoidance, unit.ud.avoidance);
        size_Text.text = unit.ud.size == Unit_Size.Small ? "¼ÒÇü" :
            unit.ud.size == Unit_Size.Medium ? "ÁßÇü"
            : unit.ud.size == Unit_Size.Large ? "´ëÇü" : "";
        SetTextColor(detail_SpawnCount_Text, unit.SpawnCount, unit.ud.spawn_Count);

        //¿ø·¡ ÀÖ´ø ÆÐ½Ãºê ¼³¸íÃ¢ »èÁ¦
        foreach (Transform child in passive_Parent)
            Destroy(child.gameObject);

        if (unit.ud.passive1 != "")
            MakeNewDetail(unit.ud.passive1, unit.ud.passive1_Detail);

        if (unit.ud.passive2 != "")
            MakeNewDetail(unit.ud.passive2, unit.ud.passive2_Detail);
    }

    void SetTextColor(TMP_Text text, float value, float originValue, bool isBiggerGood = true)
    {
        text.text = value.ToString();

        if (value == originValue)
        {
            text.color = Color.white;
            return;
        }

        if ((value > originValue) == isBiggerGood)
            text.color = Color.green;
        else
            text.color = Color.red;
    }


    public void MakeNewDetail(string passiveName, string passiveDetail)
    {
        PassivePanel newPd = Instantiate(passivePanel_Prefab, passive_Parent);
        newPd.SetNameText(passiveName);
        newPd.SetDetailText(passiveDetail);
    }

    public void SetFrame()
    {
        foreach (var item in upDownFrame_Image)
        {
            item.sprite = upDownFrameByFaction_Sprite[(int)unit.ud.faction];
        }
        foreach (var item in backGroundFrame_Image)
        {
            item.sprite = backGroundByFaction_Sprite[(int)unit.ud.faction];
        }
    }
}
