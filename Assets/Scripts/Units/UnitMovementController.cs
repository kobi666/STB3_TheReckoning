using System;
using UnityEngine;
using System.Threading.Tasks;

public class UnitMovementController : MonoBehaviour
{
    public Transform UnitTransform;
    public GenericUnitController GenericUnitController;
    public float MovementSpeed = 1;
    private Rigidbody2D Rigidbody2D;
    public AnimationClip MovementAnimation;
    public AnimationController AnimationController;

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        onMovementStart += StartMovementAnimation;
        onMovementEnd += StopMovementAnimation;
    }

    private void StartMovementAnimation()
    {
        if (MovementAnimation != null)
        {
            AnimationController.PlayLoopingAnimation(MovementAnimation);
        }
    }

    private void StopMovementAnimation()
    {
        if (MovementAnimation != null)
        {
            AnimationController.animancer.Stop();
        }
    }

    public void MoveTowardsTarget(Vector2? targetPos)
    {
        GenericUnitController.FlipDirection((Vector2)targetPos);
        transform.position = Vector2.MoveTowards(UnitTransform.position, (Vector2)targetPos,
        MovementSpeed * StaticObjects.DeltaGameTime);
        /*UnitTransform.position = Vector2.MoveTowards(UnitTransform.position, (Vector2)targetPos,
        MovementSpeed * StaticObjects.DeltaGameTime);*/
    }

    public event Action onMovementStart;
    public event Action onMovementEnd;

    private bool pathMovementInProgress = false;

    public bool PathMovementInProgress
    {
        get => pathMovementInProgress;
        set
        {
            pathMovementInProgress = value;
            MovementInProgress = value;
        }
    }

    private bool freeMovementInProgress = false;

    public bool FreeMovementInprogress
    {
        get => freeMovementInProgress;
        set
        {
            freeMovementInProgress = value;
            MovementInProgress = value;
        }
    }
    
    private bool m_MovementInProgress;

    public bool MovementInProgress
    {
        get => m_MovementInProgress;
        set
        {
            if (value != m_MovementInProgress) {
                if (value)
                {
                    onMovementStart?.Invoke();
                }
                else
                {
                    onMovementEnd?.Invoke();
                }

                m_MovementInProgress = value;
            }
        }
    }
    public Vector2? TargetPositionFreeMovement;
    public Vector2? TargetPositionOnPath;
    public async void MoveTowardsTargetAsync(Vector2? targetPosition)
    {
        TargetPositionFreeMovement = targetPosition;
        if (!FreeMovementInprogress)
        {
            FreeMovementInprogress = true;
            while (FreeMovementInprogress)
            {
                //need to fix this not right now
                MovementInProgress = true;
                //Debug.LogWarning("moving free");
                GenericUnitController.FlipDirection(TargetPositionFreeMovement);
                transform.position = Vector2.MoveTowards(UnitTransform.position, (Vector2)TargetPositionFreeMovement,
                    MovementSpeed * StaticObjects.DeltaGameTime);
                await Task.Yield();
            }

            FreeMovementInprogress = false;
        }
    }

    public async void MoveAlongPathAsync(Vector2 targetPosition)
    {
        TargetPositionOnPath = targetPosition;
        
        if (!PathMovementInProgress)
        {
            PathMovementInProgress = true;
            while (PathMovementInProgress)
            {
                MovementInProgress = true;
                //Debug.LogWarning("moving path");
                GenericUnitController.FlipDirection(TargetPositionOnPath);
                transform.position = (Vector2)TargetPositionOnPath;
                await Task.Yield();
            }
            PathMovementInProgress = false;
        }
    }


    private void OnDisable()
    {
        MovementInProgress = false;
        PathMovementInProgress = false;
        FreeMovementInprogress = false;
    }
}


