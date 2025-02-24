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





    public void SpawnCharacter(int spawnNum=1) //Item ��Ʈ��Ʈ���� CharacterIndex��, ���������̸� �������ְ�.    =>    �� �Լ� ȣ��
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


        Debug.Log("ĳ���� �ε����� :"+ CharacterIdx);
        //Team.GetComponent<BaseCharacter>().Spawn();
        //UI Init
        towerScript.InitUI(); //��� �ؽ�Ʈ �ʱ�ȭ
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
