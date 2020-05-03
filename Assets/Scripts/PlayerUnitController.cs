using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerUnitController : UnitController
{
    // Start is called before the first frame update
    public bool isEnemyTargetSlotEmpty {
        get {
            if (Data.EnemyTarget == null) {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    void checkOrSetSingleTarget(string targetName) {
        if (isEnemyTargetSlotEmpty) {
            if (Data.EnemyTarget.name != targetName) {
                Data.EnemyTarget = TargetBank.FindSingleTargetNearestToEndOfSpline();
            }
        }
    }

    void Test(string s) {
        Debug.LogWarning("Target found, its name is : " + s);
    }

    

    

    private void Start() {
        TargetBank.targetEnteredRange += checkOrSetSingleTarget;
        TargetBank.targetEnteredRange += Test;
    }
    
    
    

    // Update is called once per frame
    
}
