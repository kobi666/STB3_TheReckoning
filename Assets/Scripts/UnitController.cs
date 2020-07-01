using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Animancer;

public abstract class UnitController : MonoBehaviour
{
    
    Rigidbody2D body;
    public Collider2D UnitCollider;
    public AnimancerComponent Animancer;
    public abstract void OnAttack();
    public abstract bool IsTargetable();
    [SerializeField]
    public UnitLifeManager LifeManager;
    public NormalUnitStates States;
    public BezierSolution.UnitWalker Walker;
    public EnemyTargetBank TargetBank;
    public StateMachine SM;

    public SpriteRenderer SR;

    [SerializeField]
    public UnitData Data;

    public ObjectState CurrentState {
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
    public abstract IEnumerator OnEnterInDirectBattle();
    public abstract IEnumerator OnExitInDirectBattle();
    public abstract IEnumerator OnEnterDefault();
    public abstract IEnumerator OnExitDefault();
    public abstract IEnumerator OnEnterPostBattle();
    public abstract IEnumerator OnExitPostBattle();
    public abstract IEnumerator OnEnterDeath();
    public abstract IEnumerator OnExitDeath();
    public virtual IEnumerator OnEnterInitialState() {
        yield break;
    }
    public virtual IEnumerator OnExitInitialState() {
        yield break;
    }
    public abstract void Test2();
    
    /////TESTTTTT DELETE MEEEE
    public IEnumerator DieAfterTwoSeconds() {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
    
    private void Awake() {
        UnitCollider = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        Animancer = GetComponent<AnimancerComponent>() ?? null;
        LifeManager.HP = Data.HP;
        TargetBank = GetComponentInChildren<EnemyTargetBank>() ?? null;
        SM = GetComponent<StateMachine>() ?? null;
        States = new NormalUnitStates(this);
        Walker = GetComponent<BezierSolution.UnitWalker>() ?? null;
        SR = GetComponent<SpriteRenderer>() ?? null;
        States.InitialState.OnEnterState += OnEnterInitialState;
        States.InitialState.OnExitState += OnExitInitialState;
        States.Default.OnEnterState += OnEnterDefault;
        States.Default.OnExitState += OnExitDefault;
        States.PreBattle.OnEnterState += OnEnterPreBattle;
        States.PreBattle.OnExitState += OnExitPreBattle;
        States.InDirectBattle.OnEnterState += OnEnterInDirectBattle;
        States.InDirectBattle.OnExitState += OnExitInDirectBattle;
        States.PostBattle.OnEnterState += OnEnterPostBattle;
        States.PostBattle.OnExitState += OnExitPostBattle;
        States.Death.OnEnterState += OnEnterDeath;
        States.Death.OnExitState += OnExitDeath;
        SM.CurrentState = States.InitialState;
        LifeManager.onUnitDeath += AnnounceDeath;
        LifeManager.onUnitDeath += delegate {SM.SetState(States.Death);};
         
    }

    private void OnEnable() {
        UnitCollider.enabled = true;
    }

    private void OnDisable() {
        
    }

    

    
}
