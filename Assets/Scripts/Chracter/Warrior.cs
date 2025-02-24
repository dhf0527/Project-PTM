using System.Collections.Generic;
using UnityEngine;


namespace Chracter
{
    
    public class Warrior : BaseCharacter
    {
        
        [SerializeField]
        List<RaycastHit2D> enemys = new List<RaycastHit2D>();
    
        // Start is called before the first frame update
        void Start()
        {
        }


        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(100, 10, 2, 2.5f, AttackRangeMeleeDefault, true, true, MoveDefault, 60, 40);
            healthBar.SetHealth(MaxHealth, MaxHealth);

        }
    }
}
