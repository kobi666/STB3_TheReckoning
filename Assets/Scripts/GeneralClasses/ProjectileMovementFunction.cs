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
public abstract class ProjectileMovementFunction
{
    [HideInInspector]
    public ProjectileMovementDynamicData ProjectileMovementDynamicData = new ProjectileMovementDynamicData();

    public abstract void OnMovementInit(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos,
        Vector2 TargetPos,
        float speed);

    public abstract void OnMovementComplete();
    
    public abstract void MovementFunction(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos,
        Vector2 TargetPos,
        float speed);



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
        OnMovementInit(projectileTransform, targetTarnsform, originPos, TargetPos, Speed);
        while (ProgressCounter <= 1f && ExternalMovementLock == false)
        {
            ProgressCounter += Speed * StaticObjects.DeltaGameTime;
            MovementFunction(projectileTransform, null, originPos, TargetPos, Speed);
            await Task.Yield();
        }

        ProgressCounter = 0;
        OnPositionReached();
        OnMovementComplete();
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
                    cachedPosition = targetTarnsform.position;
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
        OnMovementComplete();
        ProgressCounter = 0;
        posreachedLock = false;
    }

}



public class MoveStraight : ProjectileMovementFunction
{
    public override void OnMovementInit(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos, Vector2 TargetPos,
        float speed)
    {
        
    }

    public override void OnMovementComplete()
    {
        
    }

    public override void MovementFunction(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos, Vector2 TargetPos,
        float speed)
    {
        projectileTransform.position = Vector2.Lerp(originPos, TargetPos , ProgressCounter);
    }
}

public class MoveInArc : ProjectileMovementFunction
{

    public float arcValue;
    private Vector2 MiddlePos;
    private Vector2 InitToMiddle;
    private Vector2 MiddleToTarget;
    public static void MoveInArcToPosition(Transform projectileTransform, Vector2 initPos, Vector2 middlePos,
        Vector2 targetPosition, ref Vector2 initToMiddle, ref Vector2 middleToTarget, float speed, ref float counter)
    {
        if (counter <= 1f)
        {
            counter += speed * StaticObjects.DeltaGameTime;
            initToMiddle = Vector2.Lerp(initPos, middlePos, counter);
            middleToTarget = Vector2.Lerp(middlePos, targetPosition, counter);
            projectileTransform.position = Vector2.Lerp(initToMiddle, middleToTarget, counter);
        }
    }
    
    public override void MovementFunction(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos, Vector2 TargetPos,
        float speed)
    {
         InitToMiddle = Vector2.Lerp(originPos, MiddlePos, ProgressCounter);
         MiddleToTarget = Vector2.Lerp(MiddlePos, TargetPos, ProgressCounter);
         projectileTransform.position = Vector2.Lerp(InitToMiddle, MiddleToTarget, ProgressCounter);
    }
    
    
    public override void OnMovementInit(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos, Vector2 TargetPos,
        float speed)
    {
        Vector2 arcDirection = Vector2.up;
        if ( arcValue < 0) {
            arcDirection = Vector2.down;
        }

        MiddlePos = originPos + (TargetPos - originPos) / 2 + arcDirection * Mathf.Abs(arcValue);
    }

    public override void OnMovementComplete()
    {
        
    }

    
}










