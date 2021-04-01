using UnityEngine;
using System;

public class DefaultEffectableUnit : MonoBehaviour
{
    
    public event Action unitDamaged;
    public event Action unitPoisioned;
    public event Action unitFroze;
    public event Action unitExploded;
    

    public virtual void Explosion(float explosionValue) {

    }

    public virtual void Poision(int poisionAmount,float poisionDuration) {

    }

    public virtual void Freeze(float freezeAmount, float totalFreezeProbability) {

    }

    
    
    
    
}
