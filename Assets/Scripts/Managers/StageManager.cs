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

    //카드 UI
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

    //카드정보
    public ItemData[] datas;
    public int selectId;
    private int[] plusNums = { 0, 2, 3 };

    //유닛생산버튼
    public Item[] unitButtons;

    //스테이지 정보
    public int wave = 1;
    public float[] firstDelay = { 10, 20, 30 };
    public float[] middleDelay = { 8, 18, 28 };
    public float[] LastDelay = { 6, 12, 24 };

    //스테이지 타임
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
        int count = System.Enum.GetValues(typeof(ItemType)).Length; //유닛 종류 개수구하기
        List<int> nums = new List<int>();

        cardClearText.text = "LV." + wave + " 용병 해금!";
        for (int i = 0; i < 3; i++)
        {
            int ranNum = Random.Range((wave - 1) * 5 + plusNums[wave-1], ((wave - 1) * 5) + 7 + (wave-1)); //스테이지 1이면 0~7, 2이면 7~13, 3이면 13~19
            Debug.Log("첫번째 ran 쑷자 " + ((wave - 1) * 5 + plusNums[wave-1]).ToString());
            //중복체크
            while(nums.Contains(ranNum)) //랜덤넘버가 만약 이미 있다면
            {
                ranNum = Random.Range((wave - 1) * 5 + plusNums[wave - 1], ((wave - 1) * 5) + 7 + (wave - 1));
            }
            nums.Add(ranNum);

            
            //UI에 랜덤유닛 정보 집어넣기
            cardLevel[i].text = "LV." + datas[ranNum].level;
            cardSpawnNum[i].text = "X "+datas[ranNum].createCountValue.ToString();
            cardType[i].text = datas[ranNum].itemName.ToString();
            cardImage[i].sprite = datas[ranNum].itemIcon;
            //cardDesc[i].text = datas[ranNum].itemDesc.ToString();
            cardCost[i].text = datas[ranNum].cost.ToString();
            //근거리 원거리 정보넣기
            if (datas[ranNum].attackType == "근거리")
            {
                attackTypeIcon[i].sprite = attackTypeSprites[0];
            }
            else
            {
                attackTypeIcon[i].sprite = attackTypeSprites[1];
            }

            //카드프레임
            switch (datas[ranNum].member)
            {
                case 0:
                    cardMember[i].sprite = memberSprites[datas[ranNum].member];
                    //카드 프레임 적용하기
                    cardUpFrame[i].sprite = blueFrameImages[0];
                    cardDownFrame[i].sprite = blueFrameImages[0];
                    cardBackground[i].sprite = blueFrameImages[1];
                    cardDarkBase[i].sprite = darkFrameImages[0];
                    break;
                case 1:
                    cardMember[i].sprite = memberSprites[datas[ranNum].member];
                    //카드 프레임 적용하기
                    cardUpFrame[i].sprite = greenFrameImages[0];
                    cardDownFrame[i].sprite = greenFrameImages[0];
                    cardBackground[i].sprite = greenFrameImages[1];
                    cardDarkBase[i].sprite = darkFrameImages[1];
                    break;
                case 2:
                    cardMember[i].sprite = memberSprites[datas[ranNum].member];
                    //카드 프레임 적용하기
                    cardUpFrame[i].sprite = redFrameImages[0];
                    cardDownFrame[i].sprite = redFrameImages[0];
                    cardBackground[i].sprite = redFrameImages[1];
                    cardDarkBase[i].sprite = darkFrameImages[2];
                    break;
                case 3:
                    cardMember[i].sprite = memberSprites[datas[ranNum].member];
                    //카드 프레임 적용하기
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
            
            //cardSkill[i].text = "" , make text 없음
            if(datas[ranNum].Skill == "")
            {
                CardSkillPanel[i].SetActive(false);
            }
            else
            {
                cardSkill[i].text = datas[ranNum].Skill;
            }
            

            if (datas[ranNum].Attribute == "물리")
            {
                cardAttribute[i].sprite = attributeImage[0];
            }
            else if (datas[ranNum].Attribute == "마법")
            {
                cardAttribute[i].sprite = attributeImage[1];
            }
            else if (datas[ranNum].Attribute == "화염")
            {
                cardAttribute[i].sprite = attributeImage[2];
            }
            //
            cards[i].cardId = ranNum;
            //Debug.Log(i+"번째 카드 아이디: " + ranNum);

            
        }

        TutorialMsg("터치해 상세내용 확인");
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

    public void SelectCard() //카드선택했을때 타입에 맞는 정보 넣기
    {
        touchSound.Play();
        for (int i = 0; i < unitButtons.Length; i++)
        {
            Debug.Log("선택한 카드 아이디: " + selectId);

            if(unitButtons[i] != null)
            {
                Debug.Log("aeswf");
            }
       
            if (unitButtons[i].data==null)
            {
                unitButtons[i].Init(datas[selectId]);

                //Debug.Log("카드적용완료");
                break;
            }
        }


        //cardDelayImage[selectId].sprite = datas[selectId].itemIcon;
        Time.timeScale = 1f;
        cardPanel.SetActive(false);


        //카드 끄기
        detailPanel.SetActive(false);
        fakeDetailPanel.SetActive(true);
        exitPanel.SetActive(false);
        selectId = 0;
        for (int i = 0; i < cardToggles.Length; i++)
        {
            cardToggles[i].isOn = false;
        }
        //enemyTower


        //웨이브 설정
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

        //Debug.Log("스테이지 시간 : " + Mathf.Floor(stageTime));
        if (wave == 1)
        {
            //30초 (중반으로 넘어가기)
            if (Mathf.Floor(stageTime) == maxStageTime - 30f)
            {
                enemyTower.spwanDelay1 = middleDelay[1];
                enemyTower.spwanDelay2 = middleDelay[2];
                enemyTower.spwanDelay3 = middleDelay[3];
            }
            //60초 (후반으로 넘어가기)
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
                    stageTimeText.text = "남은 시간 " + Mathf.Floor(stageTime - 360f + 1f) + "초!";
                }
            }
        }
        else if (wave == 2)
        {
            //30초 (중반으로 넘어가기)
            if (Mathf.Floor(stageTime) == maxStageTime - 30f - wave1StageTime)
            {
                enemyTower.spwanDelay1 = middleDelay[1];
                enemyTower.spwanDelay2 = middleDelay[2];
                enemyTower.spwanDelay3 = middleDelay[3];
            }
            //60초 (후반으로 넘어가기)
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
                    stageTimeText.text = "남은 시간 " + Mathf.Floor(stageTime - 240f + 1f) + "초!";
                }
            }
        }
        else if (wave == 3)
        {
            //30초 (중반으로 넘어가기)
            if (Mathf.Floor(stageTime) == wave3StageTime - 30f)
            {
                enemyTower.spwanDelay1 = middleDelay[1];
                enemyTower.spwanDelay2 = middleDelay[2];
                enemyTower.spwanDelay3 = middleDelay[3];
            }
            //60초 (후반으로 넘어가기)
            if (Mathf.Floor(stageTime) == wave3StageTime - 90f)
            {
                enemyTower.spwanDelay1 = LastDelay[0];
                enemyTower.spwanDelay2 = LastDelay[1];
                enemyTower.spwanDelay3 = LastDelay[2];
            }

            if (Mathf.Floor(stageTime) == 120f)
            {
                stageTimeObj.SetActive(true);
                stageTimeText.text = "남은 시간 " + Mathf.Floor(stageTime+1.0f) + "초!";
                Invoke("SetFalseTimeText", 3f);
            }
            else if (stageTime <= 10f)
            {
                stageTimeObj.SetActive(true);
                stageTimeText.text = "남은 시간 " + Mathf.Floor(stageTime + 1f) + "초!";
            }

            if (!isAppearBoss && stageTime <= 240f - 15f) //보스가 출현한 적이 없다면
            {
                isAppearBoss = true;
                AppearBoss();
            }
        }

        if (stageTime <= 0f && !isGameOver)
        {
            stageTimeText.text = "시간초과...";
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
        TutorialMsg("빈 공간을 터치해 이전으로");

        //member, 프레임
        switch (datas[selectId].member)
        {
            case 0:
                dCardMember.text = "중앙왕국";
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
                dCardMember.text = "요정숲";
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
                dCardMember.text = "마왕군";
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
                dCardMember.text = "묘지기";
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

        if (datas[selectId].Attribute == "물리") //물리 아이콘
        {
            dCardAttribute.sprite = attributeImage[0];
        }
        else if (datas[selectId].Attribute == "마법")
        {
            dCardAttribute.sprite = attributeImage[1];
        }
        else if (datas[selectId].Attribute == "화염")
        {
            dCardAttribute.sprite = attributeImage[2];
        }

        if (datas[selectId].weakAttribute == "없음") //취약 속성
        {
            dCardWeakAttribute.color = new Color(0, 0, 0, 0);
        }
        else
        {
            dCardWeakAttribute.color = new Color(1,1,1,1);
        }

        if (datas[selectId].weakAttribute == "물리")
        {
            dCardWeakAttribute.sprite = attributeImage[0];
        }
        else if (datas[selectId].weakAttribute == "마법")
        {
            dCardWeakAttribute.sprite = attributeImage[1];
        }
        else if (datas[selectId].weakAttribute == "화염")
        {
            dCardWeakAttribute.sprite = attributeImage[2];
        }

        if (datas[selectId].strongAttribute == "없음") //저항 속성
        {
            dCardStrongAttribute.color = new Color(0, 0, 0, 0);
        }
        else
        {
            dCardStrongAttribute.color = new Color(1, 1, 1, 1);
        }

        if (datas[selectId].strongAttribute == "물리")
        {
            dCardStrongAttribute.sprite = attributeImage[0];
        }
        else if (datas[selectId].strongAttribute == "마법")
        {
            dCardStrongAttribute.sprite = attributeImage[1];
        }
        else if (datas[selectId].strongAttribute == "화염")
        {
            dCardStrongAttribute.sprite = attributeImage[2];
        }



        if (datas[selectId].attackType == "근거리") //물리 아이콘
        {
            dAttackTypeIcon.sprite = attackTypeSprites[0];
        }
        else
        {
            dAttackTypeIcon.sprite = attackTypeSprites[1];
        }


        //텍스트
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
                dCardUnitSize.text = "소형";
                break;
            case 2:
                dCardUnitSize.text = "중형";
                break;
            case 3:
                dCardUnitSize.text = "대형";
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
        TutorialMsg("터치해 상세내용 확인");
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
        GameObject boss = GameObject.Instantiate(BossObj, enemyTower.enemySpawnPoint); //순서를 정해놓은 배열의 n번째 오브젝트가 보스
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