using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class SkeletonWarrior_New : Unit
{
    //부활 후 애니메이션
    [SerializeField] AnimatorController reborn_anim;
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
            Cur_Hp = unitData_st.max_Hp;
            //애니메이션(스프라이트) 변경
            animator.runtimeAnimatorController = reborn_anim;

            //'부활 상태' 부여
            gameObject.AddComponent<Reborn>();
        }
        else
        {
            base.Dead();
        }
        
    }
}
