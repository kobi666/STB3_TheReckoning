using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitController2 : MonoBehaviour
{
    public float AttackCounter;
    public UnitType unitType;
    public BezierSolution.UnitWalker Walker;
    public UnitData Data;
    public UnitLifeManager LifeManager;
    public StateMachine SM;
    public NormalUnitStates states {get => unitType.states;}
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



    
    // Start is called before the first frame update
    void Start()
    {
        Walker = GetComponent<BezierSolution.UnitWalker>();
        SM = GetComponent<StateMachine>();
        unitType = new UnitType(this, SM);
        LifeManager = new UnitLifeManager(Data.HP, Data.Armor, Data.SpecialArmor);
        LifeManager.hp_changed += ChangeDisplayStats;

        states.Default.OnEnterState += EmptyCoroutine;
        states.Default.OnExitState += EmptyCoroutine;

        states.PreBattle.OnEnterState += EmptyCoroutine;
        states.PreBattle.OnEnterState += StartBattleSequence;
        states.PreBattle.OnExitState += EmptyCoroutine;

        states.InBattle.OnEnterState += EmptyCoroutine;
        states.InBattle.OnExitState += EmptyCoroutine;

        states.Death.OnEnterState += EmptyCoroutine;
        states.Death.OnExitState += EmptyCoroutine;

        SM.SetState(states.Default);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
