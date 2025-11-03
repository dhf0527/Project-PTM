using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownSword : MonoBehaviour
{
    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void Sound_CrownSword()
    {
        AudioManager.Instance.PlayerSfx(SFX_Enum.BrokenHeroSword);
    }
}
