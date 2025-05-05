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

    public GameObject heroList_go;
    public HeroData heroData;

    public GameObject heroSkill_go;
    public GameObject heroDescription_go;

    public HeroDetail HeroDetail;

    [HideInInspector] public UnitData hero_ud;

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

    }

    public void OnCloseButton()
    {
        gameObject.SetActive(false);
        Init();
    }

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
        hero_ud = ud;
        heroData.SetData(ud);
        HeroDetail.SetData(ud);
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
}
