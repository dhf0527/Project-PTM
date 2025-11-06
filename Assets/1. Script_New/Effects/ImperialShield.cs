using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImperialShield : MonoBehaviour
{
    [SerializeField] GameObject shield_Particle_prf;
    [SerializeField] Transform shieldSprite_Trans;
    [SerializeField] float particle_period;
    float cur_Time = 0;
    int count = 0;

    private void Update()
    {
        cur_Time+= Time.deltaTime;
        if(cur_Time >= particle_period)
        {
            float upValue = count % 2 == 0 ? 0.13f : -0.13f;
            count++;

            cur_Time = 0;
            Instantiate(shield_Particle_prf, transform.parent).transform.position = shieldSprite_Trans.position + Vector3.up * upValue;
        }
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void Sound_CrownSword()
    {
        AudioManager.Instance.PlayerSfx(SFX_Enum.BrokenHeroSword);
    }
}
