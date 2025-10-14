using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    [SerializeField] Toggle itemToggle;
    [SerializeField] GameObject itemList;
    [SerializeField] GameObject mealList;

    [SerializeField] GameObject itemMask;
    [SerializeField] GameObject mealMask;

    public void SetActiveByToggle(bool isOn)
    {
        if (!isOn)
            return;

        itemList.SetActive(itemToggle.isOn);
        mealList.SetActive(!itemToggle.isOn);

        itemMask.SetActive(!itemToggle.isOn);
        mealMask.SetActive(itemToggle.isOn);
    }
}
