using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class WeaponAttack : IHasEffects,IHasRangeComponents,IhasExitAndFinalPoint
{
    public GenericWeaponController parentWeaponController;
    public virtual void Attack(Effectable singleTarget, Vector2 SingleTargetPosition)
    {
        
    }

    public virtual void StopAttack()
    {
        
    }

    public virtual void InitlizeAttack(GenericWeaponController weapon)
    {
        
    }

    public virtual List<Effect> GetEffects()
    {
        return null;
    }

    public virtual List<Effect> GetEffectList()
    {
        return null;
    }

    public virtual void UpdateEffect(Effect ef, List<Effect> appliedEffects)
    {
        
    }

    public float rangeSize { get; set; }
    public abstract List<TagDetector> GetTagDetectors();


    public abstract void UpdateRange(float RangeSizeDelta, List<TagDetector> detectors);

    public abstract List<ProjectileFinalPoint> GetFinalPoints();
    public abstract void SetInitialFinalPointPosition();
    

    public abstract List<ProjectileExitPoint> GetExitPoints();
    public abstract void SetInitialExitPointPosition();

}
