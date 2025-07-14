using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUnitSpawnButton : MonoBehaviour
{
    public TestUnitSpawn us;
    public Unit spawnUnit;
    public Image unit_Image;
    
    Image img_component;

    void Start()
    {
        unit_Image.sprite = spawnUnit.ud.unit_Sprite;
        img_component = GetComponent<Image>();
    }

    private void Update()
    {
        if (us.selected_Unit != null && us.selected_Unit == spawnUnit)
            img_component.color = new Color(134 / 255f, 14 / 255f, 14 / 255f);
        else
            img_component.color = new Color(100 / 255f, 100 / 255f, 100 / 255f);
    }

    public void SelectUnit()
    {
        us.selected_Unit = spawnUnit;
    }
}
