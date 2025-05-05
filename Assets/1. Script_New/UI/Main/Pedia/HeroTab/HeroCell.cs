using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCell : MonoBehaviour
{
    public UnitData ud;

    public void OnClick()
    {
        PediaManager.instance.SetHeroData(ud);
        PediaManager.instance.OnHeroDescription();
    }
}
