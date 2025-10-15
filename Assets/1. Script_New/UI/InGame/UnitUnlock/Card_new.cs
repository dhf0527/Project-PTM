using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
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
    [SerializeField] GameObject passive_Icon_go;
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

        unit.unitStatData_st = new();

        #region 아이템 효과 적용
        //아이템 효과 적용
        switch (item?.ItemCode)
        {
            case 0:
            case 100:
                unit.unitStatData_st.spawnCoolDown_MinusPercent += item.itemValue;
                break;
            case 1:
            case 101:
                unit.unitStatData_st.attack_Plus += item.itemValue * unit.ud.level;
                break;
            case 2:
            case 102:
                unit.unitStatData_st.attackSpeed_Plus += item.itemValue;
                break;
            case 3:
            case 103:
                unit.unitStatData_st.armor_Plus = (int)item.itemValue;
                break;
            case 4:
            case 104:
                unit.unitStatData_st.max_Hp_Plus += item.itemValue;
                break;
            case 5:
            case 105:
                unit.unitStatData_st.cost_MinusPercent += item.itemValue;
                break;
        }
        #endregion

        #region 업그레이드 효과 적용
        //신입 모집
        if (unit.ud.level == 1)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 0);
            if (upgradeLv != 0)
                unit.unitStatData_st.cost_MinusPercent += DunGeonManager_New.instance.unitUpgradeDatas[0].upgradeValue[upgradeLv - 1];
        }
        //군사 훈련
        else if (unit.ud.level == 2)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 1);
            if (upgradeLv != 0)
                unit.unitStatData_st.attackSpeed_Plus += DunGeonManager_New.instance.unitUpgradeDatas[1].upgradeValue[upgradeLv - 1];
        }
        //성과 대우
        else if (unit.ud.level == 3)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 2);
            if (upgradeLv != 0)
                unit.unitStatData_st.spawnCoolDown_MinusPercent += DunGeonManager_New.instance.unitUpgradeDatas[2].upgradeValue[upgradeLv - 1];
        }
        //장갑 보강
        if (unit.ud.attack_RangeType == AttackRangeType.Melee)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 3);
            if (upgradeLv != 0)
                unit.unitStatData_st.max_Hp_Plus += DunGeonManager_New.instance.unitUpgradeDatas[3].upgradeValue[upgradeLv - 1] * 0.01f * unit.ud.hp;
        }
        //사격 훈련
        if (unit.ud.attack_RangeType == AttackRangeType.Ranged)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 4);
            if (upgradeLv != 0)
                unit.unitStatData_st.attack_PlusPercent += DunGeonManager_New.instance.unitUpgradeDatas[4].upgradeValue[upgradeLv - 1];
        }
        //개인 침낭
        if (unit.ud.size == Unit_Size.Small)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 5);
            if (upgradeLv != 0)
                unit.unitStatData_st.avoidance_Plus += (int)DunGeonManager_New.instance.unitUpgradeDatas[5].upgradeValue[upgradeLv - 1];
        }
        //대형 텐트
        if (unit.ud.size == Unit_Size.Medium || unit.ud.size == Unit_Size.Large)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 6);
            if (upgradeLv != 0)
                unit.unitStatData_st.moveSpeed_PlusPercent += DunGeonManager_New.instance.unitUpgradeDatas[6].upgradeValue[upgradeLv - 1];
        }
        #endregion

        #region 식사 효과 적용
        if (GameManager.Instance.current_Meal)
        {
            MealData md = GameManager.Instance.current_Meal;
            //유리비늘 생선구이
            if (md.code == 0 && unit.ud.armor == 0)
                unit.unitStatData_st.armor_Plus += (int)md.mealValue;
            //칠면조 바비큐
            else if (md.code == 3)
                unit.unitStatData_st.max_Hp_Plus += md.mealValue;
            //로즈베리 케이크
            else if (md.code == 104)
                unit.isTrueDamage = true;
            //불사조 닭발
            else if (md.code == 100)
                unit.unitStatData_st.attack_PlusPercent += md.mealValue2;
            //든든 국밥
            else if (md.code == 101)
            {
                unit.unitStatData_st.avoidance_Plus += (int)md.mealValue;
                unit.unitStatData_st.accuracy_Plus += (int)md.mealValue2;
            }
            //정체불명 햄버거
            else if (md.code == 102)
                unit.unitStatData_st.cost_MinusPercent += md.mealValue;
        }
        #endregion

        attackRangeType_Image.sprite = attackRangeType_Sprites[(int)unit.ud.attack_RangeType];
        spawnCount_Text.text = $"X {unit.SpawnCount}";
        unitLevel_Text.text = $"LV.{unit.ud.level}";
        faction_Image.sprite = faction_Sprites[(int)unit.ud.faction];
        unit_Image.sprite = unit.ud.unit_Sprite;
        unitName_Text.text = $"{unit.ud.unit_Name}";
        SetTextColor(unitCost_Text, unit.Cost, unit.ud.cost, false);
        SetTextColor(unitArmor_Text, unit.Armor, unit.ud.armor);
        SetTextColor(unitHp_Text, unit.Max_Hp, unit.ud.hp);
        SetTextColor(unitDamage_Text, unit.AttackDamage, unit.ud.damage);
        SetTextColor(unitAttackSpeed_Text, unit.AttackSpeed, unit.ud.attack_Speed);
        //AttackType[0] = none이므로 제외하고 1부터
        attackType_Image.sprite = attackType_Sprites[(int)unit.ud.attack_Type - 1];

        //패시브 아이콘
        passive_Icon_go.SetActive(unit.ud.passive1 != "" || unit.ud.passive2 != "");

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
