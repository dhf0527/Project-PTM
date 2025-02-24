using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject upgrade;
    public GameObject upgradeposition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickUpgrade()
    {
        Debug.Log(BaseGameManager.instance.AttackStats);
        BaseGameManager.instance.AttackStats = 2.0f;

        Instantiate(upgrade, transform.position, Quaternion.identity);
    }
}
