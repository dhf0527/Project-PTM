using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    [SerializeField] Toggle itemToggle;
    [SerializeField] RectTransform toggles_rectTrans;
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

        Vector2 tmp_vec = GetComponent<RectTransform>().sizeDelta;
        tmp_vec.y = itemToggle.isOn ? toggles_rectTrans.sizeDelta.y + itemList.GetComponent<RectTransform>().sizeDelta.y :
                    toggles_rectTrans.sizeDelta.y + mealList.GetComponent<RectTransform>().sizeDelta.y;
        GetComponent<RectTransform>().sizeDelta = tmp_vec;
    }
}
