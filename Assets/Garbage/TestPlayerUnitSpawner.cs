﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerUnitSpawner : PlayerUnitSpawner
{
    private void Update() {
        for (int i = 0; i < Data.SpawnerData.maxUnits ; i++) {
            Debug.DrawLine(RallyPoint.transform.position, GetRallyPoint(i));
        }

    }


    protected void Start()
    {
        base.Start();
        
        
    }
    public override void PostAwake() {

    }

    public override void PostStart() {
        
    }
}