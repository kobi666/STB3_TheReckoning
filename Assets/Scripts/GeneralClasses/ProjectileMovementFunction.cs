using System;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Threading.Tasks;

[Serializable]
public class ProjectileMovementDynamicData
{
    public float Speed;
    public float ArcingValue;
}






[System.Serializable]
public class ProjectileMovementFunction
{
    [HideInInspector]
    public ProjectileMovementDynamicData ProjectileMovementDynamicData = new ProjectileMovementDynamicData();
    public virtual void MovementFunction(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos,
        Vector2 TargetPos,
        float speed)
    {
        
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

    public float Speed;

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
            ProgressCounter += Speed * StaticObjects.DeltaGameTime;
            MovementFunction(projectileTransform, null, originPos, TargetPos, Speed);
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
            ProgressCounter += StaticObjects.DeltaGameTime * Speed;
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
            MovementFunction(projectileTransform, null, originPos, cachedPosition, Speed);
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










