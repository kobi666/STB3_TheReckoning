using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileUtils 
{


    
    public static Projectile SpawnProjectileFromPool(PoolObjectQueue<Projectile> pool) {
        return pool.Get();
    }

    

    public static void SpawnProjectileWithTargetEnemyController(PoolObjectQueue<Projectile> pool, Vector2 exitPoint, Effectable targetEffectable) {
        Projectile proj = SpawnProjectileFromPool(pool);
        proj.transform.position = exitPoint;
        proj.TargetUnit = targetEffectable;
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

    
    public static Projectile SpawnDirectHitTargetFacingProjectile(PoolObjectQueue<Projectile> pool, Vector2 exitPoint, Vector2 targetPosition, Quaternion direction, int damage) {
        Projectile proj = SpawnTargetPositionFacingProjectile(pool,exitPoint,targetPosition,direction);
        proj.Damage = damage;
        return proj;
    }

    public static void MoveStraightUntilReachedTargetPosition(Transform self, Vector2 targetPosition, float speed, Action onTargetPositionReach) {
        if ((Vector2)self.position != targetPosition) {
        self.position = Vector2.MoveTowards(self.position,targetPosition,speed * StaticObjects.instance.DeltaGameTime);
        }
        else if ((Vector2)self.position == targetPosition) {
            onTargetPositionReach.Invoke();
        }
    }

    public IEnumerator MoveInArc(Transform projectileTransform, Vector2 targetPos, float arcValue, float speed, Action action) {
    //point[1] = point[0] +(point[2] -point[0])/2 +Vector3.up *5.0f;
    Vector2 initPos = projectileTransform.position;
    Vector2 arcDirection = Vector2.up;
    if (arcValue < 0) {
        arcDirection = Vector2.down;
    }
    Vector2 middlePos = initPos + (targetPos - initPos) / 2 + arcDirection * Mathf.Abs(arcValue);
    float count = 0;
    while (count < 1.0f) {
        count += StaticObjects.instance.DeltaGameTime * speed;
        Vector2 m1 = Vector2.Lerp( initPos, middlePos, count );
        Vector3 m2 = Vector2.Lerp( middlePos, targetPos, count );
        projectileTransform.position = Vector2.Lerp(m1, m2, count);
        yield return new WaitForFixedUpdate();
    }
    action.Invoke();
    yield break;
}

    






    
    
}
