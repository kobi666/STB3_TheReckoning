using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyUnitController2 : MonoBehaviour
{
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
    UnitState CurrentState { get => SM.CurrentState;}
    public event Action onAttack;
    public void OnAttack(){
        if (onAttack != null){
            onAttack.Invoke();
        }
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
    private float attackCounter;
    public float AttackCounter {
        get => attackCounter;
        set {
            attackCounter = value;
        }
    }
    public UnitType unitType;
    public BezierSolution.UnitWalker Walker;
    public UnitData Data;
    public UnitLifeManager LifeManager;
    public StateMachine SM;
    public NormalUnitStates states {get => Data.unitType.states;}
    public PlayerUnitController2 TargetController { get => Data.Target.GetComponent<PlayerUnitController2>();}

    public void ChangeDisplayStats() {
        Data.HP = LifeManager.HP;
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
            SM.SetState(states.Default);
        }
        else {
            SM.SetState(states.InBattle);
        }
    }

    public void Attack() {
       // Debug.Log("Attacking " + Data.Target.name);
        TargetController.LifeManager.DamageToUnit(UnityEngine.Random.Range(Data.DamageRange.min,Data.DamageRange.max), Data.damageType);
    }

    public void StartAttackRoutine() {
        StartCoroutine(Utils.IncrementCounterOverTimeAndInvokeAction(AttackCounter, 1.0f, Data.AttackRate / 10, CanAttack, OnAttack ));
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
        Debug.Log("OMG " + gameObject.name + " is dying...");
        yield break;
    }

    public IEnumerator Die() {
        Destroy(gameObject);
        yield break;
    }


    
    // Start is called before the first frame update
    void Start()
    {
        Walker = GetComponent<BezierSolution.UnitWalker>();
        SM = GetComponent<StateMachine>();
        Data.unitType = new UnitType(this, SM);
        LifeManager = new UnitLifeManager(Data.HP, Data.Armor, Data.SpecialArmor);
        LifeManager.hp_changed += ChangeDisplayStats;
        LifeManager.onUnitDeath += StartDying;
        onAttack += Attack;

        states.Default.OnEnterState += EmptyCoroutine;
        states.Default.OnExitState += EmptyCoroutine;

        states.PreBattle.OnEnterState += EmptyCoroutine;
        states.PreBattle.OnEnterState += StartBattleSequence;
        states.PreBattle.OnExitState += EmptyCoroutine;

        states.InBattle.OnEnterState += EmptyCoroutine;
        states.InBattle.OnEnterState += StartAttack;
        states.InBattle.OnExitState += EmptyCoroutine;

        states.Death.OnEnterState += AnnounceDeath;
        states.Death.OnEnterState += PreDeathSequence;
        states.Death.OnEnterState += Die;
        states.Death.OnExitState += EmptyCoroutine;

        SM.SetState(states.Default);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
