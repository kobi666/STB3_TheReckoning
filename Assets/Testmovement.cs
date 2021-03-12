using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testmovement : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogError(other.name + " entered");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.LogError(other.gameObject.name + " entered");
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        Debug.LogError(other.gameObject.name + " entered");
    }

    public float speed;

    private Rigidbody2D Rigidbody2D;

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.LogError(other.name + " Exited");
    }

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, (Vector2)transform.position + Vector2.right, speed * Time.deltaTime);
    }
}
