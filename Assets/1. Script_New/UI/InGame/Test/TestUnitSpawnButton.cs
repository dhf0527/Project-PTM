using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUnitSpawnButton : MonoBehaviour
{
    public Unit spawnUnit;

    public Image unit_Image;

    void Start()
    {
        unit_Image.sprite = spawnUnit.ud.unit_Sprite;
    }
}
