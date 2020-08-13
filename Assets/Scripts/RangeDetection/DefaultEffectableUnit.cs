using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DefaultEffectableUnit : MonoBehaviour
{
    UnitController unitController;
    public event Action unitDamaged;
    public event Action unitPoisioned;
    public event Action unitFroze;
    public event Action unitExploded;
    public virtual void Damage(int damageAmount) {
        unitController.LifeManager.DamageToUnit(damageAmount);
    }

    public virtual void Explosion(float explosionValue) {

    }

    public virtual void Poision(int poisionAmount,float poisionDuration) {

    }

    public virtual void Freeze(float freezeAmount, float totalFreezeProbability) {

    }

    
    void Awake()
    {
        unitController = GetComponent<UnitController>() ?? null;
    }
    
    
}
