using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using UniRx;

[System.Serializable]
public abstract class ProjectileAttackFunction 
{
    public abstract int ProjectileMultiplier { get; set; }
    public abstract void InitializeAttack();

    public abstract void AttackFunction(Effectable singleTarget,
         Vector2 SingleTargetPosition);
    
    public event Action onAttack;
    
    public bool AsyncAttackInProgress;
    
    
    [OdinSerialize] public List<ProjectilePoolCreationData> Projectiles = new List<ProjectilePoolCreationData>();

    public async void Attack(Effectable singleTarget,
        Vector2 singleTargetPosition)
    {
        onAttack?.Invoke();
        AttackFunction(singleTarget, singleTargetPosition);
    }
}
