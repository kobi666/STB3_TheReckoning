using System;
using BansheeGz.BGSpline.Components;
using Sirenix.OdinInspector;
using UnityEngine;

public class PathWalker : MonoBehaviour
{
    [Required]
    public UnitMovementController UnitMovementController;
    private SpriteRenderer SpriteRenderer;
    public BGCcMath spline;
    public SplinePathController SplinePathController;
    [HideInInspector]
    public bool SplineAttached;


    public Vector2 FindClosestPointToCurrentPosition()
    {
        Vector2 pos = new Vector2();
        
        return pos;
    }

    public GenericUnitController parentUnit;

    public bool PathDirection = true;

    private void OnEnable()
    {
        EndOfPathReached = false;
    }

    public float ProximityToPathEnd
    {
        get
        {
            if (PathDirection == true) { 
            return SplineTotalLength - CurrentDistanceOnSpline;
            }
            else
            {
                return 0 + CurrentDistanceOnSpline;
            }
        }
    }
    public BGCcMath Spline
    {
        get => spline;
        set
        {
            spline = value;
            if (spline != null)
            {
                SplineAttached = true;
            }
            else
            {
                SplineAttached = false;
            }
        }
    }

    public float SplineTotalLength = 0;
    [SerializeField]
    public Transform ObjectTransform;

    public event Action onPathEnd;
    public event Action onReversePathEnd;

    public event Action<float> onPathMovement;

    public void OnPathMovement(float distanceDelta)
    {
        onPathMovement?.Invoke(distanceDelta);
    }

    public bool OnPath;
    public event Action<bool> onPathShift;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == Spline.name)
        {
            onPathShift?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == Spline.name)
        {
            onPathShift?.Invoke(false);
        }
    }

    private void OnDisable()
    {
        currentDistanceOnSpline = 0;
    }


    private float currentDistanceOnSpline = 999f;

    public bool EndOfPathReached;
    
    [ShowInInspector]
    public float CurrentDistanceOnSpline { 
        get => currentDistanceOnSpline;
        set
        {
            if (!SplineAttached)
            {
                EndOfPathReached = true;
                if (EndOfPathReached)
                {
                    Debug.LogWarning("End of spline Reached because spline was not attached");
                }
                return;
            }
            if (PathDirection) {
                if (CurrentDistanceOnSpline < SplineTotalLength) {
                currentDistanceOnSpline = value;
                }
                if (currentDistanceOnSpline >= SplineTotalLength)
                {
                    /*if (OnPath)
                    {*/
                        if (!EndOfPathReached) {
                        onPathEnd?.Invoke();
                        EndOfPathReached = true;
                        if (EndOfPathReached)
                        {
                            Debug.LogWarning("End of spline Reached because reached Spline End");
                        }
                        }
                    //}
                }
            }
            else if (!PathDirection)
            {
                if (CurrentDistanceOnSpline <= 0) {
                    currentDistanceOnSpline = value;
                }
                if (currentDistanceOnSpline <= 0)
                {
                    /*if (OnPath)
                    {*/
                    if (!EndOfPathReached) {
                        onReversePathEnd?.Invoke();
                        EndOfPathReached = true;
                    }
                    //}
                }
            }
        }
    }
    
    [ShowInInspector]
    private Vector2 TargetPosition
    {
        get => Spline?.CalcPositionByDistance(CurrentDistanceOnSpline) ?? transform.position;
    }

    private float directionInt
    {
        get
        {
            if (PathDirection)
            {
                return 1;
            }
            return -1;
        }
    }
    public void MoveAlongSpline(float distancedelta)
    {
        CurrentDistanceOnSpline += distancedelta * directionInt;
        parentUnit.FlipDirection(TargetPosition);
        parentUnit.FlipDirection(TargetPosition);
        UnitMovementController.MoveAlongPathAsync(TargetPosition);
        //ObjectTransform.position = TargetPosition;
        //transform.position = TartPosition;
    }
    

    private void Start()
    {
        ObjectTransform = transform.parent;
        parentUnit = ObjectTransform.GetComponent<GenericUnitController>();
        currentDistanceOnSpline = 0;
        SplineTotalLength = Spline?.GetDistance() ?? 0;
        onPathShift += delegate(bool b) { OnPath = b; };
        onPathMovement += MoveAlongSpline;
        Spline = SplinePathController?.BgCcMath ?? Spline;
    }
}
