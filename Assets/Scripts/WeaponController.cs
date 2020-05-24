using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Animancer;
using TMPro;

public abstract class WeaponController : TowerComponent
{
    public AnimancerComponent Animancer;
    public IEnumerator<WeaponController> AttackCoroutine = null;
    public WeaponData Data;
    
    public EnemyUnitController Target {
        get => Data.EnemyTarget;
    }

    public abstract event Action onAttack;
    



     
    
    public EnemyTargetBank TargetBank {get ; private set;}

    private void Awake() {
        TargetBank = GetComponent<EnemyTargetBank>() ?? transform.parent.GetComponent<EnemyTargetBank>() ?? null;
        Animancer = GetComponent<AnimancerComponent>() ?? null;
    }

}
