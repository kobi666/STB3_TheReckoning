using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using MyBox;
using System.Threading.Tasks;
using Sirenix;
using Sirenix.OdinInspector;
using Sirenix.Serialization;


[RequireComponent(typeof(EffectableTargetBank))]
public class GenericWeaponController : TowerComponent
{
    [ValueDropdown("valueList")]
    [HideLabel]
    [BoxGroup]
    [SerializeField]
    protected int WeaponType;

    public bool RotatingTurret;
    private GenericRotator GenericRotator;
    private string cachedTargetName;
    
    private static ValueDropdownList<int> valueList = new ValueDropdownList<int>()
    {
        {"Projectile Effect", 0},
        {"Beam Effect",4},
        {"Area Of Effect", 1}
    };

    [ShowIf("WeaponType", 0)][BoxGroup]
    public ProjectileAttack projectileAttack;

    void FireProjectileAttack()
    {
        projectileAttack.AttackProperties.Attack(Target.Effectable,Target.transform.position);
    }

    void FireSplineAttack()
    {
        SplineAttack.Attack(Target.Effectable, Target.transform.position);
    }

    void FireAOEAttack()
    {
        AoeAttack.Attack(Target.Effectable,Target.transform.position);
    }




    [ShowIf("WeaponType", 1)][TypeFilter("GetAOEAttacks")]
    public AOEAttack AoeAttack = new TriggerAOEOnce();

    /*[ShowIf("WeaponType", 4)]*/[SerializeField]
    public SplineAttack SplineAttack = new SplineAttack();
    public string WeaponName;

    public void InitWeapon()
    {
        if (WeaponType == 0)
        {
            projectileAttack.InitlizeAttack(this);
            onAttack += FireProjectileAttack;
            /*onAttackInitiate += projectileFinalPoint.StartAsyncRotation;
            onAttackCease += projectileFinalPoint.StopAsyncRotation;*/
            if (!projectileAttack.AttackProperties.Projectiles.Any(x => x.projectileEffect.Homing))
            {
                RotatingTurret = true;
            }
        }

        if (WeaponType == 1)
        {
            AoeAttack.AOESize = Data.componentRadius;
            AoeAttack.InitlizeAttack(this);
            onAttack += FireAOEAttack;
            onAttackCease += AoeAttack.StopAOEAttack;
        }

        if (WeaponType == 4)
        {
            SplineAttack.InitlizeAttack(this);
            onAttack += FireSplineAttack;
            onAttackCease += SplineAttack.StopSplineAttack;
        }
        if (RotatingTurret)
        {
            onAttackInitiate += GenericRotator.StartAsyncRotation;
            onAttackCease += GenericRotator.StopAsyncRotation;
        }
    }
    
    private static IEnumerable<Type> GetAOEAttacks()
    {
        var q = typeof(AOEAttack).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(AOEAttack).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
    
    
    [ConditionalField("debug")]
    public EffectableTargetBank TargetBank;

    private ComponentRotator componentRotator = null;
    private ComponentRotator ComponentRotator
    {
        get
        {
            if (componentRotator == null)
            {
                componentRotator = this.GetOrAddComponent<ComponentRotator>();
            }

            return componentRotator;
        }
    }

    [ConditionalField("debug")] 
    public int testAsyncAttackcounter;
    
    public float AttackCounter;
    public float CounterMax = 1;
    
    [ConditionalField("debug")]
    public PoolObjectQueue<GenericProjectile> ProjectileQueuePool;
    
    [ConditionalField("debug")]
    public bool ExternalAttackLock = false;

    [SerializeField]
    [ConditionalField("debug")]
    public bool canattackFieldPH;
    public virtual bool CanAttack() {
        if (ExternalAttackLock == false) {
            if (Target != null) {
                canattackFieldPH = true;
                return true;
            }
        }
        canattackFieldPH = false;
        return false;
    }
    
    
    public bool ShouldRotate 
    {
        get {
            if (CanAttack()) {
                if (InAttackState == true) {
                    return true;
                }
            }
            return false;
        }
    }


    public event Action<GenericWeaponController, Effectable> onEnemyEnteredRange;
    public void OnEnemyEnteredRange(Effectable ef) {
        onEnemyEnteredRange?.Invoke(this, ef);
    }

    ProjectileExitPoint projectileExitPoint;
    

    public Vector2 ProjectileExitPoint {
        get {
            return projectileExitPoint?.transform.position ?? transform.position;
        }
    }
    public ProjectileFinalPoint projectileFinalPoint;
    public Transform ProjectileFinalPointTransform { get => projectileFinalPoint.transform;}
    public Vector2 ProjectileFinalPointV2 {
        get {
            return projectileFinalPoint?.transform.position ?? Target?.UnitController.transform.position ?? transform.position;
        }
    }

    public event Action<TargetUnit> onTargetAdd;

    public void OnTargetAdd(TargetUnit tu)
    {
        onTargetAdd?.Invoke(tu);
    }

    void SetTarget(TargetUnit tu)
    {
        Target = tu;
        
    }
    
    
    public void StandardOnTargetEnteredRange(GenericWeaponController self, Effectable ef) {
        TargetUnit tu = GameObjectPool.Instance.GetTargetUnit(ef.name);
        if (Target?.Effectable == null) {
            OnTargetAdd(tu);
            InAttackState = true;
        }
        if (Target?.Effectable != null) {
            
            if (tu.Proximity < Target.Proximity) {
                OnTargetAdd(tu);
                InAttackState = true;
            }
        }
    }

    public virtual void StandardOnTargetLeftRange(string targetName,string callerName) {
        if (Target?.name == targetName)
        {
            Target = FindSingleTargetNearestToEndOfSpline(targetName);
        }
        else
        {
            Target = FindSingleTargetNearestToEndOfSpline();
        }
    }
    string CurrentLowestProximtyTargetName;
    TargetUnit FindSingleTargetNearestToEndOfSpline() {
        TargetUnit tu = null;
        float p = 999999.0f;
        foreach (var item in TargetBank.Targets)
        {
            if (!GameObjectPool.Instance.ActiveUnitPool.Pool.ContainsKey(item.Key)) {
                continue;
            }
            float tp = GameObjectPool.Instance.ActiveUnitPool.Pool[item.Key].Proximity;
            if (tp < p) {
                p = tp;
                tu = GameObjectPool.Instance.GetTargetUnit(item.Key);
            }
        }
        CurrentLowestProximtyTargetName = tu?.name ?? "NULL";
        return tu;
    }
    
    TargetUnit FindSingleTargetNearestToEndOfSpline(string formerTargetName) {
        TargetUnit tu = null;
        float p = 999999.0f;
        foreach (var item in TargetBank.Targets)
        {
            if (!GameObjectPool.Instance.ActiveUnitPool.Pool.ContainsKey(item.Key)) {
                continue;
            }

            if (item.Key == formerTargetName)
            {
                continue;
            }
            float tp = GameObjectPool.Instance.ActiveUnitPool.Pool[item.Key].Proximity;
            if (tp < p) {
                p = tp;
                tu = GameObjectPool.Instance.GetTargetUnit(item.Key);
            }
        }
        CurrentLowestProximtyTargetName = tu?.name ?? "NULL";
        return tu;
    }

    bool AsyncAttackInProgress = false;

    public void StopAsyncAttack() {
        AsyncAttackInProgress = false;
    }
    public async void StartAsyncAttack() {
        testAsyncAttackcounter += 1;
        AsyncAttackInProgress = true;
        while (CanAttack() && AsyncAttackInProgress == true) {
            if (AttackCounter >= CounterMax)
            {
                Attack();
            }

            if (CanAttack() == false)
            {
                Debug.LogWarning("Can attack is false bu i'm still attacking");
            }
            await Task.Yield();
        }
        AsyncAttackInProgress = false;
        InAttackState = false;
        Target = FindSingleTargetNearestToEndOfSpline();
        if (CanAttack()) {
            InAttackState = true;
        }

        testAsyncAttackcounter -= 1;
    }


    event Action onAttack;

    void OnAttack()
    {
        onAttack?.Invoke();
    }

    

    void Attack()
    {
        if (CounterMax > 0 ) {
            if (AttackCounter >= CounterMax)
            {
                AttackCounter = 0;
                OnAttack();
            }
        }

    }
    public event Action<string,string> onEnemyLeftRange;
    public void OnEnemyLeftRange(string targetName,string callerName) {
        onEnemyLeftRange?.Invoke(targetName ?? null, name ?? null);
    }
    
    [ShowInInspector]
    public TargetUnit Target {
        get => Data.targetUnit;
        set {
            Data.targetUnit = value;
            GenericRotator.Target = value?.transform;    
        }
    }

    [SerializeField]
    bool inAttackState = false;
    public bool InAttackState {
        get => inAttackState;
        set {
            if (value == true) {
                if (CanAttack()) {
                    inAttackState = true;
                    if (AsyncAttackInProgress == false)
                    {
                        OnAttackInitiate();
                    }
                }
                else
                {
                    Debug.LogWarning("Attack state set to true, but conditions to attack are False.");
                }
            }
            if (value == false) {
                inAttackState = false;
                OnAttackCease();
            }
            
        }
    }

    public event Action onAttackInitiate;
    public void OnAttackInitiate() {
            onAttackInitiate?.Invoke();
    }
    public event Action onAttackCease;
    public void OnAttackCease() {
        onAttackCease?.Invoke();
    }

    void RotateTowardsTarget()
    {
        ComponentRotator.TargetTransform = Target.transform;
        ComponentRotator.StartRotatingTowardsTarget();
    }

    protected void Awake()
    {
        base.Awake();
        GenericRotator = GetComponent<GenericRotator>();
        onTargetAdd += SetTarget;
    }
    public override void PostAwake() {
        
        TargetBank = GetComponent<EffectableTargetBank>();
        if (TargetBank != null) {
            TargetBank.onTargetAdd += OnEnemyEnteredRange;
            TargetBank.onTargetRemove += OnEnemyLeftRange;
            //TargetBank.onConcurrentProximityCheck += OnConcurrentTargetCheck;
        }
        
        onAttackInitiate += StartAsyncAttack;
        onAttackCease += StopAsyncAttack;
        
        
        if (RotatingComponent) {
        onAttackInitiate += RotateTowardsTarget;
        onAttackCease += ComponentRotator.StopRotating;
        }
        
        onEnemyEnteredRange += StandardOnTargetEnteredRange;
        onEnemyLeftRange += StandardOnTargetLeftRange;
    }



    
    protected void Start() {
        RangeDetector = GetComponentInChildren<RangeDetector>() ?? null;
        projectileExitPoint = GetComponentInChildren<ProjectileExitPoint>() ?? null;
        projectileFinalPoint = GetComponentInChildren<ProjectileFinalPoint>() ?? null;
        if (TargetBank != null) {
            if (Data.componentRadius == 0) {
                Data.componentRadius = 1;
            }
            else if (Data.componentRadius != 0) {
                RangeDetector.SetSize(Data.componentRadius);
            }
        }

        ParentTower = ParentTower ?? GetComponentInParent<TowerControllerLegacy>();
        onAttackInitiate += projectileExitPoint.StartAsyncRotation;
        onAttackCease += projectileExitPoint.StopAsyncRotation;
        InitWeapon();
        projectileFinalPoint.Position = new Vector2(projectileFinalPoint.Position.x + (Data.componentRadius), projectileFinalPoint.Position.y);
    }

    protected void Update()
    {
        if (AttackCounter < CounterMax)
        {
            if (Data.fireRate > 0) {
            AttackCounter += StaticObjects.Instance.DeltaGameTime * Data.fireRate;
            }
            else
            { 
                AttackCounter += StaticObjects.Instance.DeltaGameTime;
            }
        }
    }

    
}
