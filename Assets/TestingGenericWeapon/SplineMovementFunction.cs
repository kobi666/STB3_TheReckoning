using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;

[System.Serializable]
public class SplineMovementFunction
{
    
    public bool Oscilating;
    [ShowIf("Oscilating")] public (float, float) beamOsciliationOffsetMinMax { get; set; } = (0.05f,0.05f);
    [ShowIf("Oscilating")] public float OscilationSpeed { get; set; } = 0.01f;

    public event Action onFinalPointReached;

    private bool finalPointReached = false;
    [ShowInInspector]
    private bool FinalPointReached
    {
        get => finalPointReached;
        set
        {
            if (value == true)
            {
                if (finalPointReached == false)
                {
                    onFinalPointReached?.Invoke();
                }
            }

            finalPointReached = value;
        }
    }

    private float distanceCounter;

    [ShowInInspector]
    private float movementCounter;
    private bool AsyncMovementInProgress;

    public bool FinalPointTeleportsToTarget;
    private ProjectileExitPoint exitPoint;
    private SplineController splineController;
    private ProjectileFinalPoint finalPoint;

    public void Initialize(SplineBehavior sb)
    {
        splineController = sb.SplineController;
        exitPoint = sb.ExitPoint;
        finalPoint = sb.FinalPoint;
    }

    public void TeleportFinalPoint(Vector2 targetPosition)
    {
        finalPoint.Position = targetPosition;
        FinalPointReached = true;
    }

    public void FinalPointTravelToTarget(Vector2 targetPosition, float travelSpeed)
    {
        distanceCounter = 1 - (Vector2.Distance(exitPoint.transform.position, targetPosition));
        if (distanceCounter <= 1)
        {
            distanceCounter += travelSpeed * StaticObjects.Instance.DeltaGameTime;
            finalPoint.Position = Vector2.Lerp(exitPoint.transform.position, targetPosition, distanceCounter);
        }

        if (distanceCounter >= 1)
        {
            FinalPointReached = true;
        }
    }
    
    private float currentBeamWidth;
    public virtual float CurrentBeamWidth
    {
        get => currentBeamWidth;
        set
        {
            currentBeamWidth = value;
            splineController.LineRenderer.startWidth = value;
            splineController.LineRenderer.endWidth = value;
        }
    }
    
    private bool _widthDirection;

    public virtual void OscilateBeam(float beamDuration)
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