using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectableUnit : Effectable
{
    public UnitController unitController;
    public GenericUnitController GenericUnitController;
    private bool _targetable;
    public bool targetable
    {
        get
        {
            return _targetable;
        }
        set
        {
            if (value != _targetable)
            {
                _targetable = value;
                onTargetableStateChange?.Invoke(name, _targetable);
            }
        }
    }
    
    

    public bool ExternalTargetableLock { get; set; }
    public override bool IsTargetable()
    {
        if (gameObject.activeSelf == true) {
            if (ExternalTargetableLock)
            {
                if (GenericUnitController.StateMachine.currentState.stateName != UnitStates.Death)
                {
                    targetable = true;
                    return true;
                }
            }
        }

        targetable = false;
        return false;
    }

    public event Action<string, bool> onTargetableStateChange;

    public override void ApplyFreeze(float FreezeAmount, float TotalFreezeProbability) {

    }

    public override void ApplyDamage(int damageAmount) {
        unitController.LifeManager.DamageToUnit(damageAmount);
    }

    public override void ApplyExplosion(float explosionValue) {

    }

    public override void ApplyPoision(int poisionAmount, float poisionDuration) {

    }

    public override void PostStart() {
        unitController = unitController ?? GetComponent<UnitController>();
        GenericUnitController = GenericUnitController ?? GetComponent<GenericUnitController>();

    }
}
