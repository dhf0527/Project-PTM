using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PediaManager : MonoBehaviour
{
    public static PediaManager instance;

    public List<GameObject> list_Tabs;
    enum E_Tabs
    {
        hero, unit, item, faction, story
    }

    public GameObject left_Scroll_go;
    public GameObject right_Scroll_go;

    #region ¿µ¿õ ÅÇ º¯¼ö
    public GameObject heroList_go;
    public HeroData_Pedia heroData;

    public GameObject heroSkill_go;
    public GameObject heroDescription_go;

    public UnitDetail_Pedia unitDetail;
    #endregion

    #region À¯´Ö ÅÇ º¯¼ö
    public GameObject unitList_go;
    public UnitData_Pedia unitData;
    #endregion

    [HideInInspector] public UnitData hero_ud;
    [HideInInspector] public UnitData unit_ud;

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
        heroSkill_go.SetActive(false);
        heroDescription_go.SetActive(false);
        unitList_go.SetActive(false);
        unitData.gameObject.SetActive(false);
    }

    public void OnCloseButton()
    {
        gameObject.SetActive(false);
        Init();
    }

    #region ¿µ¿õ ÅÇ
    //¿µ¿õ ÅÇ ´­·¶À» ¶§ È£Ãâ
    public void OnHeroTab()
    {
        Init();

        left_Scroll_go.SetActive(true);
        heroList_go.SetActive(true);
    }

    //¿µ¿õ Å¬¸¯ÇßÀ» ¶§ heroCell¿¡¼­ È£Ãâ
    public void SetHeroData(UnitData ud)
    {
        hero_ud = ud;
        heroData.SetData(ud);
        unitDetail.SetData(ud, true);
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
        heroSkill_go.SetActive(isOpen);
        heroData.gameObject.SetActive(isOpen);
    }
    #endregion

    #region À¯´Ö ÅÇ
    //À¯´Ö ÅÇ ´­·¶À» ¶§ È£Ãâ
    public void OnUnitList()
    {
        Init();

        left_Scroll_go.SetActive(true);
        unitList_go.SetActive(true);
    }

    //¿µ¿õ Å¬¸¯ÇßÀ» ¶§ UnitCell¿¡¼­ È£Ãâ
    public void SetUnitData(UnitData ud)
    {
        unit_ud = ud;
        unitData.gameObject.SetActive(true);
        unitData.SetData(ud);
        unitDetail.SetData(ud, false);
    }

    //¿µ¿õ Å¬¸¯ÇßÀ» ¶§ UnitCell¿¡¼­ È£Ãâ
    public void OnUnitDescription()
    {
        
    }
    #endregion
}
