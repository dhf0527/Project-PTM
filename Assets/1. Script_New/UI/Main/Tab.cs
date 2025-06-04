using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab : MonoBehaviour
{
    public GameObject activeObj;

    public void SetTab(bool isOn)
    {
        activeObj.SetActive(isOn);
    }
}
