using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasEffects
{
    List<Effect> GetEffectList();
    void UpdateEffect(Effect ef, List<Effect> appliedEffects);
}

public interface IHasEffectAnimation
{
    List<EffectAnimationController> EffectAnimationControllers { get; set; }
    void InitEffectAnimation();
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


public interface ISpawnsPlayerUnits
{
    List<PlayerUnitPoolCreationData> Units { get; set; }
    Dictionary<string, PoolObjectQueue<PlayerUnitController>> UnitPools { get; set; }

    void InitPools();

}


