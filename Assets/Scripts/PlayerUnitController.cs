using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PlayerUnitController : UnitController
{
    public IEnumerator MovementCoroutine;
    public abstract bool CanEnterNewBattle();
    public EnemyUnitController Target { get => Data.EnemyTarget ?? null;}
    
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

    

    public abstract IEnumerator OnEnterJoinBattle();
    public abstract IEnumerator OnExitJoinBattle();

    

    

    
    public abstract void OnTargetEnteredRange(EnemyUnitController ec);
        
    

    // void checkOrSetSingleTarget() {
    //     if (isEnemyTargetSlotEmpty) {
    //             Data.EnemyTarget = TargetBank.FindSingleTargetNearestToEndOfSpline();
    //     }
    // }

    void Test() {
        Debug.LogWarning("Target found, its name is : " + (Data.EnemyTarget?.name ?? "No Object"));
    }

    
    
    public abstract void LateStart();
    

    private void Start() {
        TargetBank.targetEnteredRange += OnTargetEnteredRange;
        States.JoinBattle.OnEnterState += OnEnterJoinBattle;
        States.JoinBattle.OnExitState += OnExitJoinBattle;
        
        

        LateStart();
    }
    
    
    

    
    
}
