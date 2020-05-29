using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Animancer;
using TMPro;

public abstract class WeaponController : TowerComponent
{
    
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
    

    private void Awake() {
        SR = GetComponent<SpriteRenderer>() ?? null;
        Animancer = GetComponent<AnimancerComponent>() ?? null;
    }

    

    

}
