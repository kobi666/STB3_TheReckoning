using System;
using System.Collections.Generic;

public interface IHasEffects
{
    List<Effect> GetEffectList();
    void UpdateEffect(Effect ef, List<Effect> appliedEffects);
    void SetEffectList(List<Effect> effects);
}

public interface IHasEffectAnimation
{
    List<EffectAnimationController> EffectAnimationControllers { get; set; }
    void InitEffectAnimation();
}


public interface IHasRangeComponents
{
    float rangeSize { get; set; }
    List<CollisionDetector> GetTagDetectors();
    void UpdateRange(float RangeSizeDelta, List<CollisionDetector> detectorsToApplyChangeOn);
    
}

public interface IhasExitAndFinalPoint
{
    List<ProjectileFinalPoint> GetFinalPoints();
    void SetInitialFinalPointPosition();
    
    List<ProjectileExitPoint> GetExitPoints();
    void SetInitialExitPointPosition();

}




public interface ITargeter
{
    event Action<int> onTargetSet;
}


