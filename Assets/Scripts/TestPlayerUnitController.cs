using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerUnitController : PlayerUnitController
{
    // Start is called before the first frame update
    
    UnitType unitType;

     // PlayerUnitUtils.FindPositionNextToUnit(gameObject, EnemyTarget);
    public GameObject EnemyTarget;
    public StateMachine EnemyStateMachine { get => EnemyTarget.GetComponent<StateMachine>();}
    Collider2D[] collisions;
    public void SetEnemyTarget() 
    {
        EnemyTarget = Utils.FindEnemyNearestToEndOfPath(gameObject, collisions);
        //Debug.Log("Player Unit Found Enemy " + EnemyTarget.name);
    }

    public void Battle() {
        
    }

    

    public IEnumerator StartBattleWithEnemy() {
        SM.SetState("PreBattle");
        EnemyStateMachine.SetState("PreBattle");
        yield break;
    }

    public IEnumerator JoinBattleWithEnemy() {
        SM.SetState("PreBattle");
        yield break;
    }
    
    private void Start() {
    unitType = UnitTypes.NormalEnemy(this);
    SM = GetComponent<StateMachine>();
    SM.InitilizeStateMachine(unitType.States);
    _onTargetCheck += SetEnemyTarget;
    }

    private void FixedUpdate() {
        
       

    }


}
