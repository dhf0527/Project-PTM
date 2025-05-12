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

    #region 영웅 탭 변수
    public GameObject heroList_go;
    public HeroData_Pedia heroData;

    public GameObject heroSkill_go;
    public GameObject heroDescription_go;

    public UnitDetail_Pedia unitDetail;
    #endregion

    #region 유닛 탭 변수
    public GameObject unitList_go;
    public UnitData_Pedia unitData;
    #endregion

    #region 아이템 탭 변수
    public GameObject itemList_go;
    public ItemDescription itemDescription;
    #endregion

    private void Awake()
    {
        instance = this;
    }

    //모든 탭 끄기
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
    }

    public void OnCloseButton()
    {
        gameObject.SetActive(false);
        Init();
    }

    #region 영웅 탭
    //영웅 탭 눌렀을 때 호출
    public void OnHeroTab()
    {
        Init();

        left_Scroll_go.SetActive(true);
        heroList_go.SetActive(true);
    }

    //영웅 클릭했을 때 heroCell에서 호출
    public void SetHeroData(UnitData ud)
    {
        heroData.SetData(ud);
        unitDetail.SetData(ud, true);
    }

    //영웅 클릭했을 때 heroCell에서 호출
    public void OnHeroDescription()
    {
        heroDescription_go.SetActive(true);
    }

    //능력치 확인 클릭했을 때 호출
    public void OnHeroData(bool isOpen)
    {
        left_Scroll_go.SetActive(!isOpen);
        heroDescription_go.SetActive(!isOpen);

        right_Scroll_go.SetActive(isOpen);
        heroSkill_go.SetActive(isOpen);
        heroData.gameObject.SetActive(isOpen);
    }
    #endregion

    #region 유닛 탭
    //유닛 탭 눌렀을 때 호출
    public void OnUnitList()
    {
        Init();

        left_Scroll_go.SetActive(true);
        unitList_go.SetActive(true);
    }

    //유닛 클릭했을 때 UnitCell에서 호출
    public void SetUnitData(UnitData ud)
    {
        unitData.gameObject.SetActive(true);
        unitData.SetData(ud);
        unitDetail.SetData(ud, false);
    }
    #endregion

    #region 아이템 탭
    //아이템 탭 눌렀을 때 호출
    public void OnItemList()
    {
        Init();

        left_Scroll_go.SetActive(true);
        itemList_go.SetActive(true);
    }

    //아이템 클릭했을 때 itemCell에서 호출
    public void SetItemData(ItemData id)
    {
        itemDescription.gameObject.SetActive(true);
        itemDescription.SetItemData(id);
    }
    #endregion
}
