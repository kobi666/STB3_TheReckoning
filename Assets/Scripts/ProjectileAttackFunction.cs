using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ProjectileAttackFunction 
{
    
    //for some reason, the object pools cannot be instantiated as Lists, so a workaround is to create static pools.
    //perhaps arrays or dictionaries will work better...
    
    
    
    public abstract int ProjectileMultiplier { get; set; }
    public abstract void InitializeAttack();

    public abstract void AttackFunction(Effectable singleTarget,
         Vector2 SingleTargetPosition);

    public event Action onAttack; 
    
    /*public event Action<List<PoolObjectQueue<GenericProjectile>>, int,
        Quaternion, Vector2, Effectable, Effectable[],
        float, float, int, List<ProjectileExitPoint> ,List<ProjectileFinalPoint>> attack;*/
    public void Attack(Effectable singleTarget,
        Vector2 singleTargetPosition)
    {
        onAttack?.Invoke();
        AttackFunction(singleTarget, singleTargetPosition );
    }
}
