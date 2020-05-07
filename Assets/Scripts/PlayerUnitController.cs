using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PlayerUnitController : UnitController
{
    public IEnumerator MovementCoroutine;
    public abstract bool CanEnterBattle();
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

    

    

    event Action onTargetEnteredRange;
    public void OnTargetEnteredRange(string targetName) {
        onTargetEnteredRange?.Invoke();
    }

    void checkOrSetSingleTarget() {
        if (isEnemyTargetSlotEmpty) {
                Data.EnemyTarget = TargetBank.FindSingleTargetNearestToEndOfSpline();
        }
    }

    void Test() {
        Debug.LogWarning("Target found, its name is : " + (Data.EnemyTarget?.name ?? "No Object"));
    }

    
    
    public abstract void LateStart();
    

    private void Start() {
        TargetBank.targetEnteredRange += OnTargetEnteredRange;
        onTargetEnteredRange += checkOrSetSingleTarget;
        States.JoinBattle.OnEnterState += OnEnterJoinBattle;
        States.JoinBattle.OnExitState += OnExitJoinBattle;
        
        

        LateStart();
    }
    
    
    

    
    
}
