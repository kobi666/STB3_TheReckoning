using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class UnitController : MonoBehaviour
{
    
    public bool IsTargetable {
         get {
            if(CurrentState == States.Death) { 
                return false;
             }
            else {return true;}
         }
     }
    [SerializeField]
    public UnitLifeManager LifeManager;
    public NormalUnitStates States;
    public BezierSolution.UnitWalker Walker;
    public EnemyTargetBank TargetBank;
    public StateMachine SM;

    public SpriteRenderer SR;

    [SerializeField]
    public UnitData Data;

    public UnitState CurrentState {
        get => SM?.CurrentState ?? null;
    }

    public event Action<string> onTargetIdentified;
    public void OnTargetIdentified(string targetName) {
        onTargetIdentified?.Invoke(targetName);
    }
    
    public event Action onSelfDeath;
    public void OnSelfDeath() {
        onSelfDeath?.Invoke();
    }

    void AnnounceDeath() {
        DeathManager.instance.OnUnitDeath(tag, name);
    }

    

    public abstract IEnumerator OnEnterPreBattle();
    public abstract IEnumerator OnExitPreBattle();
    public abstract IEnumerator OnEnterInBattle();
    public abstract IEnumerator OnExitInBattle();
    public abstract IEnumerator OnEnterDefault();
    public abstract IEnumerator OnExitDefault();
    public abstract IEnumerator OnEnterPostBattle();
    public abstract IEnumerator OnExitPostBattle();
    public IEnumerator OnEnterInitialState() {
        yield break;
    }
    public IEnumerator OnExitInitialState() {
        yield break;
    }
    public abstract void Test2();
    
    
    private void Awake() {
        Test2();
        LifeManager.onUnitDeath += AnnounceDeath;
        TargetBank = GetComponentInChildren<EnemyTargetBank>();
        SM = GetComponent<StateMachine>() ?? null;
        States = new NormalUnitStates(this);
        Walker = GetComponent<BezierSolution.UnitWalker>() ?? null;
        Data = new UnitData();
        SR = GetComponent<SpriteRenderer>() ?? null;

        States.InitialState.OnEnterState += OnEnterInitialState;
        States.InitialState.OnExitState += OnExitInitialState;
        States.Default.OnEnterState += OnEnterDefault;
        States.Default.OnExitState += OnExitDefault;
        States.PreBattle.OnEnterState += OnEnterPreBattle;
        States.PreBattle.OnExitState += OnExitPreBattle;
        States.InDirectBattle.OnEnterState += OnEnterInBattle;
        States.InDirectBattle.OnExitState += OnExitInBattle;
        States.PostBattle.OnEnterState += OnEnterPostBattle;
        States.PostBattle.OnExitState += OnExitPostBattle;
        SM.CurrentState = States.InitialState;
    }

    

    

    
}
