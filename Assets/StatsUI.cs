using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] GameObject[] statsImage;
    [SerializeField] String statName;

    int UpgradeSoul;
    int statCount;
    int CurrentSoul;
    [SerializeField]
    Text SoulText;
    



    void Start()
    {
        PlayerPrefs.SetInt("CurrentSoul", 10000);
        CurrentSoul = PlayerPrefs.GetInt("CurrentSoul",0);
        SoulText.text = CurrentSoul.ToString();
        statCount = PlayerPrefs.GetInt(statName, 0);
        for(int i=0; i < statCount; i++)
        {
            statsImage[i].SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        //
    }

    public void GetUpgradeFee()
    {
        switch (statCount)
        {
            case 0:
                UpgradeSoul = 500;
                break;
            case 1:
                UpgradeSoul = 1000;
                break;

            case 2:
                UpgradeSoul = 2000;
                break;
            case 3:
                UpgradeSoul = 4000;
                break;
            case 4:
                UpgradeSoul = 8000;
                break;

        }
    }


    public void StatUpgrade()
    {
        if (statsImage[4].activeInHierarchy)
        {
            Debug.Log("Max Stats");
            return;
        }
        if (CurrentSoul>=UpgradeSoul)
        {
            PlayerPrefs.SetInt("CurrentSoul", CurrentSoul - UpgradeSoul);
            CurrentSoul = CurrentSoul - UpgradeSoul;

            for (int i = 0; i < statsImage.Length; i++)
            {
                if (!statsImage[i].activeInHierarchy)
                {



                    statsImage[i].SetActive(true);
                    PlayerPrefs.SetInt(statName, i + 1);
                    return;
                }
            }
        }
    }
}
