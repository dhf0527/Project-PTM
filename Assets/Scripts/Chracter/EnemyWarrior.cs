using System.Collections;
using System.Collections.Generic;
using Chracter;
using UnityEngine;

public class EnemyWarrior : BaseCharacter
{
    public ParticleSystem moveParticle;
    // Start is called before the first frame update
    void Start()
    {
        if(moveParticle != null)
        {
            moveParticle.Play();
        }

        CheckTeam();
        SetCharacterSettings( 500, 20, 10, 1, 0.5f);
    }
}
