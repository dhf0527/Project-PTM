using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseLevelUpPanel : MonoBehaviour
{
    [SerializeField] TMP_Text level_Text;
    [SerializeField] TMP_Text cost_Text;
    Animation anim;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public void Set_LevelText(int level)
    {
        level_Text.text = $"Level {level}";
        if (level != 1)
            Anim_LevelUp();
    }
    public void Set_CostText(int cost)
    {
        cost_Text.text = $"{cost}";
    }

    public void Anim_LevelUp()
    {
        anim.Play();
    }
}
