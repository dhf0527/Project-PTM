using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    //���� ������
    public float maxSpawnDelay;
    public float spawnDelay = 0;
    private bool isCanSpawn = true;
    private bool isFirstSpawn = true;
    public AudioSource audioSource;



    public SpawnPoint spawnPoint;
    //info data
    public ItemData data;
    public int level;
    [SerializeField] private Tower towerScript;

    //UI
    public GameObject lockImageGO;
    public GameObject costTextGO;
    public Image unitImage;
    public Image delayImage; //delay image
    public Text levelText;
    public Text costText;
    public GameObject warningText;

    private int SpawnNum = 1;

    //GameObject
    [SerializeField] private GameObject[] unitObejcts;
    
    [SerializeField] GameObject spawnEffect;

    private bool bEffect = false;
    private void Start()
    {
        //���� �̹���
        Init(data);
    }

    private void Update()
    {
        spawnDelay -= Time.deltaTime;
        float time = spawnDelay / maxSpawnDelay;
        delayImage.fillAmount = time;

        if (spawnDelay <= 0f)
        {
            isCanSpawn = true;
        }
        if(delayImage.fillAmount <= 0.05f && bEffect)
        {
            SpawnEffect();
        }
    }

    private void SpawnEffect()
    {
        spawnEffect.GetComponent<Animation>().Play();
        bEffect = false;
    }
    
    public void Init(ItemData itemData)
    {
        if (data == null && itemData==null)
        {
            lockImageGO.SetActive(true);
            costTextGO.SetActive(false);
            return;
        }

        data = itemData;
        unitImage.sprite = data.itemIcon;
        lockImageGO.SetActive(false);
        costTextGO.SetActive(true);

        costText.text = data.cost.ToString();
        levelText.text = "Lv." + data.level;
       
        //���� ������ �־��ֱ�
        switch (data.level)
        {
            case 1:
                maxSpawnDelay = 4;
                break;
            case 2:
                maxSpawnDelay = 12;
                break;
            case 3:
                maxSpawnDelay = 20;
                break;
        }
        if (isFirstSpawn)
            return;

        isFirstSpawn = false;
        spawnDelay = maxSpawnDelay;
    }

    public void CreateUnit()
    {
        if (data == null)
            return;

        if (spawnDelay > 0f || towerScript.currentGold - data.cost < 0) //������ �ð��� ������ �ʾҴٸ�
        {
            //Warn();

            return;
        }
        audioSource.Play();
        bEffect = true;
        switch (data.itemType)
        {
            case ItemData.ItemType.Warrior: //�˻� LV1 ~
                spawnPoint.CharacterIndex = 0;   
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.Thief:   //����
                spawnPoint.CharacterIndex = 1;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.Archer:  //�����ü�
                spawnPoint.CharacterIndex = 2;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.Goblin:  //���
                spawnPoint.CharacterIndex = 3;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.Slime:  //������
                spawnPoint.CharacterIndex = 4;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.SkeletonWarrior:  //�ذ�����
                spawnPoint.CharacterIndex = 5;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.Bat:  //��ü����
                spawnPoint.CharacterIndex = 6;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.Wizard:  //������ LV2 ~
                spawnPoint.CharacterIndex = 7;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.MagicWarrior:  //���˻�
                spawnPoint.CharacterIndex = 8;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.Minotaur:  //�̳�Ÿ��ν�
                spawnPoint.CharacterIndex = 9;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.Centaur:  //��Ÿ��ν�
                spawnPoint.CharacterIndex = 10;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.Golem:  //��
                spawnPoint.CharacterIndex = 11;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.FireSkull:  //��Ÿ���ذ�
                spawnPoint.CharacterIndex = 12;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.Paladin:  //����� LV3~
                spawnPoint.CharacterIndex = 13;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.DeerWarrior:  //�罿���
                spawnPoint.CharacterIndex = 14;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.WoodGolem:  //��������
                spawnPoint.CharacterIndex = 15;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.Elementalist:  //���ɼ���
                spawnPoint.CharacterIndex = 16;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.Dragon:  //��Ȯ��
                spawnPoint.CharacterIndex = 17;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;

            case ItemData.ItemType.Reaper:  //�巹��
                spawnPoint.CharacterIndex = 18;
                spawnPoint.SpawnCharacter(data.createCountValue);
                break;
        }

        spawnDelay = maxSpawnDelay;
    }
    /*public void Warn()
    {
        warningText.SetActive(true);
        Invoke("SetFalseWarn", 1f);
    }

    public void SetFalseWarn()
    {
        warningText.SetActive(false);
    }*/
}
