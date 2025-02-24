using System.Collections;
using UnityEngine;

namespace Chracter
{
    public class Elementalist : BaseCharacter
    {

        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(160, 40, 0, 1.4f, AttackRangeRangedLong, false, false, MoveDefault, 120, 80);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }



        public override IEnumerator RangedAttack(RaycastHit2D hit)
        {

            yield return StartCoroutine(base.RangedAttack(hit));
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2f, LayerMask.GetMask(Team));

            BaseCharacter healCharacter = this;
            if (hitColliders.Length == 0)
            {
                this.GetComponent<BaseCharacter>().GainHealth(10);
                yield break;
            }
            foreach (var hitCollider in hitColliders)
            {
       

                float teamHealth =hitCollider.GetComponent<BaseCharacter>().CurrentHealth;

                if (healCharacter.CurrentHealth > teamHealth)
                {
                    healCharacter = hitCollider.GetComponent<BaseCharacter>();
                }
               
            }
            healCharacter.GainHealth(10);
       
        }
    }
}
