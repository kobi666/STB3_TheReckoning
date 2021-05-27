using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEdenDetection : MonoBehaviour
{
    private BoxCollider2D BoxCollider2D;
    private float _radius;
    void Start()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
        _radius = Mathf.Sqrt(2f * BoxCollider2D.size.x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
