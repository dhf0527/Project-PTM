using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [Range(-1.0f, 1.0f)]
    public float speed = 0.5f;
    private Vector2 offset = Vector2.zero;
    private Material material;
    
    
    
    void Start()
    {
        material = GetComponent<Renderer>().material;
        
    }

    // Update is called once per frame
    void Update()
    {
        offset.x += speed * Time.deltaTime;
        material.mainTextureOffset = offset;
    }
}
