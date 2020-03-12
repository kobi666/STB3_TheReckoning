﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjects : MonoBehaviour
{
    
    public float GameTimeMultiplier = 1.0f;
    public float GameTime;
    // Start is called before the first frame update
    public static Transform PPH;
    public static StaticObjects instance;
    public float TowerSize;
    public Sprite TowerSprite;
    void Awake()
    {
        instance = this;
        GameTime = Time.time;
        TowerSize = TowerSprite.bounds.size.x;
    }

    private void Start() {
        PPH = GameObject.FindGameObjectWithTag("PPH").transform;
    }

    private void FixedUpdate() {
        GameTime = (Time.time * GameTimeMultiplier);
    }

    // Update is called once per frame
    
}
