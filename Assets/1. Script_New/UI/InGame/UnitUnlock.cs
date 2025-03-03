using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUnlock : MonoBehaviour
{
    public List<Card_new> cards;
    [SerializeField] Button select_Button;

    Unit selected_Unit;
    int level = 0;

    private void Awake()
    {
        Init();
    }

    //������ ī�带 ������ �������� ��Ӱ� ����� �Լ�
    public void SetDark()
    {
        foreach (var item in cards)
        {
            item.SetDarkMask();
        }
    }

    //card toggle���� ȣ��
    public void OnCardSelect()
    {
        SetDark();

        foreach(var item in cards)
        {
            if(item.GetComponent<Toggle>().isOn)
            {
                selected_Unit = item.unit;
                break;
            }
        }
    }

    //���� ��ư ������ �� ȣ��
    public void OnSelectButton()
    {
        //���� ������ ����
        DunGeonManager_New.instance.spawnUnits[level] = selected_Unit;
        //���� ���� ��ư ����ȭ
        DunGeonManager_New.instance.SetUnitSpawnButton();
        //���� �ر��� ���� ���� ����
        level++;
        //����â �ݱ�
        OpenUnitUnlock(false);
    }

    public void OpenUnitUnlock(bool isOpen)
    {
        Init();
        if (isOpen)
        {
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }

    public void Init()
    {
        //ī�� ���� �ʱ�ȭ
        foreach (var item in cards)
            item.GetComponent<Toggle>().isOn = false;

        select_Button.interactable = false;
    }
}
