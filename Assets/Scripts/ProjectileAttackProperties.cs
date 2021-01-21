using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using UniRx;

[System.Serializable]
public abstract class ProjectileAttackProperties : AttackProperties,IhasExitAndFinalPoint
{
    public abstract int ProjectileMultiplier { get; set; }
    
    

    public void InitializeAttackProperties(GenericWeaponController parentWeapon)
    {
        ParentWeapon = parentWeapon;
        InitializeAttack(ParentWeapon);
    }
    public abstract void InitializeAttack(GenericWeaponController parentWeapon);

    public abstract void AttackFunction(Effectable singleTarget,
         Vector2 SingleTargetPosition);
    
    public event Action onAttack;
    
    public bool AsyncAttackInProgress;
    
    
    [SerializeField] public List<ProjectilePoolCreationData> Projectiles = new List<ProjectilePoolCreationData>();

    public async void Attack(Effectable singleTarget,
        Vector2 singleTargetPosition)
    {
        onAttack?.Invoke();
        AttackFunction(singleTarget, singleTargetPosition);
    }

    public abstract List<ProjectileFinalPoint> GetFinalPoints();




    public abstract List<ProjectileExitPoint> GetExitPoints();

}
