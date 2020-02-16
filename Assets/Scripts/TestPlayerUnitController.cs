using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerUnitController : PlayerUnitController
{
    // Start is called before the first frame update
    public Vector2 SetPosition;

     // PlayerUnitUtils.FindPositionNextToUnit(gameObject, EnemyTarget);
    public GameObject EnemyTarget;
    public OldStateMachine EnemyStateMachine { get => EnemyTarget.GetComponent<OldStateMachine>();}
    public EnemyUnitController EnemyController {get => EnemyTarget.GetComponent<EnemyUnitController>();}

    public bool ConditionTo_Start_BattleWithEnemy() {
        if (EnemyTarget != null) {
            if (EnemyController.TargetPlayerUnit == null) {
                if (EnemyStateMachine.CurrentState.stateName != "Death" ) {
                    return true;
                }
            }
        }
        return false;
    }



    public bool ConditionTo_Join_BattleWithEnemy() {
        if (EnemyTarget != null) {
            if (EnemyController.TargetPlayerUnit != null && EnemyController.TargetPlayerUnit.name != gameObject.name) {
                if (EnemyStateMachine.CurrentState.stateName != "Death" ) {
                    return true;
                }
            }
        }
        return false;
    }
    public void SetEnemyTarget() 
    {
        EnemyTarget = Utils.FindEnemyNearestToEndOfPath(gameObject, collisions);
        //Debug.Log("Player Unit Found Enemy " + EnemyTarget.name);
    }

    public void BattleWithEnemy() {
        
    }

    public void DecideIfToStartOrJoinBattleWithEnemy() {
        if (ConditionTo_Start_BattleWithEnemy()) {
            StartCoroutine(StartBattleWithEnemy());
        }
        else if (ConditionTo_Join_BattleWithEnemy()) {
            StartCoroutine(JoinBattleWithEnemy());
        }
    }

    

    public IEnumerator StartBattleWithEnemy() {
        // SM.SetState("PreBattle");
        SM.SetState(states.PreBattle);
        EnemyController.TargetPlayerUnit = gameObject;
        EnemyStateMachine.SetState("PreBattle");
        yield break;
    }

    public IEnumerator JoinBattleWithEnemy() {
        base.SM.SetState(states.PreBattle);
        yield break;
    }

    public IEnumerator MoveToBattlePosition() {
        Vector2 TargetPosition = PlayerUnitUtils.FindPositionNextToUnit(gameObject, EnemyTarget);
        yield return StartCoroutine(Utils.MoveToTarget(gameObject, transform.position, TargetPosition, speed));
    }
    
    private void Start() {
    
    SM = GetComponent<StateMachine>();
    SetPosition = transform.position;
    unitType = new UnitType(this);
    _onTargetCheck += SetEnemyTarget;
    }

    private void FixedUpdate() {
        
       

    }


}
