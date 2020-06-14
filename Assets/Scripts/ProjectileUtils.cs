using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileUtils 
{
    public static Projectile SpawnProjectileFromPool(PoolObjectQueue<Projectile> pool) {
        return pool.Get();
    }

    public static void SpawnProjectileWithTargetEnemyController(PoolObjectQueue<Projectile> pool, Vector2 exitPoint, EnemyUnitController targetUnit) {
        Projectile proj = SpawnProjectileFromPool(pool);
        proj.transform.position = exitPoint;
        proj.TargetUnit = targetUnit;
    }

    public static void SpawnProjectileWithTargetPosition(PoolObjectQueue<Projectile> pool, Vector2 exitPoint, Vector2 targetPosition) {
        Projectile proj = SpawnProjectileFromPool(pool);
        proj.transform.position = exitPoint;
        proj.TargetPosition = targetPosition;
    }


    
    
}
