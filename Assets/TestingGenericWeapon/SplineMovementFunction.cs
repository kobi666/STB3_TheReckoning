using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SplineMovementFunction
{
    public abstract void RenderSpline(Vector2 TargetPos);
    public bool Oscilating;
    [HideIf("FinalPointTeleportsToTarget")]
    public bool FinalPointTravelsToTarget;

    [ShowIf("FinalPointTravelsToTarget")] public float travelSpeed { get; set; } = 0.5f;
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
    }

    public void FinalPointTravelToTarget(Vector2 targetPosition)
    {
        finalPoint.Position = Vector2.MoveTowards(finalPoint.Position, targetPosition, travelSpeed * StaticObjects.Instance.DeltaGameTime)
    }
    
    
}