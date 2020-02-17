using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestPlayerUnitController : PlayerUnitController
{
    // Start is called before the first frame update
    event Action<GameObject> enemyTargetSet;
    public void EnemyTargetSet(GameObject _enemy) {
        if (enemyTargetSet != null) {
            enemyTargetSet.Invoke(_enemy);
        }
    }
    event Action<GameObject> enemyTargetReleased;
    public void EnemyTargetReleased(GameObject _enemy) {
        if (enemyTargetReleased != null) {
            enemyTargetReleased.Invoke(_enemy);
        }
    }
    public Damage_Type damageType = new Damage_Type("normal");
    event Action onAttack;
    public void OnAttack() {
        if (onAttack != null) {
            onAttack.Invoke();
        }
    }
    float attackCounter;
    float AttackCounter {
        get => attackCounter;
        set {
            attackCounter = value;
            if (attackCounter >= 1.0f && SM.CurrentState.stateName == states.InBattle.stateName ) {
              OnAttack();
              attackCounter = 0.0f;  
            }
        }
    }
    public Vector2 SetPosition;

     // PlayerUnitUtils.FindPositionNextToUnit(gameObject, EnemyTarget);
     [SerializeField]
    public GameObject enemyTarget;
    public GameObject EnemyTarget {
        get => enemyTarget ;
        set {
            
            if (value != null) {
            EnemyTargetSet(value);
            }
            else {
            EnemyTargetReleased(value);
            }
        }
        }
    public OldStateMachine EnemyStateMachine { get => EnemyTarget.GetComponent<OldStateMachine>();}
    public EnemyUnitController EnemyController {get => EnemyTarget.GetComponent<EnemyUnitController>();}
    public UnitLifeManager EnemyLife {get => EnemyTarget.GetComponent<UnitLifeManager>();}

    public void LeaveBattle(GameObject _enemy) {
        enemyTarget = _enemy;
        if (_enemy != null) {
            if (_enemy.GetComponent<EnemyUnitController>().targetPlayerUnit.name == gameObject.name) {
                _enemy.GetComponent<EnemyUnitController>().targetPlayerUnit = null;
            }
        }
        SM.SetState(states.Default);
    }

    public void StartOrJoinBattle(GameObject _enemy) {
        if (_enemy.GetComponent<EnemyUnitController>().InBattleWithUnit() == false) {
            enemyTarget = _enemy;
            EnemyController.targetPlayerUnit = gameObject;
            SM.SetState(states.PreBattle);
        }
        else {
            enemyTarget = _enemy;
            SM.SetState(states.PreBattle);
        }
    }

    



    

    
    public void SetEnemyTarget() 
    {
        if (EnemyTarget == null) {
        EnemyTarget = Utils.FindEnemyNearestToEndOfPath(gameObject, collisions);
        //Debug.Log("Player Unit Found Enemy " + EnemyTarget.name);
        }
    }

    public void HitEnemy() {
        EnemyController.UnitLife.DamageToUnit(UnityEngine.Random.Range(DamageRange.min, DamageRange.max), damageType);
    }


    public IEnumerator EmptyRoutine() {
        yield break;
    }

    public IEnumerator MoveToBattlePosition() {
        Vector2 TargetPosition = PlayerUnitUtils.FindPositionNextToUnit(gameObject, EnemyTarget);
        yield return StartCoroutine(Utils.MoveToTarget(gameObject, transform.position, TargetPosition, speed));
    }

    public IEnumerator AnnounceReachBattlePosition() {
        ReachedBattlePosition();
        yield break;
    }

    public IEnumerator SetBattleState() {
        SM.SetState(states.InBattle);
        yield break;
    }

    

    public IEnumerator ReturnToSetPosition () {
        yield return StartCoroutine(Utils.MoveToTarget(gameObject, transform.position, SetPosition, speed));
    }
    
    private void Start() {
    SetPosition = transform.position;
    unitType = new UnitType(this, SM);
    onTargetCheck += SetEnemyTarget;

    states.PreBattle.OnEnterState += MoveToBattlePosition;
    states.PreBattle.OnEnterState += AnnounceReachBattlePosition;
    states.PreBattle.OnEnterState += SetBattleState;
    states.PreBattle.OnExitState += EmptyRoutine;

    states.Default.OnEnterState += ReturnToSetPosition;
    states.Default.OnExitState += EmptyRoutine;

    states.InBattle.OnEnterState += EmptyRoutine;
    states.InBattle.OnExitState += EmptyRoutine;
    

    onAttack += HitEnemy;
    enemyTargetSet += StartOrJoinBattle;
    enemyTargetReleased += LeaveBattle;
    onTargetCheck += SetEnemyTarget;
    }

    private void FixedUpdate() {
        if (AttackCounter < 1.0f) {
            AttackCounter += (Time.fixedDeltaTime * AttackRate) / 10;
        }

    }


}
