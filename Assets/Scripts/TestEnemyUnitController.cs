using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestEnemyUnitController : EnemyUnitController
{
    event Action<GameObject> targetUnitSet;
    event Action targetUnitReleased;
    public void TargetUnitReleased(GameObject _playerUnit) {
        if (targetUnitReleased != null) {
            targetUnitReleased.Invoke();
        }
    }
    public void TargetUnitSet(GameObject _playerUnit) {
        if (targetUnitSet != null){
            targetUnitSet.Invoke(_playerUnit);
        }
    }
    public GameObject TargetPlayerUnit {
        get => targetPlayerUnit;
        set {
            if (value != null) {
            TargetUnitSet(value);
            }
            else {
            TargetUnitReleased(value);
            }
        }
    }

    public void WaitForTargetToEnterBattlePosition(GameObject _playerUnit) {
        SM.SetState(states.PreBattle);
        _playerUnit.GetComponent<PlayerUnitController>().reachedBattlePosition += StartBattle;
    }

    public void SetTargetUnit(GameObject _targetUnit) {
        targetPlayerUnit = _targetUnit;
    }

    public void LeaveBattle(GameObject _nullPlayerUnit) {
        if (targetPlayerUnit != null) {
            // Release Battle Function from target player unit event
            targetPlayerUnit.GetComponent<PlayerUnitController>().reachedBattlePosition -= StartBattle;
            targetPlayerUnit = _nullPlayerUnit;
        }
        else {
            targetPlayerUnit = _nullPlayerUnit;
        }
        SM.SetState(states.Default);
    }

    public IEnumerator Die() {
        Destroy(gameObject);
        yield break;
    }

    public IEnumerator ReleaseInBattleFunctionForExistingTargetUnit() {
        if (targetPlayerUnit != null) {
            // Release Battle Function from target player unit event
            targetPlayerUnit.GetComponent<PlayerUnitController>().reachedBattlePosition -= StartBattle;
        }
        yield break;
    }

    public void StartBattle() {
        SM.SetState(states.InBattle);
    }

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

    

    IEnumerator EmptyRoutine() {
        yield break;
    }

    

    
    
    

    private void Start() {
        unitType = new UnitType(this, SM);
        UnitLife._onUnitDeath += UnitDeath;
        targetUnitSet += WaitForTargetToEnterBattlePosition;

        states.Default.OnExitState += EmptyRoutine;
        states.Default.OnEnterState += ReturnToWalkPath;

        states.Death.OnEnterState += ReleaseInBattleFunctionForExistingTargetUnit;
        states.Death.OnEnterState += Die;
        
        states.PreBattle.OnEnterState += StopWalkingOnPath;
        states.PreBattle.OnExitState += EmptyRoutine;

        states.InBattle.OnEnterState += EmptyRoutine;
        states.InBattle.OnExitState += EmptyRoutine;
    }

    
    
}
