using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using BansheeGz.BGSpline.Curve;

[System.Serializable]
public abstract class SplineMovementFunction
{
    public abstract void MovementFunction(Vector2 targetPosition);
    
    public bool Oscilating;
    [ShowIf("Oscilating")] public (float, float) beamOsciliationOffsetMinMax { get; set; } = (0.05f,0.05f);
    [ShowIf("Oscilating")] public float OscilationSpeed { get; set; } = 0.01f;

    public event Action onTargetPositionReached;

    private bool targetPositionReached = false;
    [ShowInInspector]
    private bool TargetPositionReached
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
    private event Action<Vector2> onFinalPointMovement; 

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

    public abstract void InitSplineProperties();

    public void TeleportFinalPoint(Vector2 targetPosition)
    {
        finalPoint.Position = targetPosition;
        TargetPositionReached = true;
    }

    public void initMovment(Vector2 targetPosition)
    {
        finalPoint.Position = exitPoint.transform.position;
        distanceCounter = 0;
    }

    public void FinalPointTravelToTarget(Vector2 targetPosition)
    {
        //distanceCounter += Vector2.Distance()
            //finalPoint.Position = Vector2.Lerp(exitPoint.transform.position, targetPosition, distanceCounter);
            finalPoint.Position = Vector2.MoveTowards(finalPoint.Position, targetPosition,
                TravelSpeed * StaticObjects.Instance.DeltaGameTime);
            /*if (distanceCounter >= 1)
        {
            
        }*/
            
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
            CurrentBeamWidth += StaticObjects.Instance.DeltaGameTime * OscilationSpeed;
        }
        else
        {
            CurrentBeamWidth -= StaticObjects.Instance.DeltaGameTime * OscilationSpeed;
        }
    }

    
    
    
    
    
}


public class StraightLaZor : SplineMovementFunction
{
    public override void MovementFunction(Vector2 targetPosition)
    {
        splineController.points[0].PositionLocal = exitPoint.transform.position;
        splineController.points[1].PositionLocal = finalPoint.Position;
    }

    public override void InitSplineProperties()
    {
        splineController.BgCurve.AddPoint(splineController.BgCurve.CreatePointFromWorldPosition(
            exitPoint.transform.position,
            BGCurvePoint.ControlTypeEnum.Absent), 0);
        /*splineController.BgCurve.CreatePointFromWorldPosition(exitPoint.transform.position,
            BGCurvePoint.ControlTypeEnum.Absent);*/
        splineController.BgCurve.AddPoint(
            splineController.BgCurve.CreatePointFromWorldPosition(finalPoint.transform.position,
                BGCurvePoint.ControlTypeEnum.Absent), 1);
    }
}