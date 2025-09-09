using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailPanel : MonoBehaviour
{
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

    [Header("소속 별 Frame : 왕국, 숲, 마왕, 묘지기 순")]
    [SerializeField] List<Sprite> upDownFrameByFaction_Sprite;
    [SerializeField] List<Sprite> backGroundByFaction_Sprite;

    [Header("0근접 1원거리")]
    [SerializeField] List<Sprite> attackRangeType_Sprites = new List<Sprite>();
    [Header("0중앙 1요정 2마왕 3묘지기")]
    [SerializeField] List<Sprite> faction_Sprites = new List<Sprite>();
    [Header("0투명 1물리 2마법 3화염")]
    [SerializeField] List<Sprite> attackType_Sprites = new List<Sprite>();


    public void SetDetail(Unit unit)
    {
        UnitData ud = unit.ud;
        SetFrame(ud);

        attackType_Image.sprite = attackType_Sprites[(int)ud.attack_Type];
        weakType_Image.sprite = attackType_Sprites[(int)ud.weak_Type];
        resistType_Image.sprite = attackType_Sprites[(int)ud.resistance_Type];
        unit_Image.sprite = ud.unit_Sprite;
        attackRangeType_Image.sprite = attackRangeType_Sprites[(int)ud.attack_RangeType];
        spawnCount_Text.text = $"X {ud.spawn_Count}";
        level_Text.text = $"Lv.{ud.level}";
        faction_Text.text = ud.faction == Faction.Guild ? "중앙 왕국"
            : ud.faction == Faction.Fairy ? "요정 숲"
            : ud.faction == Faction.Demon ? "마왕군"
            : ud.faction == Faction.Graveyard ? "묘지기" : "";

        faction_Image.sprite = faction_Sprites[(int)ud.faction];
        name_Text.text = $"{ud.unit_Name}";
        SetTextColor(cost_Text, unit.Cost, ud.cost, false);
        SetTextColor(armor_Text, unit.Armor, ud.armor);
        SetTextColor(hp_Text, unit.Max_Hp, ud.hp);
        SetTextColor(attack_Text, unit.AttackDamage, ud.damage);
        SetTextColor(attackSpeed_Text, unit.AttackSpeed, ud.attack_Speed);
        targetCount_Text.text = $"{ud.target_Count}";
        SetTextColor(speed_Text, unit.MoveSpeed, ud.move_Speed);
        SetTextColor(accuracy_Text, unit.Accuracy, ud.accuracy);
        SetTextColor(avoidance_Text, unit.Avoidance, ud.avoidance);
        size_Text.text = ud.size == Unit_Size.Small ? "소형" :
            ud.size == Unit_Size.Medium ? "중형"
            : ud.size == Unit_Size.Large ? "대형" : "";
        SetTextColor(detail_SpawnCount_Text, unit.SpawnCount, ud.spawn_Count);

        //원래 있던 패시브 설명창 삭제
        foreach (Transform child in passive_Parent)
            Destroy(child.gameObject);

        if (ud.passive1 != "")
            MakeNewDetail(ud.passive1, ud.passive1_Detail);

        if (ud.passive2 != "")
            MakeNewDetail(ud.passive2, ud.passive2_Detail);
    }

    public void SetDetail(UnitData ud)
    {
        SetFrame(ud);

        attackType_Image.sprite = attackType_Sprites[(int)ud.attack_Type];
        weakType_Image.sprite = attackType_Sprites[(int)ud.weak_Type];
        resistType_Image.sprite = attackType_Sprites[(int)ud.resistance_Type];
        unit_Image.sprite = ud.unit_Sprite;
        attackRangeType_Image.sprite = attackRangeType_Sprites[(int)ud.attack_RangeType];
        spawnCount_Text.text = $"X {ud.spawn_Count}";
        level_Text.text = $"Lv.{ud.level}";
        faction_Text.text = ud.faction == Faction.Guild ? "중앙 왕국"
            : ud.faction == Faction.Fairy ? "요정 숲"
            : ud.faction == Faction.Demon ? "마왕군"
            : ud.faction == Faction.Graveyard ? "묘지기" : "";

        faction_Image.sprite = faction_Sprites[(int)ud.faction];
        name_Text.text = $"{ud.unit_Name}";

        cost_Text.text = ud.cost.ToString();
        armor_Text.text = ud.armor.ToString();
        hp_Text.text = ud.hp.ToString();
        attack_Text.text = ((int)ud.damage).ToString();
        attackSpeed_Text.text = ((int)ud.attack_Speed).ToString();
        targetCount_Text.text = $"{ud.target_Count}";
        speed_Text.text = ((int)ud.move_Speed).ToString();
        accuracy_Text.text = ud.accuracy.ToString();
        avoidance_Text.text = ud.avoidance.ToString();
        size_Text.text = ud.size == Unit_Size.Small ? "소형" :
            ud.size == Unit_Size.Medium ? "중형"
            : ud.size == Unit_Size.Large ? "대형" : "";
        detail_SpawnCount_Text.text = ud.spawn_Count.ToString();

        //원래 있던 패시브 설명창 삭제
        foreach (Transform child in passive_Parent)
            Destroy(child.gameObject);

        if (ud.passive1 != "")
            MakeNewDetail(ud.passive1, ud.passive1_Detail);

        if (ud.passive2 != "")
            MakeNewDetail(ud.passive2, ud.passive2_Detail);
    }

    void SetTextColor(TMP_Text text, float value, float originValue, bool isBiggerGood = true)
    {
        text.text = ((int)value).ToString();

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

    public void SetFrame(UnitData ud)
    {
        foreach (var item in upDownFrame_Image)
        {
            item.sprite = upDownFrameByFaction_Sprite[(int)ud.faction];
        }
        foreach (var item in backGroundFrame_Image)
        {
            item.sprite = backGroundByFaction_Sprite[(int)ud.faction];
        }
    }
}
