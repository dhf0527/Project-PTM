using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Unit_Size
{
    Small,
    Medium,
    Large
}

public enum AttackType
{
    None,
    Physical,
    Magical,
    Fire
}

public enum AttackRange
{
    Melee,
    Ranged
}

public enum Faction
{
    Guild,
    Fairy,
    Demon,
    Graveyard
}

public enum ItemRarity
{
    Uncommon,
    Rare
}

public enum BGM_Enum 
{ 
    Map_1 , Boss, WorldMap
}
public enum SFX_Enum 
{ 
    Touch , CardAppear, TowerUpGrade , UnitConfirm, UnitCoolDown , Deny , UnitEmploy , Hit_Physic , Hit_Magic, Hit_Fire 
        , Avoid , ShieldSmite, BrokenHeroSword , Dialogue2, HeroUpgrade, Eating, MealComplete , StartStage
}

public enum EMixer
{
    Master, BGM, SFX
}

public enum MealRarity
{
    Uncommon,
    Rare
}