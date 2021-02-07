using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Animancer;
using Sirenix.OdinInspector;

[RequireComponent(typeof(UnitAnimationController))]

[Serializable]
public abstract class UnitController : MonoBehaviour,IActiveObject<UnitController>,IHasEffects
{
    [TypeFilter("GetEffects")][SerializeReference]
    public List<Effect> AttackEffects = new List<Effect>();
    
    
    
    private static IEnumerable<Type> GetEffects()
    {
        var q = typeof(Effect).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(Effect))); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
    
    public Type QueueableType {get;set;}
    public virtual void OnEnqueue() {}
    public ActiveObjectPool<UnitController> activePool;
    public ActiveObjectPool<UnitController> ActivePool {get => activePool;set { activePool = value;}}

    public float Proximity {
        get => Walker?.ProximityToEndOfSplineFunc() ?? 90210.0f;
    }
    
    bool spriteXDirection;
    public bool SpriteXDirection {
        get => spriteXDirection;
        set {
            if (value == true) {
                SR.flipX = true;
            }
            if (value == false) {
                SR.flipX = false;
            }
            spriteXDirection = value;
        }
    }

    public void SetXdirection(bool xdirection) {
        SpriteXDirection = xdirection;
    }
    public UnitAnimationController animationController;
    Rigidbody2D body;
    public Collider2D UnitCollider;
    public AnimancerComponent Animancer;
    public abstract bool IsTargetable();
    [SerializeField]
    public UnitLifeManager LifeManager;
    public NormalUnitStates States;
    public BezierSolution.UnitWalker Walker;
    public EnemyTargetBank TargetBank;
    public StateMachine SM;

    public SpriteRenderer SR;

    [SerializeField]
    public UnitDataLegacy dataLegacy;

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
        DeathManager.Instance.OnUnitDeath(tag, name);
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
        gameObject.SetActive(false);
        
    }
    
    private void Awake() {
        ActivePool = GameObjectPool.Instance.ActiveUnitPool;
        animationController = GetComponent<UnitAnimationController>() ?? null;
        UnitCollider = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        Animancer = GetComponent<AnimancerComponent>() ?? null;
        LifeManager.HP = dataLegacy.HP;
        TargetBank = GetComponentInChildren<EnemyTargetBank>() ?? null;
        SM = GetComponent<StateMachine>() ?? null;
        States = new NormalUnitStates(this);
        Walker = GetComponent<BezierSolution.UnitWalker>() ?? null;
        if (Walker != null) {
            Walker.xDirectionChanged += SetXdirection;
        }
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

    protected void OnEnable() {
        activePool.AddObjectToActiveObjectPool(this);
        UnitCollider.enabled = true;
    }

    private void OnDisable() {
        GameObjectPool.Instance.RemoveObjectFromAllPools(name,name);
    }


    public List<Effect> GetEffectList()
    {
        throw new NotImplementedException();
    }

    public void UpdateEffect(Effect ef, List<Effect> appliedEffects)
    {
        throw new NotImplementedException();
    }
}
