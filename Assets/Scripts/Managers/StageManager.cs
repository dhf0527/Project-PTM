using Chracter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ItemData;

public class StageManager : MonoBehaviour
{
    public AudioSource touchSound;
    
    private static StageManager instance = null;

    //ī�� UI
    public GameObject cardPanel;
    public GameObject selectCardPanel;
    public Text tutorialText;
    public Text[] cardLevel;
    public Text[] cardSpawnNum;
    public Text[] cardType;
    public Image[] cardImage;
    //public Text[] cardDesc;
    public Text[] cardCost;
    public Card[] cards;
    public Image[] cardMember;
    public Text[] cardDefense;
    public Text[] cardHealth;
    public Text[] cardStrength;
    public Text[] cardAttackSpeed;
    public Image[] cardAttribute;
    public Sprite[] attributeImage;
    public Toggle[] cardToggles;
    public Sprite[] attackTypeSprites;
    public Text cardClearText;
    public Text[] cardSkill;
    public GameObject[] CardSkillPanel;

    //
    [Header("Frame")]
    public Sprite[] blueFrameImages;
    public Sprite[] greenFrameImages;
    public Sprite[] redFrameImages;
    public Sprite[] purpleFrameImages;
    public Sprite[] darkFrameImages;

    public Image[] cardUpFrame;
    public Image[] cardDownFrame;
    public Image[] cardBackground;
    public Image[] cardDarkBase;
    public Image[] FirstCardBase;
    public Image[] SecondCardBase;
    public Image[] ThirdCardBase;
    public Image[] attackTypeIcon;

    //
    [Header("Detail")]
    public Image dCardMemberIcon;
    public GameObject exitPanel;
    public GameObject detailPanel;
    public GameObject fakeDetailPanel;
    public Image dcardImage;
    public Text dCardLevel;
    public Text dCardName;
    public Text dCardCost;
    public Text dCardMember;
    public Sprite[] memberSprites;
    public Image dCardAttribute;
    public Image dCardWeakAttribute;
    public Image dCardStrongAttribute;

    public Text dCardDefenseValue;
    public Text dCardHealthValue;
    public Text dCardStrengthValue;
    public Text dCardAttackSpeedValue;
    public Text dCardAttackCountLimitValue;
    public Text dCardSpeedValue;
    public Text dCardAccuracyValue;
    public Text dCardAvoidanceValue;
    public Text dCardUnitSize;
    public Text dCardCreateCountValue;
    public Image[] dCardFrame;
    public Image dAttackTypeIcon;
    public Image dCardBackground;
    public Image[] dCardBase;
    public Text dCardSkill;
    public Text dCardSkillIntro;
    public GameObject dCardSkillPanel;
    public Text dCardSkill1;
    public Text dCardSkillIntro1;
    public GameObject dCardSkillPanel1;
    public Text dSpawnNum;

    //ī������
    public ItemData[] datas;
    public int selectId;
    private int[] plusNums = { 0, 2, 3 };

    //���ֻ����ư
    public Item[] unitButtons;

    //�������� ����
    public int wave = 1;
    public float[] firstDelay = { 10, 20, 30 };
    public float[] middleDelay = { 8, 18, 28 };
    public float[] LastDelay = { 6, 12, 24 };

    //�������� Ÿ��
    public float stageTime = 480f;
    private float totalTimer;
    private float wave1StageTime, wave2StageTime, wave3StageTime;
    private float maxStageTime;
    private float[] stageMaxTime = { 120f, 120f, 240f };
    public GameObject stageTimeObj;
    public Text stageTimeText;
    public Text waveText;
    private bool stage3TimeFlag = false;

    //GameOver
    [Header("GameOver")]
    public GameObject GameOverPanel;
    public Image fadeimage;
    private bool isGameOver = false;

    //GameClear
    [Header("GameOver")]
    public GameObject GameClearPanel;
    public Image clearfadeimage;
    private bool isGameClear = false;

    //
    //boss
    public bool isAppearBoss = false;
    public EnemyTower enemyTower;
    public GameObject BossObj;
    public Dungeon[] dungeonDatas;



    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            /*DontDestroyOnLoad(this.gameObject);*/
        }
        else
        {
            /*Destroy(this.gameObject);*/
        }
    }

    public static StageManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Start()
    {
        StageStart();
    }

    private void Update()
    {
        totalTimer += Time.deltaTime;
        stageTime -= Time.deltaTime;
        CheckStageTime();
    }

    public void StageStart()
    {
        Time.timeScale = 0f;
        MakeCard();
    }

    public void MakeCard()
    {
        int count = System.Enum.GetValues(typeof(ItemType)).Length; //���� ���� �������ϱ�
        List<int> nums = new List<int>();

        cardClearText.text = "LV." + wave + " �뺴 �ر�!";
        for (int i = 0; i < 3; i++)
        {
            int ranNum = Random.Range((wave - 1) * 5 + plusNums[wave-1], ((wave - 1) * 5) + 7 + (wave-1)); //�������� 1�̸� 0~7, 2�̸� 7~13, 3�̸� 13~19
            Debug.Log("ù��° ran ���� " + ((wave - 1) * 5 + plusNums[wave-1]).ToString());
            //�ߺ�üũ
            while(nums.Contains(ranNum)) //�����ѹ��� ���� �̹� �ִٸ�
            {
                ranNum = Random.Range((wave - 1) * 5 + plusNums[wave - 1], ((wave - 1) * 5) + 7 + (wave - 1));
            }
            nums.Add(ranNum);

            
            //UI�� �������� ���� ����ֱ�
            cardLevel[i].text = "LV." + datas[ranNum].level;
            cardSpawnNum[i].text = "X "+datas[ranNum].createCountValue.ToString();
            cardType[i].text = datas[ranNum].itemName.ToString();
            cardImage[i].sprite = datas[ranNum].itemIcon;
            //cardDesc[i].text = datas[ranNum].itemDesc.ToString();
            cardCost[i].text = datas[ranNum].cost.ToString();
            //�ٰŸ� ���Ÿ� �����ֱ�
            if (datas[ranNum].attackType == "�ٰŸ�")
            {
                attackTypeIcon[i].sprite = attackTypeSprites[0];
            }
            else
            {
                attackTypeIcon[i].sprite = attackTypeSprites[1];
            }

            //ī��������
            switch (datas[ranNum].member)
            {
                case 0:
                    cardMember[i].sprite = memberSprites[datas[ranNum].member];
                    //ī�� ������ �����ϱ�
                    cardUpFrame[i].sprite = blueFrameImages[0];
                    cardDownFrame[i].sprite = blueFrameImages[0];
                    cardBackground[i].sprite = blueFrameImages[1];
                    cardDarkBase[i].sprite = darkFrameImages[0];
                    break;
                case 1:
                    cardMember[i].sprite = memberSprites[datas[ranNum].member];
                    //ī�� ������ �����ϱ�
                    cardUpFrame[i].sprite = greenFrameImages[0];
                    cardDownFrame[i].sprite = greenFrameImages[0];
                    cardBackground[i].sprite = greenFrameImages[1];
                    cardDarkBase[i].sprite = darkFrameImages[1];
                    break;
                case 2:
                    cardMember[i].sprite = memberSprites[datas[ranNum].member];
                    //ī�� ������ �����ϱ�
                    cardUpFrame[i].sprite = redFrameImages[0];
                    cardDownFrame[i].sprite = redFrameImages[0];
                    cardBackground[i].sprite = redFrameImages[1];
                    cardDarkBase[i].sprite = darkFrameImages[2];
                    break;
                case 3:
                    cardMember[i].sprite = memberSprites[datas[ranNum].member];
                    //ī�� ������ �����ϱ�
                    cardUpFrame[i].sprite = purpleFrameImages[0];
                    cardDownFrame[i].sprite = purpleFrameImages[0];
                    cardBackground[i].sprite = purpleFrameImages[1];
                    cardDarkBase[i].sprite = darkFrameImages[2];
                    break;
            }
            SetBaseFrame(datas[ranNum].member, i);
            cardDefense[i].text = datas[ranNum].defenseValue.ToString();
            cardHealth[i].text = datas[ranNum].healthValue.ToString();
            cardStrength[i].text = datas[ranNum].strengthValue.ToString();
            cardAttackSpeed[i].text = datas[ranNum].attackSpeedValue.ToString();
            
            //cardSkill[i].text = "" , make text ����
            if(datas[ranNum].Skill == "")
            {
                CardSkillPanel[i].SetActive(false);
            }
            else
            {
                cardSkill[i].text = datas[ranNum].Skill;
            }
            

            if (datas[ranNum].Attribute == "����")
            {
                cardAttribute[i].sprite = attributeImage[0];
            }
            else if (datas[ranNum].Attribute == "����")
            {
                cardAttribute[i].sprite = attributeImage[1];
            }
            else if (datas[ranNum].Attribute == "ȭ��")
            {
                cardAttribute[i].sprite = attributeImage[2];
            }
            //
            cards[i].cardId = ranNum;
            //Debug.Log(i+"��° ī�� ���̵�: " + ranNum);

            
        }

        TutorialMsg("��ġ�� �󼼳��� Ȯ��");
        cardPanel.SetActive(true);
        selectCardPanel.SetActive(true);
    }

    public void SetBaseFrame(int member,int num)
    {
        switch (member)
        {
            case 0:
                for (int i = 0; i < FirstCardBase.Length; i++)
                {
                    switch (num)
                    {
                        case 0:
                            FirstCardBase[i].sprite = blueFrameImages[2];
                            break;
                        case 1:
                            SecondCardBase[i].sprite = blueFrameImages[2];
                            break;
                        case 2:
                            ThirdCardBase[i].sprite = blueFrameImages[2];
                            break;
                    }
                }
                break;
            case 1:
                for (int i = 0; i < FirstCardBase.Length; i++)
                {
                    switch (num)
                    {
                        case 0:
                            FirstCardBase[i].sprite = greenFrameImages[2];
                            break;
                        case 1:
                            SecondCardBase[i].sprite = greenFrameImages[2];
                            break;
                        case 2:
                            ThirdCardBase[i].sprite = greenFrameImages[2];
                            break;
                    }
                }
                break;
            case 2:
                for (int i = 0; i < FirstCardBase.Length; i++)
                {
                    switch (num)
                    {
                        case 0:
                            FirstCardBase[i].sprite = redFrameImages[2];
                            break;
                        case 1:
                            SecondCardBase[i].sprite = redFrameImages[2];
                            break;
                        case 2:
                            ThirdCardBase[i].sprite = redFrameImages[2];
                            break;
                    }
                }
                break;
            case 3:
                for (int i = 0; i < FirstCardBase.Length; i++)
                {
                    switch (num)
                    {
                        case 0:
                            FirstCardBase[i].sprite = purpleFrameImages[2];
                            break;
                        case 1:
                            SecondCardBase[i].sprite = purpleFrameImages[2];
                            break;
                        case 2:
                            ThirdCardBase[i].sprite = purpleFrameImages[2];
                            break;
                    }
                }
                break;
        }
    }

    public void SelectCard() //ī�弱�������� Ÿ�Կ� �´� ���� �ֱ�
    {
        touchSound.Play();
        for (int i = 0; i < unitButtons.Length; i++)
        {
            Debug.Log("������ ī�� ���̵�: " + selectId);

            if(unitButtons[i] != null)
            {
                Debug.Log("aeswf");
            }
       
            if (unitButtons[i].data==null)
            {
                unitButtons[i].Init(datas[selectId]);

                //Debug.Log("ī������Ϸ�");
                break;
            }
        }


        //cardDelayImage[selectId].sprite = datas[selectId].itemIcon;
        Time.timeScale = 1f;
        cardPanel.SetActive(false);


        //ī�� ����
        detailPanel.SetActive(false);
        fakeDetailPanel.SetActive(true);
        exitPanel.SetActive(false);
        selectId = 0;
        for (int i = 0; i < cardToggles.Length; i++)
        {
            cardToggles[i].isOn = false;
        }
        //enemyTower


        //���̺� ����
        switch (wave)
        {
            case 1:
                wave1StageTime = 120f;
                break;
            case 2:
                wave2StageTime = 120f;
                break;
            case 3:
                wave3StageTime = 240f;
                break;
        }

        maxStageTime = 480f;
        waveText.text = "WAVE " + wave;
        enemyTower.ReadSpawnFile();
    }

    public void CheckStageTime()
    {
        if (isGameOver)
        {
            stageTime = 0f;
            return;
        }

        //Debug.Log("�������� �ð� : " + Mathf.Floor(stageTime));
        if (wave == 1)
        {
            //30�� (�߹����� �Ѿ��)
            if (Mathf.Floor(stageTime) == maxStageTime - 30f)
            {
                enemyTower.spwanDelay1 = middleDelay[1];
                enemyTower.spwanDelay2 = middleDelay[2];
                enemyTower.spwanDelay3 = middleDelay[3];
            }
            //60�� (�Ĺ����� �Ѿ��)
            if (Mathf.Floor(stageTime) == maxStageTime - 90f)
            {
                enemyTower.spwanDelay1 = LastDelay[0];
                enemyTower.spwanDelay2 = LastDelay[1];
                enemyTower.spwanDelay3 = LastDelay[2];
            }

            if (stageTime <= maxStageTime - wave1StageTime + 10f)
            {
                if (stageTime <= 360f)
                {
                    stageTimeObj.SetActive(false);
                    wave++;
                    StageStart();
                }
                else
                {
                    stageTimeObj.SetActive(true);
                    stageTimeText.text = "���� �ð� " + Mathf.Floor(stageTime - 360f + 1f) + "��!";
                }
            }
        }
        else if (wave == 2)
        {
            //30�� (�߹����� �Ѿ��)
            if (Mathf.Floor(stageTime) == maxStageTime - 30f - wave1StageTime)
            {
                enemyTower.spwanDelay1 = middleDelay[1];
                enemyTower.spwanDelay2 = middleDelay[2];
                enemyTower.spwanDelay3 = middleDelay[3];
            }
            //60�� (�Ĺ����� �Ѿ��)
            if (Mathf.Floor(stageTime) == maxStageTime - 90f - wave1StageTime)
            {
                enemyTower.spwanDelay1 = LastDelay[0];
                enemyTower.spwanDelay2 = LastDelay[1];
                enemyTower.spwanDelay3 = LastDelay[2];
            }

            if (stageTime <= 240f + 10f) //480-120-120 250
            {
                if (stageTime <= 240f)
                {
                    stageTimeObj.SetActive(false);
                    wave++;
                    StageStart();
                }
                else
                {
                    stageTimeObj.SetActive(true);
                    stageTimeText.text = "���� �ð� " + Mathf.Floor(stageTime - 240f + 1f) + "��!";
                }
            }
        }
        else if (wave == 3)
        {
            //30�� (�߹����� �Ѿ��)
            if (Mathf.Floor(stageTime) == wave3StageTime - 30f)
            {
                enemyTower.spwanDelay1 = middleDelay[1];
                enemyTower.spwanDelay2 = middleDelay[2];
                enemyTower.spwanDelay3 = middleDelay[3];
            }
            //60�� (�Ĺ����� �Ѿ��)
            if (Mathf.Floor(stageTime) == wave3StageTime - 90f)
            {
                enemyTower.spwanDelay1 = LastDelay[0];
                enemyTower.spwanDelay2 = LastDelay[1];
                enemyTower.spwanDelay3 = LastDelay[2];
            }

            if (Mathf.Floor(stageTime) == 120f)
            {
                stageTimeObj.SetActive(true);
                stageTimeText.text = "���� �ð� " + Mathf.Floor(stageTime+1.0f) + "��!";
                Invoke("SetFalseTimeText", 3f);
            }
            else if (stageTime <= 10f)
            {
                stageTimeObj.SetActive(true);
                stageTimeText.text = "���� �ð� " + Mathf.Floor(stageTime + 1f) + "��!";
            }

            if (!isAppearBoss && stageTime <= 240f - 15f) //������ ������ ���� ���ٸ�
            {
                isAppearBoss = true;
                AppearBoss();
            }
        }

        if (stageTime <= 0f && !isGameOver)
        {
            stageTimeText.text = "�ð��ʰ�...";
            isGameOver = true;
            GameOver();
        }
    }

    public void OnClickDetail()
    {
        touchSound.Play();
        exitPanel.SetActive(true);
        detailPanel.SetActive(true);
        selectCardPanel.SetActive(false);
        TutorialMsg("�� ������ ��ġ�� ��������");

        //member, ������
        switch (datas[selectId].member)
        {
            case 0:
                dCardMember.text = "�߾ӿձ�";
                dCardMemberIcon.sprite = memberSprites[0];
                dCardFrame[0].sprite = blueFrameImages[0];
                dCardFrame[1].sprite = blueFrameImages[0];
                dCardBackground.sprite = blueFrameImages[1];
                for (int i = 0; i < dCardBase.Length; i++)
                {
                    dCardBase[i].sprite = blueFrameImages[2];
                }
                break;
            case 1:
                dCardMember.text = "������";
                dCardFrame[0].sprite = greenFrameImages[0];
                dCardMemberIcon.sprite = memberSprites[1];
                dCardFrame[1].sprite = greenFrameImages[0];
                dCardBackground.sprite = greenFrameImages[1];
                for (int i = 0; i < dCardBase.Length; i++)
                {
                    dCardBase[i].sprite = greenFrameImages[2];
                }
                break;
            case 2:
                dCardMember.text = "���ձ�";
                dCardFrame[0].sprite = redFrameImages[0];
                dCardMemberIcon.sprite = memberSprites[2];
                dCardFrame[1].sprite = redFrameImages[0];
                dCardBackground.sprite = redFrameImages[1];
                for (int i = 0; i < dCardBase.Length; i++)
                {
                    dCardBase[i].sprite = redFrameImages[2];
                }
                break;
            case 3:
                dCardMember.text = "������";
                dCardFrame[0].sprite = purpleFrameImages[0];
                dCardMemberIcon.sprite = memberSprites[3];
                dCardFrame[1].sprite = purpleFrameImages[0];
                dCardBackground.sprite = purpleFrameImages[1];
                for (int i = 0; i < dCardBase.Length; i++)
                {
                    dCardBase[i].sprite = purpleFrameImages[2];
                }
                break;
        }

        dcardImage.sprite = datas[selectId].itemIcon;
        dCardLevel.text = "LV." + datas[selectId].level;
        dCardName.text = datas[selectId].itemName;
        dCardCost.text = datas[selectId].cost.ToString();

        if (datas[selectId].Skill == "")
        {
            dCardSkillPanel.SetActive(false);
        }
        else
        {
            dCardSkillPanel.SetActive(true);
        }
        if(datas[selectId].SkillNext == "")
        {
            dCardSkillPanel1.SetActive(false);
        }
        else
        {
            dCardSkillPanel1.SetActive(true);
        }
        dCardSkill.text = datas[selectId].Skill;
        dCardSkillIntro.text = datas[selectId].SkillIntro;
        dCardSkill1.text = datas[selectId].SkillNext;
        dCardSkillIntro1.text = datas[selectId].SkillIntroNext;

        if (datas[selectId].Attribute == "����") //���� ������
        {
            dCardAttribute.sprite = attributeImage[0];
        }
        else if (datas[selectId].Attribute == "����")
        {
            dCardAttribute.sprite = attributeImage[1];
        }
        else if (datas[selectId].Attribute == "ȭ��")
        {
            dCardAttribute.sprite = attributeImage[2];
        }

        if (datas[selectId].weakAttribute == "����") //��� �Ӽ�
        {
            dCardWeakAttribute.color = new Color(0, 0, 0, 0);
        }
        else
        {
            dCardWeakAttribute.color = new Color(1,1,1,1);
        }

        if (datas[selectId].weakAttribute == "����")
        {
            dCardWeakAttribute.sprite = attributeImage[0];
        }
        else if (datas[selectId].weakAttribute == "����")
        {
            dCardWeakAttribute.sprite = attributeImage[1];
        }
        else if (datas[selectId].weakAttribute == "ȭ��")
        {
            dCardWeakAttribute.sprite = attributeImage[2];
        }

        if (datas[selectId].strongAttribute == "����") //���� �Ӽ�
        {
            dCardStrongAttribute.color = new Color(0, 0, 0, 0);
        }
        else
        {
            dCardStrongAttribute.color = new Color(1, 1, 1, 1);
        }

        if (datas[selectId].strongAttribute == "����")
        {
            dCardStrongAttribute.sprite = attributeImage[0];
        }
        else if (datas[selectId].strongAttribute == "����")
        {
            dCardStrongAttribute.sprite = attributeImage[1];
        }
        else if (datas[selectId].strongAttribute == "ȭ��")
        {
            dCardStrongAttribute.sprite = attributeImage[2];
        }



        if (datas[selectId].attackType == "�ٰŸ�") //���� ������
        {
            dAttackTypeIcon.sprite = attackTypeSprites[0];
        }
        else
        {
            dAttackTypeIcon.sprite = attackTypeSprites[1];
        }


        //�ؽ�Ʈ
        dSpawnNum.text = "X " + datas[selectId].createCountValue;
        dCardDefenseValue.text = datas[selectId].defenseValue.ToString();
        dCardHealthValue.text = datas[selectId].healthValue.ToString();
        dCardStrengthValue.text = datas[selectId].strengthValue.ToString();
        dCardAttackSpeedValue.text = datas[selectId].attackSpeedValue.ToString(); 
        dCardAttackCountLimitValue.text = datas[selectId].attackCountLimitValue.ToString();
        dCardSpeedValue.text = datas[selectId].speedValue.ToString();
        dCardAccuracyValue.text = datas[selectId].accuracyValue.ToString();
        dCardAvoidanceValue.text = datas[selectId].avoidanceValue.ToString();
        
        switch (datas[selectId].size)
        {
            case 1:
                dCardUnitSize.text = "����";
                break;
            case 2:
                dCardUnitSize.text = "����";
                break;
            case 3:
                dCardUnitSize.text = "����";
                break;
        }
        dCardCreateCountValue.text = datas[selectId].createCountValue.ToString();
    }

    public void OnClickExit()
    {
        touchSound.Play();
        exitPanel.SetActive(false);
        detailPanel.SetActive(false);
        selectCardPanel.SetActive(true);
        TutorialMsg("��ġ�� �󼼳��� Ȯ��");
    }

    public void SetFalseTimeText()
    {
        stageTimeObj.SetActive(false);
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
        StartCoroutine(FadeCor());
    }

    IEnumerator FadeCor()
    {
        float fadeCount = 0;
        while (true)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.02f);
            fadeimage.color = new Color(0.47f, 0.47f, 0.47f, fadeCount);

            if (fadeCount >= .8f)
            {
                TimeScaleZero();
                break;
            }
        }
    }

    public void GameClear()
    {
        if (isGameClear)
            return;

        PlayerPrefs.SetInt("dungeonTime"+DataManager.currentDungeon, Mathf.FloorToInt(totalTimer));
        PlayerPrefs.SetInt("dungeonClear" + DataManager.currentDungeon, 1);
        dungeonDatas[DataManager.currentDungeon].isClear = true;
        //

        isGameClear = true;
        GameClearPanel.SetActive(true);
        StartCoroutine(GameClearCor());
    }

    IEnumerator GameClearCor()
    {
        Time.timeScale = 1.0f;
        float fadeCount = 0;
        while (true)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.02f);
            clearfadeimage.color = new Color(0.47f, 0.47f, 0.47f, fadeCount);

            if (fadeCount >= .8f)
            {
                TimeScaleZero();
                break;
            }
        }
    }

    public void TimeScaleZero()
    {
        Time.timeScale = 0f;
    }

    public GameObject AppearBoss()
    {
        GameObject boss = GameObject.Instantiate(BossObj, enemyTower.enemySpawnPoint); //������ ���س��� �迭�� n��° ������Ʈ�� ����
        BaseCharacter bossCharacter = boss.GetComponent<BaseCharacter>();
        bossCharacter.Spawn();
        bossCharacter.ChangeBossStats(8,2,0.8f);
        enemyTower.Boss = boss;
        return boss;
    }

    public void TutorialMsg(string msg)
    {
        tutorialText.text = msg;
    }
}