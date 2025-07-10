using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitData_Pedia : MonoBehaviour
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
    public TMP_Text spawnCount_Text;

    public TMP_Text name_Text;
    public TMP_Text level_Text;
    public TMP_Text cost_Text;
    public TMP_Text armor_Text;
    public TMP_Text hp_Text;
    public TMP_Text damage_Text;
    public TMP_Text attackSpeed_Text;

    public TMP_Text[] passive_Text;

    public void SetData(UnitData ud)
    {
        attackRangeType_Image.sprite = attackRange_Sprites[(int)ud.attack_RangeType];
        faction_Image.sprite = faction_Sprites[(int)ud.faction];
        character_Image.sprite = ud.unit_Sprite;
        attackType_Image.sprite = attackType_Sprites[(int)ud.attack_Type - 1];
        spawnCount_Text.text = "X " + ud.spawn_Count.ToString();

        name_Text.text = ud.unit_Name;
        level_Text.text = "Lv." + ud.level.ToString();
        cost_Text.text = ud.cost.ToString();
        armor_Text.text = ud.armor.ToString();
        hp_Text.text = ud.hp.ToString();
        damage_Text.text = ud.damage.ToString();
        attackSpeed_Text.text = ud.attack_Speed.ToString();

        //패시브
        passive_Text[0].transform.parent.gameObject.SetActive(false);
        passive_Text[1].transform.parent.gameObject.SetActive(false);

        if (ud.passive1 != "")
        {
            passive_Text[0].transform.parent.gameObject.SetActive(true);
            passive_Text[0].text = ud.passive1;
        }
        if (ud.passive2 != "")
        {
            passive_Text[1].transform.parent.gameObject.SetActive(true);
            passive_Text[1].text = ud.passive2;
        }
    }
}
