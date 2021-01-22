using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasEffects
{
    List<Effect> GetEffectList();
    void UpdateEffect(Effect ef, List<Effect> appliedEffects);
}


public interface IHasRangeComponent
{
    List<TagDetector> GetRangeDetectors();
    void UpdateRange(float RangeSizeDelta, List<TagDetector> detectorsToApplyChangeOn);
}

public interface IhasExitAndFinalPoint
{
    List<ProjectileFinalPoint> GetFinalPoints();
    void SetInitialFinalPointPosition();
    
    List<ProjectileExitPoint> GetExitPoints();
    void SetInitialExitPointPosition();

}
