﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerUnitController : PlayerUnitController
{
    // Start is called before the first frame update
    


    private void Awake() {
        _onTargetCheck += SetEnemyTarget;
    }


}