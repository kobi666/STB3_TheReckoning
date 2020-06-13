using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public abstract class WeaponControllerYield : TowerComponent
{
    public float AttackCounter;

    public bool ExternalAttackLock = false;
    public abstract bool CanAttack();
    public event Action<WeaponControllerYield, EnemyUnitController> onEnemyEnteredRange;
    public void OnEnemyEnteredRange(EnemyUnitController ec) {
        onEnemyEnteredRange?.Invoke(this, ec);
    }

    ProjectileExitPoint projectileExitPoint;

    public Vector2 ProjectileExitPoint {
        get {
            return projectileExitPoint?.transform.position ?? transform.position;
        }
    }

    public virtual void StandardOnTargetEnteredRange(WeaponControllerYield self, EnemyUnitController ec) {
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
            MainAttackFunction();
            await Task.Yield();
        }
        AsyncAttackInProgress = false;
        InAttackState = false;
    }

    public abstract void MainAttackFunction();



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
        }
        onAttackInitiate += StartAsyncAttack;
        onAttackCease += StopAsyncAttack;
        onEnemyEnteredRange += StandardOnTargetEnteredRange;
        onEnemyLeftRange += StandardOnTargetLeftRange;
    }

    public abstract void PostStart();
    private void Start() {
        projectileExitPoint = GetComponentInChildren<ProjectileExitPoint>() ?? null;
        PostStart();
    }

}
