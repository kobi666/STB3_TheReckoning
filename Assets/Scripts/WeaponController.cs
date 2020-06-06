﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Animancer;
using TMPro;


public abstract class WeaponController : TowerComponent
{
    public override event Action<EnemyUnitController> onEnemyEnteredRange;
    public override void OnEnemyEnteredRange(EnemyUnitController ec) {
        onEnemyEnteredRange?.Invoke(ec);
    }
    public abstract Vector2 ProjectileExitPoint {get;}
    
    public IEnumerator<WeaponController> AttackCoroutine = null;
    
    public EnemyUnitController Target {
        get => Data.EnemyTarget;
    }

    public event Action<WeaponController, EnemyUnitController> onAttackInitiate;
    public void OnAttackInitiate(EnemyUnitController ec) {
        onAttackInitiate?.Invoke(this, ec);
    }

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
        TargetBank.targetEnteredRange += OnEnemyEnteredRange;
        }
        PostStart();
    }

    

    

}
