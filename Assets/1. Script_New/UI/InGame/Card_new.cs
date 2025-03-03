using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_new : MonoBehaviour
{
    [HideInInspector] public Unit unit;

    [SerializeField] Image attackRangeType_Image;
    [SerializeField] TMP_Text spawnCount_Text;
    [SerializeField] TMP_Text unitLevel_Text;
    [SerializeField] Image faction_Image;
    [SerializeField] Image unit_Image;
    [SerializeField] TMP_Text unitName_Text;
    [SerializeField] TMP_Text unitCost_Text;
    [SerializeField] TMP_Text unitArmor_Text;
    [SerializeField] TMP_Text unitHp_Text;
    [SerializeField] TMP_Text unitAttack_Text;
    [SerializeField] TMP_Text unitAttackSpeed_Text;
    [SerializeField] Image attackType_Image;
    [SerializeField] GameObject passive_Gameobject;
    [SerializeField] List<GameObject> dark_Masks = new List<GameObject>();

    [Header("0���� 1���Ÿ�")]
    [SerializeField] List<Sprite> attackRangeType_Sprites = new List<Sprite>();
    [Header("0�߾� 1���� 2���� 3������")]
    [SerializeField] List<Sprite> faction_Sprites = new List<Sprite>();
    [Header("0���� 1���� 2ȭ��")]
    [SerializeField] List<Sprite> attackType_Sprites = new List<Sprite>();

    [HideInInspector] public Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    public void SetData(Unit setUnit)
    {
        unit = setUnit;

        attackRangeType_Image.sprite = attackRangeType_Sprites[(int)unit.ud.attack_RangeType];
        spawnCount_Text.text = $"X {unit.ud.spawn_Count}";
        unitLevel_Text.text = $"{unit.ud.level}";
        faction_Image.sprite = faction_Sprites[(int)unit.ud.faction];
        unit_Image.sprite = unit.ud.unit_Sprite;
        unitName_Text.text = $"{unit.ud.unit_Name}";
        unitCost_Text.text = $"{unit.ud.cost}";
        unitArmor_Text.text = $"{unit.ud.armor}";
        unitHp_Text.text = $"{unit.ud.hp}";
        unitAttack_Text.text = $"{unit.ud.damage}";
        unitAttackSpeed_Text.text = $"{unit.ud.attack_Speed}";
        //AttackType[0] = none�̹Ƿ� �����ϰ� 1����
        attackType_Image.sprite = attackType_Sprites[(int)unit.ud.attack_Type - 1];

        passive_Gameobject.SetActive(unit.isHavePassive);
    }

    //���õ��� ������ ��Ӱ� ����� �Լ�
    public void SetDarkMask()
    {
        foreach (var item in dark_Masks)
        {
            item.SetActive(!toggle.isOn);
        }
    }
}
