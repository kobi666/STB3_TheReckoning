using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using MyBox;
using System.Threading.Tasks;
using Sirenix;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class GenericWeaponController : TowerComponent
{
    
    [ValueDropdown("valueList")]
    [HideLabel]
    [BoxGroup]
    [SerializeField]
    protected string WeaponType;
    
    private static ValueDropdownList<string> valueList = new ValueDropdownList<string>()
    {
        {"Projectile Effect", "Projectile Effect"},
        {"Area Of Effect", "Area Of Effect"},
        {"Beam Effect","Beam Effect"}
    };

    [ShowIf("WeaponType", "Projectile Effect")] [OdinSerialize] [BoxGroup]
    public ProjectileFireFunction fireFunction = new ProjectileFireFunction();
    
    
    
    
    //private ValueDropdownList<MethodInfo> attackTypes = ProjectileAttacks.Atypes();
    
    
    /*[ShowIf("WeaponType", "Projectile Effect")] 
    [SerializeField]
    [BoxGroup]
    public List<ProjectileBehaviorData> ProjectileTypes = new List<ProjectileBehaviorData>();
    [ShowIf("WeaponType", "Projectile Effect")] public GenericProjectile ProjectileBase;*/
    
    
    

    [ShowIf("WeaponType", "Area Of Effect")]
    [SerializeField]
    protected Action<AOEProjectile>[] AoeEvent = new Action<AOEProjectile>[0];

    [ShowIf("WeaponType", "Beam Effect")]
    [SerializeField]
    protected Func<bool>[] LaZorEvent = new Func<bool>[0];
    public string WeaponName;

    void InitializeProjectileWeapon()
    {
        
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

    public virtual void StandardOnTargetEnteredRange(GenericWeaponController self, Effectable ef) {
        TargetUnit tu = GameObjectPool.Instance.GetTargetUnit(ef.name);
        if (Target?.Effectable == null) {
            Target = tu;
            InAttackState = true;
        }
        if (Target?.Effectable != null) {
            
            if (tu.Proximity < Target.Proximity) {
                Target = tu;
                InAttackState = true;
            }
        }
    }

    public virtual void StandardOnTargetLeftRange(string targetName,string callerName) {
        Target = FindSingleTargetNearestToEndOfSpline();
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
                AttackOnce();
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

    public event Action OnAttack;

    public event Action<GenericProjectile> onProjectileAttack;

    public void ProjectileAttack()
    {
        GenericProjectile proj = ProjectileQueuePool.GetInactive();
        proj.transform.position = ProjectileExitPoint;
        proj.EffectableTarget = Target.Effectable;
        proj.TargetPosition = ProjectileFinalPointV2;
        proj.Damage = Data.damageRange.RandomDamage();
        proj.Speed = Data.projectileData.projectileSpeed;
        proj.gameObject.SetActive(true);
        onProjectileAttack?.Invoke(proj);
    }

    public void MainAttackFunction()
    {
        
    }

    void AttackOnce()
    {
        if (CounterMax > 0 ) {
            if (AttackCounter >= CounterMax)
            {
                AttackCounter = 0;
                MainAttackFunction();
            }
        }

    }
    public event Action<string,string> onEnemyLeftRange;
    public void OnEnemyLeftRange(string targetName,string callerName) {
        onEnemyLeftRange?.Invoke(targetName ?? null, name ?? null);
    }

    public TargetUnit Target {
        get => Data.targetUnit;
        set {
            Data.targetUnit = value;
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
                TargetBank.RangeDetector.SetRangeRadius(Data.componentRadius);
            }
        }
    }

    protected void Update()
    {
        
        if (AttackCounter < CounterMax)
        {
            if (Data.fireRate > 0) {
            AttackCounter += StaticObjects.instance.DeltaGameTime * Data.fireRate;
            }
            else
            { 
                AttackCounter += StaticObjects.instance.DeltaGameTime;
            }
        }
        
        
    }

    
}
