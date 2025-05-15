using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PediaManager : MonoBehaviour
{
    public static PediaManager instance;

    public List<GameObject> list_Tabs;
    public enum E_Tabs
    {
        hero, unit, item, faction, story
    }

    public GameObject left_Scroll_go;
    public GameObject right_Scroll_go;

    #region øµøı ≈« ∫Øºˆ
    public GameObject heroList_go;
    public HeroData_Pedia heroData;

    public GameObject heroSkill_go;
    public GameObject heroDescription_go;

    public UnitDetail_Pedia unitDetail;
    #endregion

    #region ¿Ø¥÷ ≈« ∫Øºˆ
    public GameObject unitList_go;
    public UnitData_Pedia unitData;
    #endregion

    #region æ∆¿Ã≈€ ≈« ∫Øºˆ
    public GameObject itemList_go;
    public ItemDescription itemDescription;
    #endregion

    #region ºº∑¬ ≈« ∫Øºˆ
    public GameObject factionList_go;
    public FactionDescription factionDescription;
    #endregion

    private void Awake()
    {
        instance = this;
    }

    //∏µÁ ≈« ≤Ù±‚
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
        itemList_go.SetActive(false);
        itemDescription.gameObject.SetActive(false);
        factionList_go.SetActive(false);
        factionDescription.gameObject.SetActive(false);
    }

    public void OnCloseButton()
    {
        gameObject.SetActive(false);
        Init();
    }

    public void OnTabClick(int index)
    {
        for (int i = 0; i < list_Tabs.Count; i++)
        {
            if (i == index)
                list_Tabs[i].GetComponent<Animator>().SetBool("IsAnim", true);
            else
                list_Tabs[i].GetComponent<Animator>().SetBool("IsAnim", false);
        }
    }

    #region øµøı ≈«
    //øµøı ≈« ¥≠∑∂¿ª ∂ß »£√‚
    public void OnHeroTab()
    {
        Init();

        left_Scroll_go.SetActive(true);
        heroList_go.SetActive(true);
    }

    //øµøı ≈¨∏Ø«ﬂ¿ª ∂ß heroCellø°º≠ »£√‚
    public void SetHeroData(UnitData ud)
    {
        heroData.SetData(ud);
        unitDetail.SetData(ud, true);
    }

    //øµøı ≈¨∏Ø«ﬂ¿ª ∂ß heroCellø°º≠ »£√‚
    public void OnHeroDescription()
    {
        heroDescription_go.SetActive(true);
    }

    //¥…∑¬ƒ° »Æ¿Œ ≈¨∏Ø«ﬂ¿ª ∂ß »£√‚
    public void OnHeroData(bool isOpen)
    {
        left_Scroll_go.SetActive(!isOpen);
        heroDescription_go.SetActive(!isOpen);

        right_Scroll_go.SetActive(isOpen);
        heroSkill_go.SetActive(isOpen);
        heroData.gameObject.SetActive(isOpen);
    }
    #endregion

    #region ¿Ø¥÷ ≈«
    //¿Ø¥÷ ≈« ¥≠∑∂¿ª ∂ß »£√‚
    public void OnUnitList()
    {
        Init();

        left_Scroll_go.SetActive(true);
        unitList_go.SetActive(true);
    }

    //¿Ø¥÷ ≈¨∏Ø«ﬂ¿ª ∂ß UnitCellø°º≠ »£√‚
    public void SetUnitData(UnitData ud)
    {
        unitData.gameObject.SetActive(true);
        unitData.SetData(ud);
        unitDetail.SetData(ud, false);
    }
    #endregion

    #region æ∆¿Ã≈€ ≈«
    //æ∆¿Ã≈€ ≈« ¥≠∑∂¿ª ∂ß »£√‚
    public void OnItemList()
    {
        Init();

        left_Scroll_go.SetActive(true);
        itemList_go.SetActive(true);
    }

    //æ∆¿Ã≈€ ≈¨∏Ø«ﬂ¿ª ∂ß itemCellø°º≠ »£√‚
    public void SetItemData(ItemData id)
    {
        itemDescription.gameObject.SetActive(true);
        itemDescription.SetItemData(id);
    }
    #endregion

    #region ºº∑¬ ≈«
    //¿Ø¥÷ ≈« ¥≠∑∂¿ª ∂ß »£√‚
    public void OnFactionTab()
    {
        Init();

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
