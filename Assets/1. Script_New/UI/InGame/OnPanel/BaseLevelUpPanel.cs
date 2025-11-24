using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseLevelUpPanel : MonoBehaviour
{
    public GameObject mask;
    public Animator levelUpWave_Anim;
    [SerializeField] TMP_Text level_Text;
    [SerializeField] TMP_Text cost_Text;
    [HideInInspector] public Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Set_LevelText(int level)
    {
        level_Text.text = $"Level {level}";
        /*
        if (level != 1)
            Anim_LevelUp();
        */
    }
    public void Set_CostText(int cost)
    {
        cost_Text.text = $"{cost}";
    }

    public void Set_CostText(string message)
    {
        cost_Text.text = message;
    } 

    public void Anim_LevelUp()
    {
        //anim.SetTrigger("levelUp");
    }
}
