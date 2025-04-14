using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Object/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterCode;
    public string characterName;
    public Sprite characterSprite;
}
