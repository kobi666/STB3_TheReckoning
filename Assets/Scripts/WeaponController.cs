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
    public float AttackCounter;
    public float CounterMax = 3;
    public PoolObjectQueue<Projectile> ProjectilePool;
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


    public event Action<WeaponController, EnemyUnitController> onEnemyEnteredRange;
    public void OnEnemyEnteredRange(EnemyUnitController ec) {
        onEnemyEnteredRange?.Invoke(this, ec);
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
            return projectileFinalPoint?.transform.position ?? Target?.transform.position ?? transform.position;
        }
    }

    public virtual void StandardOnTargetEnteredRange(WeaponController self, EnemyUnitController ec) {
        if (Target == null) {
            Target = ec;
            InAttackState = true;
        }
        if (Target != null) {
            if (ec.Proximity < Target.Proximity) {
                Target = ec;
                InAttackState = true;
            }
        }
    }

    public void OnConcurrentTargetCheck(EnemyUnitController ec) {
        if (ec.name == Target.name) {
            Debug.LogWarning("Same Target, attack state is : " + InAttackState);
             if (CanAttack() && InAttackState == false) {
                 InAttackState = true;
            }
        }
        if (ec.name != Target.name) {
            Target = ec;
            if (CanAttack()) {
                inAttackState = true;
            }
        }
    }

    public virtual void StandardOnTargetLeftRange(string targetName) {
        if (Target.name == targetName) {
            Target = TargetBank.FindSingleTargetNearestToEndOfSpline() ?? null;
        }
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
        Target = TargetBank.FindSingleTargetNearestToEndOfSpline();
        if (CanAttack()) {
            InAttackState = true;
        }
    }

    public abstract void MainAttackFunction();

    public void DefaultAttack() {

    }



    public event Action<string> onEnemyLeftRange;
    public void OnEnemyLeftRange(string targetName) {
        onEnemyLeftRange?.Invoke(targetName);
    }

    public EnemyUnitController Target {
        get => Data.EnemyTarget;
        set {
            Data.EnemyTarget = value;
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
        if (TargetBank != null) {
            TargetBank.targetEnteredRange += OnEnemyEnteredRange;
            TargetBank.targetLeftRange += OnEnemyLeftRange;
            //TargetBank.onConcurrentProximityCheck += OnConcurrentTargetCheck;
        }
        onAttackInitiate += StartAsyncAttack;
        onAttackCease += StopAsyncAttack;
        onEnemyEnteredRange += StandardOnTargetEnteredRange;
        onEnemyLeftRange += StandardOnTargetLeftRange;
    }

    public abstract void PostStart();
    private void Start() {
        ProjectilePool = GameObjectPool.Instance.GetProjectilePool(Data.ProjectilePrefab) ?? null;
        projectileExitPoint = GetComponentInChildren<ProjectileExitPoint>() ?? null;
        projectileFinalPoint = GetComponentInChildren<ProjectileFinalPoint>() ?? null;
        if (TargetBank != null) {
            if (Data.Radius == 0) {
                Data.Radius = TargetBank.RangeCollider.radius;
            }
            else if (Data.Radius != 0) {
                TargetBank.RangeCollider.radius = Data.Radius;
            }
        }
        PostStart();
    }

}