using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDragonFire : Projectile
{
    [SerializeField] List<Sprite> sprites_By_Wave;
    SpriteRenderer projectile_spriteRenderer;
    private void Awake()
    {
        base.Awake();
        projectile_spriteRenderer = GetComponent<SpriteRenderer>();
        projectile_spriteRenderer.sprite = sprites_By_Wave[EnemySpawnManager.instance.cur_Wave];
    }
}
