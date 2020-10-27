using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Threading.Tasks;

[System.Serializable]
public abstract class ProjectileMovementFunction
{
    public abstract void MovementFunction(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos,
        Vector2 TargetPos,
        float speed);

    public float speed;

    [ShowInInspector] protected float ProgressCounter { get; private set; } = 0;

    [ShowInInspector]
    public bool ExternalMovementLock { get; set; } = false;

    public void StopMoving()
    {
        ExternalMovementLock = false;
    }

    public event Action OnPositionReached;
    
    
    public async void MoveToTargetPosition(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos, Vector2 TargetPos
        )
    {
        while ((Vector2)projectileTransform.position != TargetPos && ExternalMovementLock == false)
        {
            MovementFunction(projectileTransform, null, originPos, TargetPos, speed);
            await Task.Yield();
        }
        OnPositionReached?.Invoke();
    }

    public async void MoveToTargetTransform(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos,
        Vector2 targetPos)
    {
        Vector2 cachedPosition = targetTarnsform.position;
        while ((Vector2)projectileTransform.position != cachedPosition && ExternalMovementLock == false)
        {
            cachedPosition = (targetTarnsform.gameObject.activeSelf)
                ? (Vector2)targetTarnsform.position
                : cachedPosition;
            MovementFunction(projectileTransform, null, originPos, cachedPosition, speed);
            await Task.Yield();
        }
        OnPositionReached?.Invoke();
    }

}



public class MoveStraight : ProjectileMovementFunction
{
    public override void MovementFunction(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos, Vector2 TargetPos,
        float speed)
    {
        projectileTransform.position = Vector2.MoveTowards(projectileTransform.position, TargetPos,
            speed * StaticObjects.instance.DeltaGameTime);
    }
}










