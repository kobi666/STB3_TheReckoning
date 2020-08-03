using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Effector : MonoBehaviour
{
    public  void Damage(Effectable effectableObject, int damageAmount) {
        effectableObject.OnDamage(damageAmount);
    }
    public void Poision(Effectable effectable, int poisionAmount, float poisionDuration) {
        effectable.OnPoision(poisionAmount,poisionDuration);
    }

    public void Freeze(Effectable effectable, float freezeAmount, float totalFreezeProbability) {
        effectable.OnFreeze(freezeAmount,totalFreezeProbability);
    }

    public void Explode(Effectable effectable, float explosionEffectValue) {
        effectable.OnExplosion(explosionEffectValue);
    }

}
