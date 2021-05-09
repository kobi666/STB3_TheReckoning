using System;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

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
    
    public abstract float Speed { get; set; }

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

    public float speed;
    public override float Speed { get => speed; set => speed = value; }
}

public class MoveInArc : ProjectileMovementFunction
{
    public bool RotateProjectile;
    [ShowIf("RotateProjectile")] public float RotationSpeed = 1;
    public bool RandomArc;
    public float arcValue;
    private Vector2 MiddlePos;
    private Vector2 InitToMiddle;
    private Vector2 MiddleToTarget;

    public override void MovementFunction(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos, Vector2 TargetPos,
        float speed)
    {
         InitToMiddle = Vector2.Lerp(originPos, MiddlePos, ProgressCounter);
         MiddleToTarget = Vector2.Lerp(MiddlePos, TargetPos, ProgressCounter);
         projectileTransform.position = Vector2.Lerp(InitToMiddle, MiddleToTarget, ProgressCounter);
         if (RotateProjectile)
         {
             projectileTransform.Rotate(Vector3.forward, RotationSpeed);
         }
    }
    
    public override float Speed { get => speed; set => speed = value; }
    public float speed = 1;

    public override void OnMovementInit(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos, Vector2 TargetPos,
        float speed)
    {
        Vector2 arcDirection = Vector2.up;
        if ( arcValue < 0) {
            arcDirection = Vector2.down;
        }

        if (RandomArc)
        {
            arcValue = Random.Range(2, 6);
        }
        MiddlePos = originPos + (TargetPos - originPos) / 2 + arcDirection * Mathf.Abs(arcValue);
    }

    public override void OnMovementComplete()
    {
        
    }

    
}










