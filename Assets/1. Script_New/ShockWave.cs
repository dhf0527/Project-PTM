using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    float curTime = 0;

    private void Update()
    {
        if (curTime < 3)
        {
        curTime += Time.deltaTime;
        transform.localScale = Vector3.one * curTime * 50;
        }    
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Unit>() && collision.gameObject.GetComponent<Unit>().IsTeam)
        {
            //가장자리 부분에 있던 적만 넉백
            if(collision.transform.position.x < (GetComponent<Collider2D>().bounds.min.x + 0.1f * transform.localScale.x) )
                collision.gameObject.GetComponent<Unit>().OnStartKnockBack();
        }
    }
}
