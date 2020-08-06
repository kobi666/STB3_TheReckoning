using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Effector : MonoBehaviour
{
    public virtual void Damage(Effectable effectableObject, int damageAmount) {
        effectableObject.ApplyDamage(damageAmount);
    }
    public virtual void Poision(Effectable effectable, int poisionAmount, float poisionDuration) {
        effectable.ApplyPoision(poisionAmount,poisionDuration);
    }

    public virtual void Freeze(Effectable effectable, float freezeAmount, float totalFreezeProbability) {
        effectable.ApplyFreeze(freezeAmount,totalFreezeProbability);
    }

    public virtual void Explode(Effectable effectable, float explosionEffectValue) {
        effectable.ApplyExplosion(explosionEffectValue);
    }

}
