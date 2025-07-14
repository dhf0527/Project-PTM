using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestItemButton : MonoBehaviour
{
    public TestUnitSpawn us;
    public ItemData id;
    public TMP_Text itemName_Text;

    Image img_component;

    private void Start()
    {
        img_component = GetComponent<Image>();
        itemName_Text.text = id.itemName;
    }

    private void Update()
    {
        if (us.selected_Id != null && us.selected_Id == id)
            img_component.color = new Color(134 / 255f, 14 / 255f, 14 / 255f);
        else
            img_component.color = new Color(100 / 255f, 100 / 255f, 100 / 255f);
    }

    public void OnItemClick()
    {
        us.selected_Id = id;
    }
}
