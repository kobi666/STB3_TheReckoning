using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestEnemyUnitController : EnemyUnitController
{

    public IEnumerator test() {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("Success!");
    }

    public virtual void GoIntoBattleState() {
        SM.SetState(states.InBattle);
    }

    IEnumerator ReturnToWalkPath() {
        walker.IsWalking = true;
        yield break;
    }

    IEnumerator StopWalkingOnPath() {
        walker.IsWalking = false;
        yield break;
    }

    IEnumerator StartBattle() {
        Debug.Log("I'm in battle!");
        yield break;
    }

    

    
    
    

    private void Start() {
        unitType = UnitTypes.NormalEnemy(this);
        UnitLife._onUnitDeath += UnitDeath;
        states.Death.OnEnterState += Die;
         // SM.States["PreBattle"].OnEnterState += StopWalkingOnPath;
        states.Default.OnEnterState += ReturnToWalkPath;
        states.PreBattle.OnEnterState += StopWalkingOnPath;
        states.InBattle.OnEnterState += StartBattle;
    }

    
    
}
