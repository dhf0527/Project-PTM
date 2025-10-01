using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDescription : MonoBehaviour
{
    [SerializeField] SkillDataDetail skillDataDetail_Prf;
    [SerializeField] Transform content_Trans;

    public void SetDescription(Hero hero)
    {
        foreach (Transform item in content_Trans)
        {
            Destroy(item.gameObject);
        }

        for (int i = hero.skillDatas.Count; i > 0; i--)
        {
            Instantiate(skillDataDetail_Prf, content_Trans);
            skillDataDetail_Prf.SetSkillDataDetail(hero.skillDatas[i - 1], i);
        }
    }
}
