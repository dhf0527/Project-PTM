using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaTop : MonoBehaviour
{
    public bool isDisable = false;
    private void OnDisable()
    {
        if (isDisable)
        {
            Destroy(gameObject);
        }
    }
}
