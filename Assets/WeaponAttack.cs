using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponAttack
{
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
}
