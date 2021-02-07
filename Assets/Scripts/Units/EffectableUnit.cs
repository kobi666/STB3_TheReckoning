using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectableUnit : Effectable
{
    public UnitController unitController;
    public GenericUnitController GenericUnitController;
    public override bool IsTargetable() {
        return unitController.IsTargetable();
    }

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
        unitController = GetComponent<UnitController>();
    }
}
