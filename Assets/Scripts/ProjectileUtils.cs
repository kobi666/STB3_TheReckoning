using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public static Projectile SpawnProjectileWithTargetPosition(PoolObjectQueue<Projectile> pool, Vector2 exitPoint, Vector2 targetPosition) {
        Projectile proj = SpawnProjectileFromPool(pool);
        proj.transform.position = exitPoint;
        proj.TargetPosition = targetPosition;
        return proj;
    }

    public static Projectile SpawnTargetPositionFacingProjectile(PoolObjectQueue<Projectile> pool, Vector2 exitPoint, Vector2 targetPosition, Quaternion direction) {
        Projectile proj = SpawnProjectileWithTargetPosition(pool, exitPoint, targetPosition);
        proj.transform.rotation = direction;
        return proj;
    }

    public static void SpawnDirectHitTargetFacingProjectile(PoolObjectQueue<Projectile> pool, Vector2 exitPoint, Vector2 targetPosition, Quaternion direction, EnemyTargetBank targetBank) {
        DirectHitProjectile proj = SpawnTargetPositionFacingProjectile(pool,exitPoint,targetPosition,direction) as DirectHitProjectile;
        proj.TargetBank = targetBank;
    }

    public static void MoveUntilReachedTargetPosition(Transform self, Vector2 targetPosition, float speed, Action onTargetPositionReach) {
        if ((Vector2)self.position != targetPosition) {
        self.position = Vector2.MoveTowards(self.position,targetPosition,speed * StaticObjects.instance.DeltaGameTime);
        }
        else if ((Vector2)self.position == targetPosition) {
            onTargetPositionReach.Invoke();
        }
    }

    






    
    
}
