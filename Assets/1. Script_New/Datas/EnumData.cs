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

public enum AttackRangeType
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
    Map_1 , Boss, WorldMap, Intro
}
public enum SFX_Enum 
{ 
    Touch , CardAppear, BaseUpgrade , UnitConfirm, UnitCoolDown , Deny , UnitEmploy , Hit_Physic , Hit_Magic, Hit_Fire 
        , Avoid , ShieldSmite, BrokenHeroSword , Dialogue2, HeroUpgrade, Eating, MealComplete , StartStage, Victory, BaseUpgrade_Fail
        , HeroDie, HeroRevive, Defeated, BossSpawned
}

public enum EMixer
{
    Master, BGM, SFX
}

public enum MealRarity
{
    Uncommon,Rare,Legendary
}

public enum SkillType
{
    Attack, Buff
}

public enum TutorialKey
{
    WorldMap_1, WorldMap_2, WorldMap_3, Dungeon_1, Dungeon_2,
}

public enum CutSceneKey
{
    StartScene_1
}

public enum SearchKey
{
    Dungeon1, Area1, GameStartButton, CardSelectButton, UnitSpawnButton1, BaseLevelUpButton, GameClearPanel, GoldPanel , Area2, FadeMask
}