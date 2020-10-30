using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ProjectileAttackFunction 
{
    public abstract int NumOfProjectiles { get; set; }
    
    public abstract List<PoolObjectQueue<GenericProjectile>> ProjectilePools {get; set; }
    public abstract void InitializeAttack();

    public abstract void AttackFunction(
        Quaternion direction, Vector2 originPosition, Vector2 SingleTargetPosition, Effectable singleTarget);

    public event Action onAttack; 
    
    /*public event Action<List<PoolObjectQueue<GenericProjectile>>, int,
        Quaternion, Vector2, Effectable, Effectable[],
        float, float, int, List<ProjectileExitPoint> ,List<ProjectileFinalPoint>> attack;*/
    public void Attack(Quaternion direction, Vector2 originPosition,
        Vector2 singleTargetPosition, Effectable singleTarget)
    {
        onAttack?.Invoke();
        AttackFunction(direction, originPosition,singleTargetPosition, singleTarget);
    }
}
