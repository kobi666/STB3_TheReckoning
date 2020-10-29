using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ProjectileAttackFunction 
{
    public abstract int NumOfProjectiles { get; set; }

    public abstract void AttackFunction(List<PoolObjectQueue<GenericProjectile>> projectilePools, int numOfProjectiles,
        Quaternion direction, Vector2 originPosition, Vector2 SingleTargetPosition, Effectable singleTarget);

    public event Action onAttack; 
    
    /*public event Action<List<PoolObjectQueue<GenericProjectile>>, int,
        Quaternion, Vector2, Effectable, Effectable[],
        float, float, int, List<ProjectileExitPoint> ,List<ProjectileFinalPoint>> attack;*/
    public void Attack(List<PoolObjectQueue<GenericProjectile>> projectilePools, Quaternion direction, Vector2 originPosition,
        Vector2 SingleTargetPosition, Effectable singleTarget)
    {
        onAttack?.Invoke();
        AttackFunction(projectilePools, NumOfProjectiles, direction, originPosition,SingleTargetPosition, singleTarget);
    }
}
