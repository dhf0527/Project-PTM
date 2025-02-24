using Chracter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Tower : BaseCharacter
{
    [Header("Tower")]
    public bool isGameOver = false;
    //sprite
    public SpriteRenderer renderer;
    //gold
    [Header("Gold")]
    public float currentGold = 0;
    public float maxGold = 1000;
    [SerializeField] private int goldPerSec = 10;
    private bool isCanGetGold = true;
    //time
    [Header("Time")]
    [SerializeField] private float currentTime;
    [SerializeField] private float maxTime = .1f;
    //UI
    [Header("UI")]
    public Text goldValueText;
    public Text goldPerSecText;
    //Tower Stats
    [Header("Tower Stats")]
    public int maxHp = 500;
    public UnityEngine.UI.Slider towerHPSlider;
    public Text towerHPText;
    
    public AudioSource TowerUpgradeSound;
    public GameObject TowerUpgradeEffect;
    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetCharacterSettings(maxHp);
        goldPerSecText.text = "+" + goldPerSec + "/s";
        goldValueText.text = currentGold + " / " + maxGold;

        healthBar.TowerHealth(maxHp,maxHp);
    }

    private void Update()
    {

        currentTime += Time.deltaTime;
        if (currentTime >= maxTime)
        {
            currentTime = 0;
            GetGold();
        }

        if (isGameOver)
            return;

        towerHPText.text = CurrentHealth + " / " + MaxHealth;
        towerHPSlider.value = CurrentHealth;
        towerHPSlider.maxValue = maxHp;
    }

    public void GetGold()
    {
        currentGold += goldPerSec / 10;
        if (currentGold > maxGold)
        {
            currentGold = maxGold;
            isCanGetGold = false;
        }
        else
        {
            isCanGetGold = true;
        }
        InitUI();
    }

    public void InitUI()
    {
        goldValueText.text = currentGold + " / " + maxGold;
    }

    public void HitAnim()
    {
        renderer.color = Color.red;
        Invoke("InvokeHitAnim", .5f);

    }

    public void ReturnColor()
    {
        renderer.color = Color.white;
    }

    public override void Die()
    {
        isGameOver = true;
        StageManager.Instance.GameOver();
    }

    public override void TakeDamage(float damage, float enemyAccuracy = 200,bool pierce=false, string weak = "없음")
    {
        if (isGameOver)
            return;


        float finalDamage = damage - Armor;
        if (finalDamage <= 0)
        {
            finalDamage = 1;
        }
        CurrentHealth -= finalDamage;
        if (CurrentHealth < 0) //죽음 처리
        {
            Die();
            CurrentHealth = 0;
        }


        if (healthBar != null)
        {
            healthBar.TowerHealth(CurrentHealth, MaxHealth);
        }
    }


    int towerLevel = 1;
    int[] upgradeCost = { 0, 25, 50, 100, 200, 400, 800, 1600, 3200 };
    int[] upgradeMaxGold = {0, 100, 330, 720, 1300, 2100, 3150, 4480, 6120, 8100 };
    int[] upgradeMoneyUp = { 0, 5, 5, 5, 10, 10, 10, 15, 20 };
    public Text towerLevelText;
    public Text upgradeCostText;
    public UnityEngine.UI.Button upgradeBtn;
    

    public void UPgradeTower()
    {
        if (towerLevel == 9)
            return;

        if (currentGold >= upgradeCost[towerLevel])
        {
            //
            towerLevel++;


            currentGold -= upgradeCost[towerLevel-1];
            maxGold = upgradeMaxGold[towerLevel];
            CurrentHealth += 200;
            MaxHealth += 200;
            towerHPSlider.maxValue = MaxHealth;
            goldPerSec += upgradeMoneyUp[towerLevel-1];
            Armor++;

            //ui
            //체력은 업데이트에서 자동으로 초기화됨
            if (towerLevel >= 9)
            {
                upgradeBtn.enabled = false;
                upgradeCostText.text = "MAX";
            }
            else
            {
                upgradeCostText.text = upgradeCost[towerLevel].ToString();
            }
            towerLevelText.text = "Level " + towerLevel;
            goldPerSecText.text = "+" + goldPerSec + "/s";
            InitUI();
            TowerUpgradeSound.Play();
            TowerUpgradeEffect.GetComponent<Animation>().Play();
        }
    }
}
