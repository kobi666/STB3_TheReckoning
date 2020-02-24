﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class PlayerUnitController2 : MonoBehaviour
{
    float attackCounter;
    public float AttackCounter {
        get => attackCounter;
        set {
            attackCounter = value;
        }
    }
    public event Action onAttack;
    public void OnAttack(){
        if (onAttack != null){
            onAttack.Invoke();
        }
    }

    public bool CanAttack {
        get {
            if (CurrentState == states.InBattle) {
                if (Data.Target != null) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }
    }
    Collider2D[] collisions;

    public UnitData Data;
    public event Action reachedTarget; 
    public void ReachedTarget() {
        if (reachedTarget != null) {
            reachedTarget.Invoke();
        }
    }
    public UnitLifeManager LifeManager;
    public StateMachine SM;
    public bool TargetSlotIsEmpty {
        get {
            if (Data.Target == null) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    
    public EnemyUnitController2 TargetController { get => Data.Target.GetComponent<EnemyUnitController2>();}
    public UnitData TargetData { get => TargetController.Data;}
    public NormalUnitStates TargetStates { get => TargetController.states;}
    public NormalUnitStates states {get => Data.unitType.states;}

    UnitState CurrentState { get => SM.CurrentState;}

    [SerializeField]
    string currentStateName {
        get => CurrentState.stateName;
    }

    public StateMachine TargetStateMachine { get => Data.Target.GetComponent<StateMachine>();}

    public void StartDying() {
        SM.StateChangeLocked = false;
        SM.SetState(states.Death);
    }

    public IEnumerator PreDeathSequence() {
        Debug.Log("OMG " + gameObject.name + " is dying...");
        yield break;
    }

    public IEnumerator Die() {
        Destroy(gameObject);
        yield break;
    }

    public void DetermineWhatToDoWhenTargetDies() {
        GameObject TentativeTarget = Utils.FindEnemyNearestToEndOfPath(gameObject, collisions);
        if (Data.Target == null || TargetStateMachine.CurrentState == TargetStates.Death) {
            if (Utils.FindEnemyNearestToEndOfPath(gameObject, collisions) == null) {
                SM.SetState(states.Default);
            }
            else {
                Data.Target = TentativeTarget;
                TargetController.LifeManager.onUnitDeath += DetermineWhatToDoWhenTargetDies;
                StartPreBattleSequnece(Data.Target);
            }
        }
    }

    public bool TargetIsInBattleWithThisUnit {
        get {
            if (Data.Target != null) {
                if (Data.Target.GetComponent<UnitData>().Target.name == gameObject.name) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                Debug.Log("When checking if i'm currently in battle with the target, the Target was null");
                return false;
            }
        }
    }
    public void SetEnemyTarget(GameObject target) {
        if (TargetSlotIsEmpty) {
            Data.Target = Utils.FindEnemyNearestToEndOfPath(gameObject, collisions);
            TargetController.LifeManager.onUnitDeath += DetermineWhatToDoWhenTargetDies;
        }
    }

    public void StartPreBattleSequnece(GameObject _dummyGO) {
        if(SM.CurrentState == states.Default) {
        SM.SetState(states.PreBattle);
        }
    }

    public void Battle() {
        if (Data.Target == null) { 
            SM.SetState(states.Default);
        }
        else {
            if (CurrentState == states.PreBattle) {
                SM.StateChangeLocked = false;
                SM.SetState(states.InBattle);
            }
        }
    }

    public void Attack() {
        if (Data.Target != null) {
        TargetController.LifeManager.DamageToUnit(UnityEngine.Random.Range(Data.DamageRange.min,Data.DamageRange.max), Data.damageType);
        }
    }

    public void StartAttackRoutine() {
        StartCoroutine(Utils.IncrementCounterOverTimeAndInvokeAction(AttackCounter, 1.0f, Data.AttackRate / 10, CanAttack, OnAttack ));
    } 

    public IEnumerator StartAttack() {
        StartAttackRoutine();
        yield break;
    }

    public IEnumerator PreBattleSequence() {
        if (Data.Target != null) {
            if (TargetStateMachine.CurrentState == TargetStates.Default) {
                if (TargetData.Target == null) 
                {
                    TargetData.Target = gameObject;
                    TargetStateMachine.SetState(TargetStates.PreBattle);
                }
                else 
                {
                    //PlaceHolder for joining Battle coroutine
                }
            }
        }
        //yield return StartCoroutine(Utils.MoveToTargetWithEvent(gameObject, transform.position, Data.Target.transform.position, Data.speed, ReachedTarget));
        yield return StartCoroutine(MoveToBattlePosition());
    }

    public IEnumerator MoveToBattlePosition() {
        Vector2 TargetPosition = PlayerUnitUtils.FindPositionNextToUnit(gameObject, Data.Target);
        yield return StartCoroutine(Utils.MoveToTargetWithEvent(gameObject, transform.position, TargetPosition, Data.speed, ReachedTarget));
    }
    

    public event Action<GameObject> enemyEnteredProximity;
    public void EnemyEnteredProximity(GameObject enemy) {
        if (enemyEnteredProximity != null) {
            enemyEnteredProximity.Invoke(enemy);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy") && CurrentState == states.Default) {
            EnemyEnteredProximity(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        
    }

    public IEnumerator EmptyCoroutine() {
        yield break;
    }

    public void ChangeDisplayStats() {
        Data.HP = LifeManager.HP;
        Data.Armor = LifeManager.Armor;
        Data.SpecialArmor = LifeManager.SpecialArmor;
    }

    private void Start() {
        LifeManager = new UnitLifeManager(Data.HP, Data.Armor, Data.SpecialArmor);
        LifeManager.hp_changed += ChangeDisplayStats;
        LifeManager.onUnitDeath += StartDying;
        SM = GetComponent<StateMachine>();
        Data.unitType = new UnitType(this, SM);
        Data.SetPosition = transform.position;
        onAttack += Attack;
        

        reachedTarget += Battle;
        enemyEnteredProximity += SetEnemyTarget;
        enemyEnteredProximity += StartPreBattleSequnece;

        states.Default.OnEnterState += EmptyCoroutine;
        states.Default.OnExitState += EmptyCoroutine;

        states.PreBattle.OnEnterState += EmptyCoroutine;
        states.PreBattle.OnEnterState += PreBattleSequence;
        states.PreBattle.OnExitState += EmptyCoroutine;

        states.InBattle.OnEnterState += EmptyCoroutine;
        states.InBattle.OnEnterState += StartAttack;
        states.InBattle.OnExitState += EmptyCoroutine;

        states.Death.OnEnterState += EmptyCoroutine;
        states.Death.OnEnterState += PreDeathSequence;
        states.Death.OnEnterState += Die;
        states.Death.OnExitState += EmptyCoroutine;

        SM.SetState(states.Default);
    }

    

    private void FixedUpdate() {
        
    }
    
}
