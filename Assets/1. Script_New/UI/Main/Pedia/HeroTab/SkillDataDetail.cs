using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillDataDetail : MonoBehaviour
{
    [SerializeField] Image skill_Icon;
    [SerializeField] Image skill_TypeIcon;
    [SerializeField] TMP_Text skill_Name;
    [SerializeField] TMP_Text skill_Description;
    [SerializeField] TMP_Text skill_Order;
    [SerializeField] TMP_Text skill_TypeText;
    [SerializeField] TMP_Text skill_CoolTime;

    [Header("공격,변화 순서")]
    [SerializeField] List<Sprite> type_Sprites;

    public void SetSkillDataDetail(SkillData sd, int order)
    {
        skill_Icon.sprite = sd.skillIcon;
        skill_Name.text = sd.skillName;
        skill_Description.text = sd.skillDescription;
        skill_CoolTime.text = $"{sd.skillCoolTime}초";
        skill_Order.text = $"스킬 {order}";
        switch (sd.skillType)
        {
            case SkillType.Attack:
                skill_TypeText.text = "공격";
                skill_TypeIcon.sprite = type_Sprites[0];
                break;
            case SkillType.Buff:
                skill_TypeText.text = "변화";
                skill_TypeIcon.sprite = type_Sprites[1];
                break;
            default:
                break;
        }
    }
}
