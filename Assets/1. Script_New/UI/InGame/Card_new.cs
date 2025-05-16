using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class Card_new : MonoBehaviour
{
    [HideInInspector] public Unit unit;
    [HideInInspector] public ItemData item;

    [SerializeField] Image attackRangeType_Image;
    [SerializeField] TMP_Text spawnCount_Text;
    [SerializeField] TMP_Text unitLevel_Text;
    [SerializeField] Image faction_Image;
    [SerializeField] Image unit_Image;
    [SerializeField] TMP_Text unitName_Text;
    [SerializeField] TMP_Text unitCost_Text;
    [SerializeField] TMP_Text unitArmor_Text;
    [SerializeField] TMP_Text unitHp_Text;
    [SerializeField] TMP_Text unitDamage_Text;
    [SerializeField] TMP_Text unitAttackSpeed_Text;
    [SerializeField] Image attackType_Image;
    [SerializeField] TMP_Text[] passive_Text;
    [SerializeField] List<GameObject> dark_Masks = new List<GameObject>();

    Animation anim;

    [SerializeField] GameObject itemParent_Go;
    [SerializeField] Image itemPanel_Image;
    [SerializeField] Image itemIcon_Image;
    [SerializeField] TMP_Text itemDescription_Text;

    [SerializeField] List<Image> upDownFrame_Image;
    [SerializeField] List<Image> backGroundFrame_Image;
    [SerializeField] List<Image> backGround2Frame_Image;
    [SerializeField] List<Image> backGround3Frame_Image;

    [Header("소속 별 Frame : 왕국, 숲, 마왕, 묘지기 순")]
    [SerializeField] List<Sprite> upDownFrameByFaction_Sprite;
    [SerializeField] List<Sprite> backGroundByFaction_Sprite;
    [SerializeField] List<Sprite> backGround2ByFaction_Sprite;
    [SerializeField] List<Sprite> backGround3ByFaction_Sprite;

    [Header("0근접 1원거리")]
    [SerializeField] List<Sprite> attackRangeType_Sprites = new List<Sprite>();
    [Header("0중앙 1요정 2마왕 3묘지기")]
    [SerializeField] List<Sprite> faction_Sprites = new List<Sprite>();
    [Header("0물리 1마법 2화염")]
    [SerializeField] List<Sprite> attackType_Sprites = new List<Sprite>();

    [HideInInspector] public Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        anim = GetComponent<Animation>();
    }

    public void SetData(Unit setUnit)
    {
        unit = setUnit;

        SetFrame();

        attackRangeType_Image.sprite = attackRangeType_Sprites[(int)unit.ud.attack_RangeType];
        spawnCount_Text.text = $"X {unit.ud.spawn_Count}";
        unitLevel_Text.text = $"LV.{unit.ud.level}";
        faction_Image.sprite = faction_Sprites[(int)unit.ud.faction];
        unit_Image.sprite = unit.ud.unit_Sprite;
        unitName_Text.text = $"{unit.ud.unit_Name}";
        unitCost_Text.text = $"{unit.ud.cost}";
        unitArmor_Text.text = $"{unit.ud.armor}";
        unitHp_Text.text = $"{unit.ud.hp}";
        unitDamage_Text.text = $"{unit.ud.damage}";
        unitAttackSpeed_Text.text = $"{unit.ud.attack_Speed}";
        //AttackType[0] = none이므로 제외하고 1부터
        attackType_Image.sprite = attackType_Sprites[(int)unit.ud.attack_Type - 1];

        //아이템에 따라 표기 변경
        switch (item?.ItemCode)
        {
            case 1:
            case 101:
                unitDamage_Text.text = $"<color=green>{unit.ud.damage + item.itemValue * unit.ud.level}</color>";
                break;
            case 2:
            case 102:
                unitAttackSpeed_Text.text = $"<color=green>{unit.ud.attack_Speed + item.itemValue}</color>";
                break;
            case 3:
            case 103:
                unitArmor_Text.text = $"<color=green>{unit.ud.armor + item.itemValue}</color>";
                break;
            case 4:
            case 104:
                unitHp_Text.text = $"<color=green>{unit.ud.hp + item.itemValue}</color>";
                break;
            case 5:
            case 105:
                unitCost_Text.text = $"<color=green>{(int)(unit.ud.cost * (1 - item.itemValue * 0.01f))}</color>";
                break;
        }

        //패시브
        passive_Text[0].transform.parent.gameObject.SetActive(false);
        passive_Text[1].transform.parent.gameObject.SetActive(false);

        if (unit.ud.passive1 != "")
        {
            passive_Text[0].transform.parent.gameObject.SetActive(true);
            passive_Text[0].text = unit.ud.passive1;  
        }
        if (unit.ud.passive2 != "")
        {
            passive_Text[1].transform.parent.gameObject.SetActive(true);
            passive_Text[1].text = unit.ud.passive2;
        }

        //아이템 표시
        if (item)
        {
            itemParent_Go.SetActive(true);
            itemPanel_Image.color = item.itemRarity == ItemRarity.Uncommon ? new Color(107 / 255f, 198 / 255f, 53 / 255f) : Color.white;
            itemIcon_Image.sprite = item.itemIcon;

            //{value}를 item.itemValue로 변환
            string fixed_Text = Regex.Replace(item.itemDescription, @"\{value\}", item.itemValue.ToString());
            itemDescription_Text.text = fixed_Text;
        }
        else
            itemParent_Go.SetActive(false);
    }

    //선택되지 않으면 어둡게 만드는 함수
    public void SetDarkMask(bool isAllMask = false)
    {
        if (!toggle)
            toggle = GetComponent<Toggle>();

        foreach (var item in dark_Masks)
        {
            //어둡게 만들기
            if (!isAllMask)
                item.SetActive(!toggle.isOn);
            else
                item.SetActive(false);
        }

        //선택된 카드 크기 키우기
        if (toggle.isOn)
        {
            transform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
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
        foreach (var item in backGround2Frame_Image)
        {
            item.sprite = backGround2ByFaction_Sprite[(int)unit.ud.faction];
        }
        foreach (var item in backGround3Frame_Image)
        {
            item.sprite = backGround3ByFaction_Sprite[(int)unit.ud.faction];
        }
    }
}
