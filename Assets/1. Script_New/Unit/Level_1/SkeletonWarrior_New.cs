using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarrior_New : Unit
{
    //부활 후 애니메이션
    [SerializeField] RuntimeAnimatorController reborn_anim;
    public float attackSpeed_Increase_Percent;
    public float moveSpeed_Increase_Percent;
    public float hp_Decrease_Per_Sec;
    bool isReborn;

    public override void Dead()
    {
        //첫 사망 시 부활
        if(!isReborn)
        {
            if (isAttacking)
                OnEndAttack();

            isReborn = true;
            //체력 회복
            Cur_Hp = Max_Hp;
            //애니메이션(스프라이트) 변경
            animator.runtimeAnimatorController = reborn_anim;

            //'부활 상태' 부여
            Reborn reborn = gameObject.AddComponent<Reborn>();
            reborn.attackSpeed_Increase_Percent = attackSpeed_Increase_Percent;
            reborn.moveSpeed_Increase_Percent = moveSpeed_Increase_Percent;
            reborn.hp_Decrease_Per_Sec = hp_Decrease_Per_Sec;
        }
        else
        {
            base.Dead();
        }
        
    }
}
