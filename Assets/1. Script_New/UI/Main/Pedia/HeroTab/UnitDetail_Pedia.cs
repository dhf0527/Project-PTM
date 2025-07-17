using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitDetail_Pedia : MonoBehaviour
{
    [Header("근, 원")]
    public List<Sprite> attackRange_Sprites;
    [Header("중앙, 요정, 마왕, 묘지기")]
    public List<Sprite> faction_Sprites;
    [Header("물리, 마법, 불")]
    public List<Sprite> attackType_Sprites;

    public Image attackRangeType_Image;
    public Image faction_Image;
    public Image character_Image;
    public Image attackType_Image;
    public Image weakType_Image;
    public Image resistType_Image;

    public TMP_Text level_Text;
    public GameObject cost_Image_go;
    public TMP_Text cost_Text;
    public TMP_Text subName_Text;

    public TMP_Text name_Text;
    public TMP_Text armor_Text;
    public TMP_Text hp_Text;
    public TMP_Text damage_Text;
    public TMP_Text attackSpeed_Text;
    public TMP_Text unitSize_Text;
    public TMP_Text targetCount_Text;
    public TMP_Text accuracy_Text;
    public TMP_Text avoidance_Text;
    public TMP_Text moveSpeed_Text;
    public TMP_Text spawnCount_Text;

    public PassivePanel passivePanel_Prefab;
    public Transform passive_Parent;

    public void SetData(UnitData ud, bool isHero)
    {
        cost_Image_go.SetActive(!isHero);
        cost_Text.gameObject.SetActive(!isHero);
        subName_Text.gameObject.SetActive(isHero);
        if (isHero)
        {
            subName_Text.text = ud.unit_SubName;
            level_Text.text = "영웅";
        }
        else
        {
            cost_Image_go.SetActive(true);
            cost_Text.text = ud.cost.ToString();
            level_Text.text = "Lv." + ud.level.ToString();
        }

        attackRangeType_Image.sprite = attackRange_Sprites[(int)ud.attack_RangeType];
        faction_Image.sprite = faction_Sprites[(int)ud.faction];
        character_Image.sprite = ud.unit_Sprite;
        attackType_Image.sprite = attackType_Sprites[(int)ud.attack_Type - 1];

        weakType_Image.gameObject.SetActive(ud.weak_Type != AttackType.None);
        weakType_Image.sprite = ud.weak_Type == AttackType.None ? null :
            attackType_Sprites[(int)ud.weak_Type - 1];

        resistType_Image.gameObject.SetActive(ud.resistance_Type != AttackType.None);
        resistType_Image.sprite = ud.resistance_Type == AttackType.None ? null :
            attackType_Sprites[(int)ud.resistance_Type - 1];

        name_Text.text = ud.unit_Name;
        armor_Text.text = ud.armor.ToString();
        hp_Text.text = ud.hp.ToString();
        damage_Text.text = ud.damage.ToString();
        attackSpeed_Text.text = ud.attack_Speed.ToString();
        unitSize_Text.text = ud.size == Unit_Size.Small ? "소형" :
            ud.size == Unit_Size.Medium ? "중형" :
            "대형";
        targetCount_Text.text = ud.target_Count.ToString();
        accuracy_Text.text = ud.accuracy.ToString();
        avoidance_Text.text = ud.avoidance.ToString();
        moveSpeed_Text.text = ud.move_Speed.ToString();

        spawnCount_Text.transform.parent.gameObject.SetActive(!isHero);
        spawnCount_Text.text = ud.spawn_Count.ToString();

        //원래 있던 패시브 설명창 삭제
        foreach (Transform child in passive_Parent)
            Destroy(child.gameObject);

        if (ud.passive1 != "")
            MakeNewDetail(ud.passive1, ud.passive1_Detail);

        if (ud.passive2 != "")
            MakeNewDetail(ud.passive2, ud.passive2_Detail);
    }

    public void MakeNewDetail(string passiveName, string passiveDetail)
    {
        PassivePanel newPd = Instantiate(passivePanel_Prefab, passive_Parent);
        newPd.SetNameText(passiveName);
        newPd.SetDetailText(passiveDetail);
    }

    public void OnSetBossData()
    {
        UnitData ud = GameManager.Instance.current_Dungeon.bossUnit.ud;
        int number = GameManager.Instance.current_Dungeon.number;

        cost_Image_go.SetActive(true);
        cost_Text.text = ud.cost.ToString();
        level_Text.text = "Lv." + ud.level.ToString();

        attackRangeType_Image.sprite = attackRange_Sprites[(int)ud.attack_RangeType];
        faction_Image.sprite = faction_Sprites[(int)ud.faction];
        character_Image.sprite = ud.unit_Sprite;
        attackType_Image.sprite = attackType_Sprites[(int)ud.attack_Type - 1];

        weakType_Image.gameObject.SetActive(ud.weak_Type != AttackType.None);
        weakType_Image.sprite = ud.weak_Type == AttackType.None ? null :
            attackType_Sprites[(int)ud.weak_Type - 1];

        resistType_Image.gameObject.SetActive(ud.resistance_Type != AttackType.None);
        resistType_Image.sprite = ud.resistance_Type == AttackType.None ? null :
            attackType_Sprites[(int)ud.resistance_Type - 1];

        name_Text.text = ud.unit_Name + " 보스";
        armor_Text.text = ud.armor.ToString();
        hp_Text.text = (ud.hp * (2 + (number * 0.5f))).ToString();
        damage_Text.text = (ud.damage * (1 + (number * 0.25f))).ToString();
        attackSpeed_Text.text = ud.attack_Speed.ToString();
        unitSize_Text.text = ud.size == Unit_Size.Small ? "중형" : "대형";

        targetCount_Text.text = (ud.target_Count * 2).ToString();
        accuracy_Text.text = ud.accuracy.ToString();
        avoidance_Text.text = ud.avoidance.ToString();
        moveSpeed_Text.text = ud.move_Speed.ToString();

        spawnCount_Text.text = "-";

        //원래 있던 패시브 설명창 삭제
        foreach (Transform child in passive_Parent)
            Destroy(child.gameObject);

        if (ud.passive1 != "")
            MakeNewDetail(ud.passive1, ud.passive1_Detail);

        if (ud.passive2 != "")
            MakeNewDetail(ud.passive2, ud.passive2_Detail);
    }
}
