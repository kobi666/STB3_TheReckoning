using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class WeaponAttack : IHasEffects,IHasRangeComponent
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

    public abstract List<TagDetector> GetRangeDetectors();


    public abstract void UpdateRange(float RangeSizeDelta, List<TagDetector> detectors);

}
