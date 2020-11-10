using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SplineBehavior
{
    public BeamDynamicData BeamDynamicData = new BeamDynamicData();
    public ProjectileFinalPoint FinalPoint;
    public ProjectileExitPoint ExitPoint;
    public (float, float) SplineWidthStartEnd { get; set; } = (0.12f, 0.12f);
    public abstract void RenderSpline(Vector2 originPosition, Vector2 targetPosition);
    public bool SingleTargetBeam;
    public bool FinalPointTravelsToTarget;
    [ShowIf("FinalPointTravelsToTarget")]
    public float FinalPointTravelSpeed = 0.1f;

    public bool AreaEffect;
    [ShowIf("AreaEffect")] public float EffectRadius;
    [ShowIf("HitsTargetsAlongBeam")] public int MaxTargets;
    public bool HitsTargetsAlongBeam;
    [ShowIf("HitsTargetsAlongBeam")]
    public bool AlwaysReachesTarget;

    public virtual void TargetingMovement()
    {
        if (!FinalPointTravelsToTarget)
        {
            FinalPoint.Position = BeamDynamicData.Target?.transform.position ?? FinalPoint.transform.position;
        }
        else
        {
            FinalPoint.Position = Vector2.MoveTowards(FinalPoint.Position, BeamDynamicData.Target.transform.position,
                FinalPointTravelSpeed * StaticObjects.instance.DeltaGameTime);
        }
    }
}
