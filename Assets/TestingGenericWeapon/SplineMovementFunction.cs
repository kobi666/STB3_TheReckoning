using Sirenix.OdinInspector;
using UnityEngine;
using System;
using BansheeGz.BGSpline.Curve;

[System.Serializable]
public class SplineMovementFunction
{
    public virtual void MovementFunction(Vector2 targetPosition)
    {
        
    }

    [HideInInspector] public bool ParentIsRotatingTowardsTarget;
    
    public bool Oscilating;
    [ShowIf("Oscilating")] public (float, float) beamOsciliationOffsetMinMax { get; set; } = (0.05f,0.05f);
    [ShowIf("Oscilating")] public float OscilationSpeed { get; set; } = 0.01f;

    public event Action onTargetPositionReached;
    
    [ShowInInspector]
    private bool targetPositionReached = false;
    
    public bool TargetPositionReached
    {
        get => targetPositionReached;
        set
        {
            if (value == true)
            {
                if (targetPositionReached == false)
                {
                    onTargetPositionReached?.Invoke();
                }
            }

            targetPositionReached = value;
        }
    }

    private void EnableSplineRender()
    {
        splineController.LineRenderer.enabled = true;
    }

    private void DisableSplineRender()
    {
        splineController.LineRenderer.enabled = false;
    }
    
    public event Action onMovementStart;
    public event Action onMovementEnd;

    public void OnMovementEnd()
    {
        onMovementEnd?.Invoke();
    }

    public void OnMovementStart(Effectable ef, Vector2 pos)
    {
        onMovementStart?.Invoke();
    }

    private float distanceCounter;

    [ShowInInspector]
    private float movementCounter;
    private bool AsyncMovementInProgress;

    public bool FinalPointTeleportsToTarget;

    [HideIf("FinalPointTeleportsToTarget")]
    public float TravelSpeed = 0.5f;
    [HideInInspector]
    public ProjectileExitPoint exitPoint;
    [HideInInspector]
    public SplineController splineController;
    [HideInInspector]
    public ProjectileFinalPoint finalPoint;
    public event Action<Vector2> onFinalPointMovement;


    public event Action<Vector2> onFinalPointUpdate; 
    public void Initialize(SplineBehavior sb)
    {
        splineController = sb.SplineController;
        exitPoint = sb.ExitPoint;
        finalPoint = sb.FinalPoint;
        if (FinalPointTeleportsToTarget)
        {
            onFinalPointMovement += TeleportFinalPoint;
        }
        else
        {
            onFinalPointMovement += FinalPointTravelToTarget;
        }
        InitSplineProperties();
    }

    public virtual void InitSplineProperties()
    {
        
    }

    public void TeleportFinalPoint(Vector2 targetPosition)
    {
        finalPoint.transform.position = targetPosition;
        onFinalPointUpdate?.Invoke(targetPosition);
        TargetPositionReached = true;
    }

    public void ResetMovementProperties()
    {
        finalPoint.transform.position = exitPoint.transform.position;
        distanceCounter = 0;
    }
    
    
    public void FinalPointTravelToTarget(Vector2 targetPosition)
    {
        if (!ParentIsRotatingTowardsTarget) {
            Vector2 position = finalPoint.transform.position;
            position = Vector2.MoveTowards(position, targetPosition,
                TravelSpeed * StaticObjects.DeltaGameTime);
            finalPoint.transform.position = position;
            onFinalPointUpdate?.Invoke(position);
            if (!targetPositionReached)
            {
                if (finalPoint.transform.position == (Vector3)targetPosition)
                {
                    TargetPositionReached = true;
                }
            }
        }
        else
        {
            Vector2 PosXOnly = new Vector2(targetPosition.x, finalPoint.transform.position.y);
            finalPoint.transform.position = Vector2.MoveTowards(finalPoint.transform.position, PosXOnly,
                TravelSpeed * StaticObjects.DeltaGameTime);
            if (!targetPositionReached)
            {
                if (finalPoint.transform.position == (Vector3)targetPosition)
                {
                    TargetPositionReached = true;
                }
            }
        }

    }


    public void MoveSpline(Vector2 targetPosition)
    {
        onFinalPointMovement?.Invoke(targetPosition);
        if (Oscilating)
        {
            OscilateBeam();
        }

        MovementFunction(targetPosition);
    }
    
    
    
    
    public float BeamWidth;
    public virtual float CurrentBeamWidth
    {
        get => BeamWidth;
        set
        {
            BeamWidth = value;
            splineController.LineRenderer.startWidth = value;
            splineController.LineRenderer.endWidth = value;
        }
    }
    
    private bool _widthDirection;

    public virtual void OscilateBeam()
    {
        if (CurrentBeamWidth <= CurrentBeamWidth - beamOsciliationOffsetMinMax.Item1)
        {
            _widthDirection = true;
        }
        else if (CurrentBeamWidth >= CurrentBeamWidth + beamOsciliationOffsetMinMax.Item2)
        {
            _widthDirection = false;
        }

        if (_widthDirection == true)
        {
            CurrentBeamWidth += StaticObjects.DeltaGameTime * OscilationSpeed;
        }
        else
        {
            CurrentBeamWidth -= StaticObjects.DeltaGameTime * OscilationSpeed;
        }
    }

    
    
    
    
    
}


public class StraightLaZor : SplineMovementFunction
{
    public override void MovementFunction(Vector2 targetPosition)
    {
        splineController.points[0].PositionWorld = exitPoint.transform.position;
        splineController.points[splineController.points.Length - 1].PositionWorld = finalPoint.transform.position;
    }

    public override void InitSplineProperties()
    {
        splineController.BgCurve.AddPoint(splineController.BgCurve.CreatePointFromLocalPosition(
            exitPoint.transform.position,
            BGCurvePoint.ControlTypeEnum.Absent), 0);
        /*splineController.BgCurve.CreatePointFromWorldPosition(exitPoint.transform.position,
            BGCurvePoint.ControlTypeEnum.Absent);*/
        splineController.BgCurve.AddPoint(
            splineController.BgCurve.CreatePointFromLocalPosition(finalPoint.transform.position,
                BGCurvePoint.ControlTypeEnum.Absent), 1);
    }
}