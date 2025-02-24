using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType {
        Warrior,   //0
        Thief,     //1
        Archer,    //2
        Goblin,    //3
        Slime,     //4
        SkeletonWarrior,
        Bat,
        Wizard,    //5
        MagicWarrior, //6
        Minotaur,  //7
        Centaur,   //8
        Golem,     //9
        FireSkull,
        Paladin,   //10
        DeerWarrior, //11
        WoodGolem, //12
        Elementalist, //13
        Dragon,     //14
        Reaper,
        Tower,
        EnemyTower,
        Character
    }

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;

    public string itemDesc;
    public Sprite itemIcon;

    public string attackType;
    public float attackRange;
    public int member;
    public int defenseValue;
    public float healthValue;
    public int strengthValue;
    public float attackSpeedValue;
    public string Defense;
    public string Health;
    public string Strength;
    public string AttackSpeed;
    public string Attribute;
    public string weakAttribute;
    public string strongAttribute;
    public int attackCountLimitValue;
    public float realSpeed;
    public int speedValue;
    public int accuracyValue;
    public int avoidanceValue;
    public int size;
    public int createCountValue;

    [Header("# Level Data")]
    public int level;
    /*public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;*/

    /*[Header("# Character Obj")]
    public GameObject Character;*/

    [Header("# Cost Data")]
    public int cost;


    public string Skill;
    public string SkillIntro;
    public string SkillNext;
    public string SkillIntroNext;
}
