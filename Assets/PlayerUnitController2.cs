﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class PlayerUnitController2 : MonoBehaviour
{
//     public event Action unitDied;
//     UnitAnimationController animationController;

//     public event Action targetDied;
//     public void TargetDied() {
//         targetDied?.Invoke();
//     }

    
//     public void UnitDied() {
//         if (unitDied != null) {
//             unitDied.Invoke();
//         }
//     }
//     public IEnumerator AnnounceDeath() {
//         UnitDied();
//         yield break;
//     }
//     float attackCounter;
//     public float AttackCounter {
//         get => attackCounter;
//         set {
//             attackCounter = value;
//         }
//     }
//     public event Action onAttack;
//     public void OnAttack(){
//         if (onAttack != null){
//             onAttack.Invoke();
//         }
//     }

//     // public bool CanAttack {
//     //     get {
//     //         if (CurrentState == states.InBattle) {
//     //             if (Data.Target != null) {
//     //                 return true;
//     //             }
//     //             else {
//     //                 return false;
//     //             }
//     //         }
//     //         else {
//     //             return false;
//     //         }
//     //     }
//     // }
//     Collider2D[] collisions;

//     public event Action reachedTarget; 
//     public void ReachedTarget() {
//         if (reachedTarget != null) {
//             reachedTarget.Invoke();
//             }
//         }
        
    
//     // public bool TargetSlotIsEmpty {
//     //     get {
//     //         if (Data.Target == null) {
//     //             return true;
//     //         }
//     //         else {
//     //             return false;
//     //         }
//     //     }
//     // }

    

    
//     public EnemyUnitController2 TargetController { get => Data.Target?.GetComponent<EnemyUnitController2>() ?? null;}
//     public UnitData TargetData { get => TargetController.Data;}
//     public NormalUnitStates TargetStates { get => TargetController.states;}
//     public NormalUnitStates states {get => Data.unitType.states;}

//     UnitState CurrentState { get => SM.CurrentState;}

//     [SerializeField]
//     string currentStateName {
//         get => CurrentState.stateName;
//     }

//     public StateMachine TargetStateMachine { get => Data.Target.GetComponent<StateMachine>();}

//     public void StartDying() {
//         SM.StateChangeLocked = false;
//         SM.SetState(states.Death);
//     }

//     public IEnumerator PreDeathSequence() {
//         animationController.TriggerDeath();
//         yield return new WaitForSeconds(2.5f);
//         yield break;
//     }

    

//     public IEnumerator Die() {
//         Destroy(gameObject);
//         yield break;
//     }

//     public void OnTargetDeath() {
//         if (SM != null) {
//         SM.StateChangeLocked = false;
//         SM.SetState(states.Default, true);
        
//         }
//     }

//     public bool TargetStillExists() {
//         if (Data.Target != null) {
//             return true;
//         }
//         else {
//             return false;
//         }
//     }

    



//     public bool TargetIsInBattleWithThisUnit {
//         get {
//             if (Data.Target != null) {
//                 if (TargetController.Data.Target.name == gameObject.name) {
//                     return true;
//                 }
//                 else {
//                     return false;
//                 }
//             }
//             else {
//                 Debug.Log("When checking if i'm currently in battle with the target, the Target was null");
//                 return false;
//             }
//         }
//     }
//     public void SetEnemyTarget(GameObject target) {
//         if (TargetSlotIsEmpty) {
            
//             Data.Target = Utils.FindEnemyNearestToEndOfPath(gameObject, collisions);
// //            Debug.Log("Current Target name :" + Data.Target.name);
//             if (TargetController != null) {
//             //TargetController.unitDied += OnTargetDeath;

//             }
//         }
//     }

//     bool InBattle;
//     bool InBattleFunc() {
//         return InBattle;
//     }

//     public IEnumerator SetInBattleBooltoFalse() {
//         InBattle = false;
//         yield break;
//     }

//     public IEnumerator SetInBattleBooltoTrue() {
//         InBattle = true;
//         yield break;
//     }

    
    
//     public IEnumerator TargetCheck() {
//         while(InBattleFunc()) {
//             if (Data.Target == null) {
//                 {
//                     SM.StateChangeLocked = false;
//                     SM?.SetState(states.Default, true);
//                 }
//             }
//             yield return new WaitForSeconds(0.2f);
//         }
//     }

//     public void ReleaseTarget() {
//         SM.StateChangeLocked = false;
//         SM?.SetState(states.Default, true);
//         targetDied = null;
//     }

//     public void StartPreBattleSequnece(GameObject _dummyGO) {
//         if(SM.CurrentState == states.Default) {
//         SM.StateChangeLocked = false;
//         SM.SetState(states.PreBattle);
//         }
//     }

//     public void DetermineIfBattleOrDefault() {
//         if (Data.Target == null) {
//             SM.StateChangeLocked = false; 
//             SM.SetState(states.Default, true);
//         }
//         else {
//             if (CurrentState == states.PreBattle) {
//                 SM.StateChangeLocked = false;
//                 SM.SetState(states.InBattle);
//             }
//         }
//     }

//     public void Attack() {
//         if (Data.Target != null) {
//         animationController.TriggerAttack();
//         TargetController.LifeManager.DamageToUnit(UnityEngine.Random.Range(Data.DamageRange.min,Data.DamageRange.max), Data.damageType);
//         }
//     }

//     public void StartAttackRoutine() {
//         //StartCoroutine(Utils.IncrementCounterOverTimeAndInvokeAction(AttackCounter, 1.0f, Data.AttackRate / 10, CanAttack, OnAttack ));
//     } 

//     public IEnumerator StartAttack() {
//         StartAttackRoutine();
//         yield break;
//     }

//     public IEnumerator PreBattleSequence() {
//         if (Data.Target != null) {
//             if (TargetStateMachine.CurrentState == TargetStates.Default) {
//                 if (TargetData.Target == null) 
//                 {
//                     TargetData.Target = gameObject;
//                     TargetStateMachine.SetState(TargetStates.PreBattle);
//                 }
//                 else 
//                 {
//                     //PlaceHolder for joining Battle coroutine
//                 }
//             }
//         }
//         //yield return StartCoroutine(Utils.MoveToTargetWithEvent(gameObject, transform.position, Data.Target.transform.position, Data.speed, ReachedTarget));
//         yield return StartCoroutine(MoveToBattlePosition());
//     }

//     public IEnumerator CheckIfIShouldStayInBattle() {
//         if (Data.Target == null) {
//             SM.StateChangeLocked = false;
//             SM.SetState(states.Default);
//         }
//         yield break;
//     }

//     public IEnumerator MoveToBattlePosition() {
//         if (Data.Target != null) {
//             Vector2 TargetPosition = PlayerUnitUtils.FindPositionNextToUnit(gameObject, Data.Target);
//             yield return StartCoroutine(Utils.MoveToTargetWithEventAndCondition(gameObject, transform.position, TargetPosition, Data.speed, ReachedTarget, TargetStillExists()));
//         }
//     }

//     public bool StillInDefaultState() {
//             if (CurrentState == states.Default){
//                 return true;
//             }
//             else {
//                 return false;
//             }
//     }
    

//     public event Action<GameObject> enemyEnteredProximity;
//     public void EnemyEnteredProximity(GameObject enemy) {
//         if (enemyEnteredProximity != null) {
//             enemyEnteredProximity.Invoke(enemy);
//         }
//     }

//     private void OnTriggerStay2D(Collider2D other) {
//         if (other.gameObject.CompareTag("Enemy") && CurrentState == states.Default) {
//             EnemyEnteredProximity(other.gameObject);
//         }
//     }

    

    

    

//     public IEnumerator ReturnToSetPosition() {
//         yield return StartCoroutine(Utils.MoveToTargetWithCondition(gameObject, transform.position, Data.SetPosition, Data.speed, StillInDefaultState()));
//         yield break;
//     }

//     public IEnumerator EmptyCoroutine() {
//         yield break;
//     }

//     public IEnumerator WaitForSmallAmountOfTime() {
//         yield return new WaitForSeconds(0.01f);
//         yield break;
//     }

//     public void ChangeDisplayStats(int _hp) {
//         Data.HP = _hp;
//         Data.Armor = LifeManager.Armor;
//         Data.SpecialArmor = LifeManager.SpecialArmor;
//     }

//     private void Awake() {
//         animationController = GetComponent<UnitAnimationController>();
//         LifeManager = new UnitLifeManager(Data.HP, Data.Armor, Data.SpecialArmor, gameObject.tag, gameObject.name);
//         LifeManager.hp_changed += ChangeDisplayStats;
//         LifeManager.onUnitDeath += StartDying;
//     }

//     public bool CanAttackFunc() {
//         if (CurrentState == states.InBattle) {
                
//                 if (Data.Target != null) {
                    
//                     return true;
//                 }
//                 else {
//                     return false;
//                 }
//             }
//             else {
//                 return false;
//             }
//     }

//     void AttackInUpdate() {
//         if (CanAttackFunc()) {
//             if (AttackCounter >= 3.0f) {
//                 OnAttack();
//                 AttackCounter = 0.0f;
//             }
//         }
//         if (AttackCounter < 3.0f) {
//             attackCounter += Time.deltaTime * Data.AttackRate / 10;
//         }
//     }

//     private void Start() {
//         StartCoroutine(TargetCheck());
// //        Debug.Log("Parent Started");
//         // LifeManager = new UnitLifeManager(Data.HP, Data.Armor, Data.SpecialArmor);
//         // LifeManager.hp_changed += ChangeDisplayStats;
//         // LifeManager.onUnitDeath += StartDying;
//         Data.unitType = new UnitType(this, SM);
//         Data.SetPosition = transform.position;
//         onAttack += Attack;
        

//         reachedTarget += DetermineIfBattleOrDefault;
//         enemyEnteredProximity += SetEnemyTarget;
//         enemyEnteredProximity += StartPreBattleSequnece;

//         states.Default.OnEnterState += SetInBattleBooltoFalse;
//         states.Default.OnEnterState += ReturnToSetPosition;
//         //states.Default.OnEnterState += LookForNewEnemy;
//         states.Default.OnExitState += EmptyCoroutine;

//         states.PreBattle.OnEnterState += EmptyCoroutine;
//         states.PreBattle.OnEnterState += SetInBattleBooltoTrue;
//         states.PreBattle.OnEnterState += PreBattleSequence;
//         states.PreBattle.OnExitState += PreBattleSequence;

//         states.InBattle.OnEnterState += EmptyCoroutine;
//         states.InBattle.OnEnterState += StartAttack;
//         states.InBattle.OnEnterState += CheckIfIShouldStayInBattle;
//         states.InBattle.OnExitState += CheckIfIShouldStayInBattle;
        

//         states.Death.OnEnterState += AnnounceDeath;
//         states.Death.OnEnterState += Die;
//         states.Death.OnExitState += EmptyCoroutine;

//         SM.SetState(states.Default);
//     }

//     private void Update() {
//         AttackInUpdate();
//     }

    


    
}
