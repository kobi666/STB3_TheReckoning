using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using Random = UnityEngine.Random;

[System.Serializable]
public class ProjectileMovementFunction
{
    public virtual void MovementFunction(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos,
        Vector2 TargetPos,
        float speed)
    {
        Debug.LogWarning("NULL MOVEMENT FUNCTION");
    }

    public event Action onTargetLost;
    private bool TargetLost
    {
        get => targetLost;
        set
        {
            onTargetLost?.Invoke();
            targetLost = value;
        }
    }
    private bool targetLost = false;

    public float speed;

    [ShowInInspector] protected float ProgressCounter = 0;

    [ShowInInspector]
    public bool ExternalMovementLock { get; set; } = false;

    public void StopMoving()
    {
        ExternalMovementLock = false;
    }

    public event Action onPositionReached;

    public void OnPositionReached()
    {
        if (posreachedLock == false)
        {
            onPositionReached?.Invoke();
            posreachedLock = true;
        }
    }

    private bool posreachedLock = false;
    
    
    public async void MoveToTargetPosition(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos, Vector2 TargetPos
        )
    {
        while (ProgressCounter <= 1f && ExternalMovementLock == false)
        {
            ProgressCounter += speed * StaticObjects.instance.DeltaGameTime;
            MovementFunction(projectileTransform, null, originPos, TargetPos, speed);
            await Task.Yield();
        }

        ProgressCounter = 0;
        OnPositionReached();
        posreachedLock = false;
    }

    public async void MoveToTargetTransform(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos,
        Vector2 targetPos)
    {
        Vector2 cachedPosition = targetTarnsform.position;
        while (ProgressCounter <= 1f && ExternalMovementLock == false)
        {
            ProgressCounter += StaticObjects.instance.DeltaGameTime * speed;
            if (targetLost == false) {
                if (targetTarnsform != null)
                {
                    cachedPosition = (Vector2) targetTarnsform.position;
                }
                else
                {
                    targetLost = true;
                }
            }
            /*cachedPosition = targetTarnsform
                ? (Vector2)targetTarnsform.position
                : cachedPosition;*/
            MovementFunction(projectileTransform, null, originPos, cachedPosition, speed);
            await Task.Yield();
        }
        OnPositionReached();
        ProgressCounter = 0;
        posreachedLock = false;
    }

}



public class MoveStraight : ProjectileMovementFunction
{
    public override void MovementFunction(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos, Vector2 TargetPos,
        float speed)
    {
        projectileTransform.position = Vector2.Lerp(originPos, TargetPos , ProgressCounter);
    }
}










