using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEffector : MonoBehaviour
{
    UnitController unitController;

    public virtual void Damage(int damageAmount) {
        unitController.LifeManager.DamageToUnit(damageAmount);
    }

    public virtual void Explosion(float explosionValue) {

    }

    public virtual void Poision(int posisionAmount,float poisionDuration) {

    }

    public virtual void Freeze(float freezeAmount, float totalFreezeProbability) {

    }

    
    void Awake()
    {
        unitController = GetComponent<UnitController>() ?? null;
    }
    // Start is called before the first frame update
    
}
