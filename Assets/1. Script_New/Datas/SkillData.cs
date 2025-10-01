using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    public float skillCoolTime;
    [TextArea(3, 10)]
    public string skillDescription;
    public SkillType skillType;
}
