using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public void OnHitEnd()
    {
        FxManager.Instance.DisableHit(this);
    }
}
