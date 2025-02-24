using Chracter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject[] characterPrefab;

    [SerializeField] public int CharacterIndex=0;

    [SerializeField] private Tower towerScript;
    [SerializeField] private ItemData[] datas;

    private int SpawnNum = 1;





    public void SpawnCharacter(int spawnNum=1) //Item 스트립트에서 CharacterIndex값, 스폰딜레이를 설정해주고.    =>    이 함수 호출
    {
        int CharacterIdx;

        if(spawnNum<=1)
        {
            spawnNum = 1;
        }
        CharacterIdx = CharacterIndex;

        towerScript.currentGold -= datas[CharacterIdx].cost;
        //spawn character
        //GameObject Team = Instantiate(characterPrefab[CharacterIndex], transform.position, Quaternion.identity);

        StartCoroutine(Spawns(spawnNum,CharacterIdx));


        Debug.Log("캐릭터 인덱스는 :"+ CharacterIdx);
        //Team.GetComponent<BaseCharacter>().Spawn();
        //UI Init
        towerScript.InitUI(); //골드 텍스트 초기화
    }

    public IEnumerator Spawns(int num,int index)
    {
        int spawnCount = num;

        for(int i=1; i<=num;i++)
        {
            GameObject Team = Instantiate(characterPrefab[index], transform.position, Quaternion.identity);
            Team.GetComponent<BaseCharacter>().Spawn();
            yield return new WaitForSeconds(1);
        }


    }
    

    
}
