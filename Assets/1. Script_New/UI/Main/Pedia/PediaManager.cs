using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PediaManager : MonoBehaviour
{
    public static PediaManager instance;

    public List<Animator> list_Tabs_Animator;
    enum E_Tabs
    {
        hero, unit, item, faction, story
    }
    E_Tabs cur_Tab;
    E_Tabs Cur_Tab 
    { 
        get { return cur_Tab; }
        set 
        { 
            cur_Tab = value;
            SetTab();
        }
    }

    public GameObject left_Scroll_go;
    public GameObject right_Scroll_go;

    #region ¿µ¿õ ÅÇ º¯¼ö
    public GameObject heroList_go;
    public HeroData_Pedia heroData;

    public SkillDescription skillDescription;
    public GameObject heroDescription_go;

    public DetailPanel unitDetail;
    #endregion

    #region À¯´Ö ÅÇ º¯¼ö
    public GameObject unitList_go;
    public UnitData_Pedia unitData;
    #endregion

    #region ¾ÆÀÌÅÛ ÅÇ º¯¼ö
    public GameObject itemList_go;
    public ItemDescription itemDescription;
    public GameObject flavorText_go;
    #endregion

    #region ¼¼·Â ÅÇ º¯¼ö
    public GameObject factionList_go;
    public FactionDescription factionDescription;
    #endregion

    private void Awake()
    {
        instance = this;
    }

    //¸ðµç ÅÇ ²ô±â
    public void Init()
    {
        left_Scroll_go.SetActive(false);
        right_Scroll_go.SetActive(false);
        heroList_go.SetActive(false);
        heroData.gameObject.SetActive(false);
        skillDescription.gameObject.SetActive(false);
        heroDescription_go.SetActive(false);
        unitList_go.SetActive(false);
        unitData.gameObject.SetActive(false);
        itemList_go.SetActive(false);
        itemDescription.gameObject.SetActive(false);
        factionList_go.SetActive(false);
        factionDescription.gameObject.SetActive(false);
    }

    public void OnCloseButton()
    {
        gameObject.SetActive(false);
        Init();

        foreach (var item in list_Tabs_Animator)
            item.SetBool("IsAnim", false);
    }

    public void OnTabClick(int index)
    {
        Cur_Tab = (E_Tabs)index;
    }

    void SetTab()
    {
        for (int i = 0; i < list_Tabs_Animator.Count; i++)
        {
            if (i == (int)cur_Tab)
                list_Tabs_Animator[i].SetBool("IsAnim", true);
            else
                list_Tabs_Animator[i].SetBool("IsAnim", false);
        }
    }

    #region ¿µ¿õ ÅÇ
    //¿µ¿õ ÅÇ ´­·¶À» ¶§ È£Ãâ
    public void OnHeroTab()
    {
        Init();
        Cur_Tab = E_Tabs.hero;

        left_Scroll_go.SetActive(true);
        heroList_go.SetActive(true);
    }

    //¿µ¿õ Å¬¸¯ÇßÀ» ¶§ heroCell¿¡¼­ È£Ãâ
    public void SetHeroData(UnitData ud)
    {
        heroData.SetData(ud);
        unitDetail.SetDetail(ud);
    }

    //¿µ¿õ Å¬¸¯ÇßÀ» ¶§ heroCell¿¡¼­ È£Ãâ
    public void OnHeroDescription()
    {
        heroDescription_go.SetActive(true);
    }

    //´É·ÂÄ¡ È®ÀÎ Å¬¸¯ÇßÀ» ¶§ È£Ãâ
    public void OnHeroData(bool isOpen)
    {
        left_Scroll_go.SetActive(!isOpen);
        heroDescription_go.SetActive(!isOpen);

        right_Scroll_go.SetActive(isOpen);
        skillDescription.gameObject.SetActive(isOpen);
        heroData.gameObject.SetActive(isOpen);
    }
    #endregion

    #region À¯´Ö ÅÇ
    //À¯´Ö ÅÇ ´­·¶À» ¶§ È£Ãâ
    public void OnUnitList()
    {
        Init();
        Cur_Tab = E_Tabs.unit;

        left_Scroll_go.SetActive(true);
        unitList_go.SetActive(true);
    }

    //À¯´Ö Å¬¸¯ÇßÀ» ¶§ UnitCell¿¡¼­ È£Ãâ
    public void SetUnitData(UnitData ud)
    {
        unitData.gameObject.SetActive(true);
        unitData.SetData(ud);
        unitDetail.SetDetail(ud);
    }
    #endregion

    #region ¾ÆÀÌÅÛ ÅÇ
    //¾ÆÀÌÅÛ ÅÇ ´­·¶À» ¶§ È£Ãâ
    public void OnItemList()
    {
        Init();
        Cur_Tab = E_Tabs.item;

        left_Scroll_go.SetActive(true);
        itemList_go.SetActive(true);
    }

    //¾ÆÀÌÅÛ Å¬¸¯ÇßÀ» ¶§ itemCell¿¡¼­ È£Ãâ
    public void SetData(ItemData id)
    {
        itemDescription.gameObject.SetActive(true);
        itemDescription.SetData(id);
        flavorText_go.SetActive(false);
    }

    public void SetData(MealData md)
    {
        itemDescription.gameObject.SetActive(true);
        itemDescription.SetData(md);
        flavorText_go.SetActive(true);
    }
    #endregion

    #region ¼¼·Â ÅÇ
    //À¯´Ö ÅÇ ´­·¶À» ¶§ È£Ãâ
    public void OnFactionTab()
    {
        Init();
        Cur_Tab = E_Tabs.faction;

        left_Scroll_go.SetActive(true);
        factionList_go.SetActive(true);
    }

    public void SetFactionData(FactionData fd)
    {
        factionDescription.gameObject.SetActive(true);
        factionDescription.SetData(fd);
    }
    #endregion
}
