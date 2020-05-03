using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyUnitController2 : UnitController
{
    Animation DeathAnimation;
    public event Action unitDied;
    public void UnitDied() {
        if (unitDied != null) {
            unitDied.Invoke();
        }
    }

    
    public IEnumerator AnnounceDeath() {
        UnitDied();
        yield break;
    }

    public IEnumerator ReturnToWalkingOnPath() {
        Walker.ReturnWalking();
        animationController.TriggerWalking();
        yield break;
    }

    public void OnTargetDeath() {
        if (SM != null) {
        SM.StateChangeLocked = false;
        SM.SetState(states.Default, true);
        }
    }

    AnimationTest animationController;
    UnitState CurrentState { get => SM.CurrentState;}
    public event Action onAttack;
    public void OnAttack(){
        if (onAttack != null){
    //            Debug.LogWarning("Attack event invoked");
            onAttack.Invoke();
        }
    }

    public IEnumerator UnSubscribeToTargetDeath() {
         if (Data.Target != null) {
            if (TargetController.TargetIsInBattleWithThisUnit) {
            TargetController.unitDied -= OnTargetDeath;
            }
        }
        yield break;
    }

    public IEnumerator SubscribeToTargetDeath() {
        if (Data.Target != null) {
            if (TargetController.TargetIsInBattleWithThisUnit) {
            TargetController.unitDied += OnTargetDeath;
            }
        }
        
        yield break;
    }

    public bool IsTargetable {
        get {
            if(CurrentState == states.Death) { 
                return false;
            }
            else {return true;}
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

    public bool CanAttackFunc() {
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
    private float attackCounter;
    public float AttackCounter {
        get => attackCounter;
        set {
            attackCounter = value;
        }
    }
    public UnitType unitType;
    //public BezierSolution.UnitWalker Walker;
    public NormalUnitStates states {get => Data.unitType.states;}
    public PlayerUnitController2 TargetController { get => Data.Target.GetComponent<PlayerUnitController2>();}

    public void ChangeDisplayStats(int hp) {
        Data.HP = hp;
        Data.Armor = LifeManager.Armor;
        Data.SpecialArmor = LifeManager.SpecialArmor;
    }

    string currentStateName {
        get => SM.CurrentState.stateName;
    }
    public StateMachine TargetStateMachine { get => Data.Target.GetComponent<StateMachine>();}
    public IEnumerator EmptyCoroutine() {
//        Debug.Log("Currnet State : " + currentStateName);
        yield break;
    }

    public IEnumerator StartBattleSequence() {
        Walker.StopWalking();
        animationController.TriggerWalking();
        if (Data.Target != null) {
            TargetController.reachedTarget += Battle;
        }
        else {
            SM.SetState(states.Default);
        }
        yield break;
    }

    public void Battle() {
        if (Data.Target == null) {
            if (CurrentState.IsFinalState == false) {
                SM.StateChangeLocked = false;        
                SM.SetState(states.Default);
            }  
        }
        else {
                SM.StateChangeLocked = false;
                SM.SetState(states.InBattle);
            }
    }

    bool attackLock = false;

    public void Attack() {
       // Debug.Log("Attacking " + Data.Target.name);
       //Debug.LogWarning("Attack triggered");
       if (Data.Target != null) {
//        Debug.LogWarning("Attack animation Triggered");
        animationController.TriggerAttack();
        TargetController.LifeManager.DamageToUnit(UnityEngine.Random.Range(Data.DamageRange.min,Data.DamageRange.max), Data.damageType);
        }
    }

    public void StartAttackRoutine() {
        //StartCoroutine(Utils.IncrementCounterOverTimeAndInvokeAction(AttackCounter, 3.0f, Data.AttackRate / 10, CanAttackFunc(), OnAttack ));
    }

    void AttackInUpdate() {
        if (CanAttackFunc()) {
            if (AttackCounter >= 3.0f) {
                OnAttack();
                AttackCounter = 0.0f;
            }
        }
        if (AttackCounter < 3.0f) {
            attackCounter += Time.deltaTime * Data.AttackRate / 10;
        }
    }

    public IEnumerator StartAttack() {
        StartAttackRoutine();
        yield break;
    }

    public void StartDying() {
        SM.StateChangeLocked = false;
        SM.SetState(states.Death);
        
    }

    

    public IEnumerator PreDeathSequence() {
        animationController.TriggerDeath();
        yield return new WaitForSeconds(1.5f);
        yield break;
    }

    public IEnumerator Die() {
        if (Data.Target != null) {
            TargetController.reachedTarget -= Battle;
        }
        Destroy(gameObject);
        yield break;
    }


    
    // Start is called before the first frame update
    void Start()
    {
        animationController = GetComponent<AnimationTest>();
        //Walker = GetComponent<BezierSolution.UnitWalker>();
        Data.unitType = new UnitType(this, SM);
        LifeManager = new UnitLifeManager(Data.HP, Data.Armor, Data.SpecialArmor, gameObject.tag, gameObject.name);
        LifeManager.hp_changed += ChangeDisplayStats;
        LifeManager.onUnitDeath += StartDying;
        onAttack += Attack;

        states.Default.OnEnterState += ReturnToWalkingOnPath;
        
        states.Default.OnExitState += EmptyCoroutine;

        states.PreBattle.OnEnterState += EmptyCoroutine;
        states.PreBattle.OnEnterState += StartBattleSequence;
        states.PreBattle.OnExitState += EmptyCoroutine;

        states.InBattle.OnEnterState += EmptyCoroutine;
        states.InBattle.OnEnterState += SubscribeToTargetDeath;
        states.InBattle.OnEnterState += StartAttack;
        states.InBattle.OnExitState += UnSubscribeToTargetDeath;

        states.Death.OnEnterState += UnSubscribeToTargetDeath;
        states.Death.OnEnterState += AnnounceDeath;
        states.Death.OnEnterState += PreDeathSequence;
        states.Death.OnEnterState += Die;
        states.Death.OnExitState += EmptyCoroutine;

        SM.SetState(states.Default);
    }

    // Update is called once per frame
    void Update()
    {
        AttackInUpdate();
    }
}
