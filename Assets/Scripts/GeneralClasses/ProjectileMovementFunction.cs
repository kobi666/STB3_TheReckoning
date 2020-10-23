using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Threading.Tasks;

[System.Serializable]
public abstract class ProjectileMovementFunction
{
    public abstract void MovementFunction(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos, Vector2 TargetPos,
        float speed,
        float assistingFloat1, float assistinfloat2);
    
    [ShowInInspector]
    public abstract float speed { get; set; }
    
    [ShowInInspector]
    protected float ProgressCounter;
    
    public abstract float assistingFloat1 { get; set; }
    public abstract float assistingFloat2 { get; set; }
    
    public bool ExternalMovementLock { get; set; }
    
    
    public async void MoveToTargetPosition(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos, Vector2 TargetPos
        ,Action onPositionReachedAction)
    {
        while ((Vector2)projectileTransform.position != TargetPos && ExternalMovementLock == false)
        {
            MovementFunction(projectileTransform, null, originPos, TargetPos, speed, assistingFloat1,
                    assistingFloat2);
            await Task.Yield();
        }
        onPositionReachedAction.Invoke();
    }

    public async void MoveToTargetTransform(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos,
        Vector2 TargetPos
        , Action onPositionReachedAction)
    {
        Vector2 cachedPosition = targetTarnsform.position;
        while ((Vector2)projectileTransform.position != cachedPosition && ExternalMovementLock == false)
        {
            cachedPosition = (targetTarnsform.gameObject.activeSelf)
                ? (Vector2)targetTarnsform.position
                : cachedPosition;
            MovementFunction(projectileTransform, null, originPos, cachedPosition, speed, assistingFloat1,
                assistingFloat2);
            await Task.Yield();
        }
        onPositionReachedAction.Invoke();
    }

    
    
    
    
}

[System.Serializable]
public delegate void ProjectileMovementDelegate(Transform projectileTransform, Vector2 originPos, Vector2 TargetPos, Vector2 assistingPos,
    float speed,
    float assistingFloat1, float assistinfloat2, ref float referenceFloat);





