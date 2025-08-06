using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    public float speed;
    float width;
    GameObject clone;
    bool isCloneRight = true;

    private void Start()
    {
        width = GetComponent<SpriteRenderer>().size.x;
        clone = Instantiate(gameObject, transform.parent);
        Destroy(clone.GetComponent<BackGroundMove>());
        Destroy(clone.GetComponent<Animator>());

        /*
        clone.AddComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        clone.GetComponent<SpriteRenderer>().drawMode = GetComponent<SpriteRenderer>().drawMode;
        clone.GetComponent<SpriteRenderer>().size = GetComponent<SpriteRenderer>().size;
        */
    }

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        if(transform.position.x < -width)
        {
            transform.position += Vector3.right * width * 2;
            isCloneRight = !isCloneRight;
        }
        else if(clone.transform.position.x < -width)
        {
            clone.transform.position += Vector3.right * width * 2;
            isCloneRight = !isCloneRight;
        }
        clone.transform.position = transform.position + Vector3.right * width * (isCloneRight ? -1 : 1);

        clone.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
    }

}
