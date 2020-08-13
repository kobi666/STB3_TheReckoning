using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Animancer;
using TMPro;
using System.Threading.Tasks;



public abstract class WeaponController : TowerComponent
{

    TargetBank<Effectable> TargetBank;
    public int Damage {
        get {
            return UnityEngine.Random.Range(Data.damageRange.min, Data.damageRange.max);
        }
    }
    public float AttackCounter;
    public float CounterMax = 3;
    public PoolObjectQueue<Projectile> ProjectileQueuePool;
    public bool ExternalAttackLock = false;

    [SerializeField]
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


    public event Action<WeaponController, Effectable> onEnemyEnteredRange;
    public void OnEnemyEnteredRange(Effectable ef) {
        onEnemyEnteredRange?.Invoke(this, ef);
    }

    ProjectileExitPoint projectileExitPoint;
    

    public Vector2 ProjectileExitPoint {
        get {
            return projectileExitPoint?.transform.position ?? transform.position;
        }
    }
    ProjectileFinalPoint projectileFinalPoint;
    public Transform ProjectileFinalPointTransform { get => projectileFinalPoint.transform;}
    public Vector2 ProjectileFinalPointV2 {
        get {
            return projectileFinalPoint?.transform.position ?? Target?.UnitController.transform.position ?? transform.position;
        }
    }

    public virtual void StandardOnTargetEnteredRange(WeaponController self, Effectable ef) {
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

    public virtual void StandardOnTargetLeftRange(string targetName) {
        if (Target?.name == targetName) {
            Target = FindSingleTargetNearestToEndOfSpline() ?? null;
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

    bool AsyncAttackInProgress = false;

    public void StopAsyncAttack() {
        AsyncAttackInProgress = false;
    }
    public async void StartAsyncAttack() {
        if (AsyncAttackInProgress == true) {
            AsyncAttackInProgress = false;
            await Task.Yield();
        }
        AsyncAttackInProgress = true;
        while (CanAttack() && AsyncAttackInProgress == true) {
            AttackCounter += (StaticObjects.instance.DeltaGameTime * Data.FireRate) / 10;
            if (AttackCounter >= CounterMax) {
                MainAttackFunction();
                AttackCounter = 0;
                }
            await Task.Yield();
        }
        AsyncAttackInProgress = false;
        InAttackState = false;
        Target = FindSingleTargetNearestToEndOfSpline();
        if (CanAttack()) {
            InAttackState = true;
        }
    }

    public abstract void MainAttackFunction();

    



    public event Action<string> onEnemyLeftRange;
    public void OnEnemyLeftRange(string targetName) {
        onEnemyLeftRange?.Invoke(targetName);
    }

    public TargetUnit Target {
        get => Data.TargetUnit;
        set {
            Data.TargetUnit = value;
        }
    }

    [SerializeField]
    bool inAttackState = false;
    public bool InAttackState {
        get => inAttackState;
        set {
            if (value == true) {
                if (CanAttack()) {
                    inAttackState = value;
                    OnAttackInitiate();
                }
                else
                {
                    Debug.LogWarning("Attack state set to true, but conditions to attack are False.");
                }
            }
            if (value == false) {
                inAttackState = value;
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

    
    public override void PostAwake() {
        
        TargetBank = GetComponent<EffectableTargetBank>();
        if (TargetBank != null) {
            TargetBank.onTargetAdd += OnEnemyEnteredRange;
            TargetBank.onTargetRemove += OnEnemyLeftRange;
            //TargetBank.onConcurrentProximityCheck += OnConcurrentTargetCheck;
        }
        
        onAttackInitiate += StartAsyncAttack;
        onAttackCease += StopAsyncAttack;
        onEnemyEnteredRange += StandardOnTargetEnteredRange;
        onEnemyLeftRange += StandardOnTargetLeftRange;
    }

    public abstract void PostStart();
    private void Start() {
        RangeDetector = GetComponentInChildren<RangeDetector>() ?? null;
        ProjectileQueuePool = GameObjectPool.Instance.GetProjectileQueue(Data.ProjectilePrefab) ?? null;
        projectileExitPoint = GetComponentInChildren<ProjectileExitPoint>() ?? null;
        projectileFinalPoint = GetComponentInChildren<ProjectileFinalPoint>() ?? null;
        if (TargetBank != null) {
            if (Data.Radius == 0) {
                Data.Radius = 1;
            }
            else if (Data.Radius != 0) {
                TargetBank.rangeDetector.SetRangeRadius(Data.Radius);
            }
        }
        PostStart();
    }

}