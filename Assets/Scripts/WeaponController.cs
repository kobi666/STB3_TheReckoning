using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Animancer;
using TMPro;

public abstract class WeaponController : TowerComponent
{
    public Tower ParentTower;
    public SpriteRenderer SR;
    public abstract Vector2 ProjectileExitPoint {get;}
    public AnimancerComponent Animancer;
    public IEnumerator<WeaponController> AttackCoroutine = null;
    public WeaponData Data;
    
    public EnemyUnitController Target {
        get => Data.EnemyTarget;
    }

    public abstract event Action onAttack;

    public abstract void OnAttack();
    public EnemyTargetBank TargetBank {get ; private set;}

    private void Awake() {
        SR = GetComponent<SpriteRenderer>() ?? null;
        Animancer = GetComponent<AnimancerComponent>() ?? null;
    }

    

    private void Start() {
        TargetBank = GetComponentInChildren<EnemyTargetBank>() ?? ParentTower?.TargetBank ?? null;
        PostStart();
    }
    public abstract void PostStart();

}
