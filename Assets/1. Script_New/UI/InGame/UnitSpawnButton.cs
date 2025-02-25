using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawnButton : MonoBehaviour
{
    [HideInInspector] public UnitData unit;

    [SerializeField] TMP_Text level_Text;
    [SerializeField] TMP_Text cost_Text;
    [SerializeField] Image unit_Image;
    [SerializeField] Image lock_Image;
    [SerializeField] Image coolDown_Image;

    public void SetUI()
    {
        level_Text.text = $"Lv.{unit.level}";
        cost_Text.text = $"{unit.cost}";
        unit_Image.sprite = unit.unit_Sprite;
        lock_Image.gameObject.SetActive(false);
        coolDown_Image.gameObject.SetActive(false);
    }
}
