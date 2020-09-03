using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class TowerPosIndicator : MonoBehaviour
{
    public TextMeshPro text;
    public SpriteRenderer SR;

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }
    
    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        SR.enabled = false;
    }


    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        text.text = "";
    }
}
