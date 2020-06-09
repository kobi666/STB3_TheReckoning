using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Animancer;
using TMPro;


public abstract class WeaponController : TowerComponent
{
    [SerializeField]
    bool canAttack = true;
    public bool CanAttack {
        get => canAttack;
        set {
            canAttack = value;
            if (value == false) {
            OnAttackCease();
            }
        }
    }


    public event Action<WeaponController,EnemyUnitController> onEnemyEnteredRange;
    public virtual void OnEnemyEnteredRange(EnemyUnitController ec) {
        onEnemyEnteredRange?.Invoke(this,ec);
    }
    ProjectileExitPoint projectileExitPoint;
    public Vector2 ProjectileExitPoint {
        get {
            if (projectileExitPoint == null) {
                return transform.position;
            }
            else {
                return projectileExitPoint.transform.position;
            }
        }
    }
    
    public IEnumerator AttackCoroutinePlaceHolder;
    public abstract IEnumerator AttackCoroutine {get;set;}

    public void ReStartAttacking(WeaponController self, IEnumerator attackSequence) {
        StopAttacking();
        AttackCoroutinePlaceHolder = attackSequence;
        StartCoroutine(AttackCoroutinePlaceHolder);
    }

    public void StopAttacking() {
        if (AttackCoroutinePlaceHolder != null) {
            StopCoroutine(AttackCoroutinePlaceHolder);
        }
        AttackCoroutinePlaceHolder = null;
    }
    
    public EnemyUnitController Target {
        get => Data.EnemyTarget;
        set {
            Data.EnemyTarget = value;
        }
    }
    [SerializeField]
    bool attacking;
    public bool Attacking {
        get => attacking;
        set {
            attacking = value;
            if (value == true) {
                OnAttackInitiate();
            }
            if (value == false) {
                OnAttackCease();
            }
        }
    }

    public event Action onAttackInitiate;
    public void OnAttackInitiate() {
        onAttackInitiate?.Invoke();
    }

    public abstract void InitiateAttackSequence();
    public abstract void CeaseAttackSequence();

    public event Action onAttackCease;

    public void OnAttackCease() {
        onAttackCease?.Invoke();
    }

    public event Action<string> onEnemyLeftRange;
    public void OnEnemyLeftRange(string enemyName) {
        onEnemyLeftRange?.Invoke(enemyName);
    }

    public virtual void GetEnemyTarget(EnemyUnitController ec) {
        if (Data.EnemyTarget == null) {

        }
    }

    

    public abstract event Action onAttack;

    public abstract void OnAttack();
    
    public abstract void PostStart();
    private void Start() {
        if (TargetBank != null) {
        projectileExitPoint = GetComponentInChildren<ProjectileExitPoint>() ?? null;
        TargetBank.targetEnteredRange += OnEnemyEnteredRange;
        onEnemyEnteredRange += WeaponUtils.StandardOnEnemyEnteredRange; 
        onAttackInitiate += InitiateAttackSequence;
        onAttackCease += CeaseAttackSequence;
        }
        
        PostStart();
    }

    

    

}
