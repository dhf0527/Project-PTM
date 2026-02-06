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
    Dungeon , Boss, WorldMap, Intro, HardDungeon, HardBoss
}
public enum SFX_Enum 
{ 
    Touch , CardAppear, BaseUpgrade , UnitConfirm, UnitCoolDown , Deny , UnitEmploy , Hit_Physic , Hit_Magic, Hit_Fire 
        , Avoid , ShieldSmite, BrokenHeroSword , Dialogue2, HeroUpgrade, Eating, MealComplete , StartStage, Victory, BaseUpgrade_Fail
        , HeroDie, HeroRevive, Defeated, BossSpawned, MealReset, StarBuy, NextPage
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
   None, WorldMap_1, WorldMap_2, WorldMap_3, Dungeon_1, Dungeon_2,
}

public enum CutSceneKey
{
    CartoonSceneStart, Dialogue_1_1, Dialogue_1_3, Dialogue_2_1, Dialogue_2_3, Dialogue_3_1, Dialogue_3_3, Dialogue_4_1, Dialogue_4_3
}

public enum SearchKey
{
    Dungeon1, Area1, GameStartButton, CardSelectButton, UnitSpawnButton1, BaseLevelUpButton, GameClearPanel, GoldPanel , Area2, FadeMask
}