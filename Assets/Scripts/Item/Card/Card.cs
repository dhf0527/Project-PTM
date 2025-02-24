using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardId;
    public ToggleGroup toggleGroup;
    private Toggle toggle;
    //���ù�ư
    public GameObject selectBtn;
    public GameObject detailBtn;
    public GameObject fakeDetailBtn;

    public AudioSource audioSource;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    private void Start()
    {
        selectBtn.SetActive(false);
        fakeDetailBtn.SetActive(true);
        detailBtn.SetActive(false);
        toggle.isOn = false;
    }

    public void OnClickCard()
    {
        StageManager.Instance.selectId = toggle.isOn ? cardId : 100;
        selectBtn.SetActive(toggle.isOn); //���ù�ư Ȱ��ȭ
        audioSource.Play();
        fakeDetailBtn.SetActive(!toggle.isOn);
        detailBtn.SetActive(toggle.isOn); //������ ��ư Ȱ��ȭ
    }
}
