using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript : MonoBehaviour
{
    void Update()
    {
        transform.position = (Vector2)transform.position + Vector2.right * (Time.deltaTime * 4) ; 
    }

}
