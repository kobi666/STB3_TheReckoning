using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test16 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2) transform.position + Vector2.left,
            moveSpeed * Time.deltaTime);
    }
}
