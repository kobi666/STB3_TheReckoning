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
using UnityEngine.Serialization;


[RequireComponent(typeof(EffectableTargetBank))][Searchable]
public class GenericWeaponController : TowerComponent,IhasExitAndFinalPoint,ITargeter
{

    public bool Autonomous = true;
    
    [ValueDropdown("valueList")]
    [HideLabel]
    [BoxGroup]
    [SerializeField]
    protected int WeaponTypeInt;

    private WeaponAttack weaponAttack;

    public WeaponAttack WeaponAttack
    {
        get => weaponAttack;
        set => weaponAttack = value;
    }



    public bool RotatingTurret;
    private GenericRotator GenericRotator;
    private string cachedTargetName;
    
    private static ValueDropdownList<int> valueList = new ValueDropdownList<int>()
    {
        {"Projectile Effect", 0},
        {"Beam Effect",4},
        {"Area Of Effect", 1}
    };
    
    

    [ShowIf("WeaponTypeInt", 0)][BoxGroup]
    public ProjectileAttack projectileAttack;




    [ShowIf("WeaponTypeInt", 1)][TypeFilter("GetAOEAttacks")]
    public AOEAttack AoeAttack = new TriggerAOEOnce();

    [ShowIf("WeaponTypeInt", 4)][SerializeField]
    public SplineAttack SplineAttack = new SplineAttack();
    public string WeaponName;

    private void SetFinalPointPosOnAttackStart(Effectable ef, Vector2 targetPos)
    {
        if (RotatingTurret)
        {
            foreach (var fp in WeaponAttack.GetFinalPoints())
            {
                fp.transform.position = fp.InitialPosition;
            }
        }
    }

    public void InitWeapon()
    {
        if (WeaponTypeInt == 0)
        {
            WeaponAttack = projectileAttack;
            /*projectileAttack.InitlizeAttack(this);
            onAttack += FireProjectileAttack;*/
            /*if (!projectileAttack.AttackProperties.Projectiles.Any(x => x.projectileEffect.Homing))
            {
                RotatingTurret = true;
            }*/
        }

        if (WeaponTypeInt == 1)
        {
            WeaponAttack = AoeAttack;
            /*AoeAttack.InitlizeAttack(this);
            onAttack += FireAOEAttack;
            onAttackCease += AoeAttack.StopAOEAttack;*/
        }

        if (WeaponTypeInt == 4)
        {
            
            WeaponAttack = SplineAttack;
            
        }

        WeaponAttack.parentWeaponController = this;
        WeaponAttack.InitlizeAttack(this);
        onAttack += WeaponAttack.Attack;
        onAttackCease += WeaponAttack.StopAttack;
        if (RotatingTurret)
        {
            onAttackInitiate += GenericRotator.StartAsyncRotation;
            onAttackCease += GenericRotator.StopAsyncRotation;
        }
        WeaponAttack.SetInitialFinalPointPosition();
    }
    
    private static IEnumerable<Type> GetAOEAttacks()
    {
        var q = typeof(AOEAttack).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(AOEAttack))); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
    
    private static IEnumerable<Type> getWeaponTypes()
    {
        var q = typeof(WeaponAttack).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(WeaponAttack))); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
    
    
    [ShowIf("debug")]
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
            if (TargetExists) {
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
    public ProjectileFinalPoint ProjectileFinalPoint;
    public Transform ProjectileFinalPointTransform { get => ProjectileFinalPoint.transform;}
    public Vector2 ProjectileFinalPointV2 {
        get {
            return ProjectileFinalPoint?.transform.position ?? Target?.transform.position ?? transform.position;
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
            if (Autonomous)
            {
                InAttackState = true;
            }
        }
        if (Target?.Effectable != null) {
            
            if (tu.Proximity < Target.Proximity) {
                OnTargetAdd(tu);
                if (Autonomous)
                {
                    InAttackState = true;
                }
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

    public List<Effect> GetEffects()
    {
        return WeaponAttack.GetEffects();
    }

    string CurrentLowestProximtyTargetName;
    TargetUnit FindSingleTargetNearestToEndOfSpline() {
        TargetUnit tu = null;
        float p = 999999.0f;
        foreach (var item in TargetBank.Targets)
        {
            if (!GameObjectPool.Instance.Targetables.Contains(item.Key)) {
                continue;
            }
            float tp = GameObjectPool.Instance.ActiveUnits[item.Key].PathWalker.ProximityToPathEnd;
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
        while (AsyncAttackInProgress) {
            if (CanAttack() && AttackCounter >= CounterMax)
            {
                Attack();
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


    event Action<Effectable,Vector2> onAttack;

    void OnAttack()
    {
        onAttack?.Invoke(Target.Effectable, Target.TargetTransform.position);
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
            onTargetSet?.Invoke(Target?.name ?? string.Empty);
            if (value != null)
            {
                TargetExists = true;
            }
            else
            {
                TargetExists = false;
            }
        }
    }
    [GUIColor(1,0,0)]
    public bool TargetExists = false;

    

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


    void UpdateTargetState(string targetName, bool state)
    {
        if (targetName == Target?.name)
        {
            if (state == false)
            {
                Target = null;
            }
        }
    }

    protected void Awake()
    {
        base.Awake();
        GenericRotator = GetComponent<GenericRotator>();
        onTargetAdd += SetTarget;
        GameObjectPool.Instance.onTargetableUpdate += UpdateTargetState;
    }

    public override void InitComponent()
    {
        InitWeapon();
    }

    public override void PostAwake() {
        
        
    }



    
    protected void Start() {
        TargetBank = TargetBank ?? GetComponent<EffectableTargetBank>();
        if (TargetBank != null) {
            TargetBank.onTargetAdd += OnEnemyEnteredRange;
            TargetBank.onTargetRemove += OnEnemyLeftRange;
            //TargetBank.onConcurrentProximityCheck += OnConcurrentTargetCheck;
        }
        
        onAttackInitiate += StartAsyncAttack;
        onAttackCease += StopAsyncAttack;
        
        
        
        
        onEnemyEnteredRange += StandardOnTargetEnteredRange;
        onEnemyLeftRange += StandardOnTargetLeftRange;
        RangeDetector = RangeDetector ?? GetComponentInChildren<TagDetector>();
        projectileExitPoint = GetComponentInChildren<ProjectileExitPoint>() ?? null;
        ProjectileFinalPoint = GetComponentInChildren<ProjectileFinalPoint>() ?? null;
        if (RotatingComponent) {
            onAttackInitiate += RotateTowardsTarget;
            onAttackCease += ComponentRotator.StopRotating;
        }
        if (TargetBank != null) {
            if (Data.componentRadius == 0) {
                Data.componentRadius = 1;
            }
            else if (Data.componentRadius != 0) {
                RangeDetector.UpdateSize(Data.componentRadius);
            }
        }

        ParentTowerLegacy = ParentTowerLegacy ?? GetComponentInParent<TowerControllerLegacy>();
        if (projectileExitPoint != null) {
        onAttackInitiate += projectileExitPoint.StartAsyncRotation;
        onAttackCease += projectileExitPoint.StopAsyncRotation;
        }
        InitWeapon();
        
    }

    public override List<Effect> GetEffectList()
    {
        return WeaponAttack.GetEffectList();
    }

    public override void UpdateEffect(Effect ef,List<Effect> appliedEffects)
    {
        WeaponAttack.UpdateEffect(ef,appliedEffects);
    }

    public override List<TagDetector> GetTagDetectors()
    {
        return WeaponAttack.GetTagDetectors();
    }

    public override void UpdateRange(float RangeSizeDelta, List<TagDetector> detectors)
    {
        base.UpdateRange(RangeSizeDelta, detectors);
        WeaponAttack.UpdateRange(RangeSizeDelta,detectors);
    }


    


    //once state machine is in place transfer to async function
    protected void Update()
    {
        if (AttackCounter < CounterMax)
        {
            if (Data.fireRate > 0) {
            AttackCounter += StaticObjects.DeltaGameTime * Data.fireRate;
            }
            else
            { 
                AttackCounter += StaticObjects.DeltaGameTime;
            }
        }
    }


    public List<ProjectileFinalPoint> GetFinalPoints()
    {
        return WeaponAttack.GetFinalPoints();
    }

    public void SetInitialFinalPointPosition()
    {
        WeaponAttack.SetInitialFinalPointPosition();
    }

    public List<ProjectileExitPoint> GetExitPoints()
    {
        return WeaponAttack.GetExitPoints();
    }

    public void SetInitialExitPointPosition()
    {
        WeaponAttack.SetInitialExitPointPosition();
    }

    public event Action<string> onTargetSet;
}
