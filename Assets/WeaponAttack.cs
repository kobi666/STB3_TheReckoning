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

    public abstract List<Effect> GetEffectList();

    public abstract void UpdateEffect(Effect ef);

    public abstract void SetEffectList(List<Effect> effects);
    

    public float rangeSize { get; set; }
    public abstract List<CollisionDetector> GetTagDetectors();


    public abstract void UpdateRange(float RangeSizeDelta, List<CollisionDetector> detectors);

    public abstract List<ProjectileFinalPoint> GetFinalPoints();
    public abstract void SetInitialFinalPointPosition();
    

    public abstract List<ProjectileExitPoint> GetExitPoints();
    public abstract void SetInitialExitPointPosition();

}
