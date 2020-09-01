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
        proj.EffectableTarget = targetEffectable;
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
        proj.gameObject.SetActive(true);
        return proj;
    }

    public static Projectile SpawnArcingAOEProjectile(PoolObjectQueue<Projectile> pool, Vector2 exitPoint, Vector2 targetPosition, float arcValue, float speed, float effectRadius, Action<Effectable> onReachAction)
    {
        ArcingAOEProjectileController proj = pool.GetInactive() as ArcingAOEProjectileController;
        proj.transform.position = exitPoint;
        proj.Data.ArcValue = arcValue;
        proj.Data.EffectRadius = effectRadius;
        proj.Speed = speed;
        proj.TargetPosition = targetPosition;
        proj.placeholderAction = onReachAction;
        proj.gameObject.SetActive(true);
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

    public static Vector2 GetArcingMiddlePosition(Vector2 initPos, Vector2 targetPos, float arcValue)
    {
        Vector2 arcDirection = Vector2.up;
        if (arcValue < 0) {
            arcDirection = Vector2.down;
        }
        Vector2 middlePos = initPos + (targetPos - initPos) / 2 + arcDirection * Mathf.Abs(arcValue);

        return middlePos;
    }
    public static void MoveInArcToPosition(Transform projectileTransform, Vector2 initPos, Vector2 middlePos,
        Vector2 targetPosition, ref Vector2 initToMiddle, ref Vector2 middleToTarget, float speed, ref float counter)
    {
        if (counter <= 1f)
        {
            counter += speed * StaticObjects.instance.DeltaGameTime;
            initToMiddle = Vector2.Lerp(initPos, middlePos, counter);
            middleToTarget = Vector2.Lerp(middlePos, targetPosition, counter);
            projectileTransform.position = Vector2.Lerp(initToMiddle, middleToTarget, counter);
        }
        
    }
    

    public static IEnumerator MoveInArcAndInvokeActionCoroutine(Transform projectileTransform, Vector2 targetPos, float arcValue, float speed, Action action) {
    float movementSpeed = speed;
    Vector2 initPos = projectileTransform.position;
    Vector2 arcDirection = Vector2.up;
    if (arcValue < 0) {
        arcDirection = Vector2.down;
    }
    Vector2 middlePos = initPos + (targetPos - initPos) / 2 + arcDirection * Mathf.Abs(arcValue);
    float count = 0;
    while (count < 0.97f) {
        count += StaticObjects.instance.DeltaGameTime * movementSpeed;
        Vector2 m1 = Vector2.Lerp( initPos, middlePos, count );
        Vector3 m2 = Vector2.Lerp( middlePos, targetPos, count );
        projectileTransform.position = Vector2.Lerp(m1, m2, count);
        projectileTransform.Rotate(Vector3.forward * 350 * Time.deltaTime, Space.Self);
        yield return new WaitForFixedUpdate();
    }
    action.Invoke();
    yield break;
}
    
    

    






    
    
}
