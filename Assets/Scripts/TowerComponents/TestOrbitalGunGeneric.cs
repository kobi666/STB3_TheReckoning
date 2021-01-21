﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TestOrbitalGunGeneric : OrbitalWeaponGeneric
{
    // Start is called before the first frame update
    public override void MainAttackFunction() {
        TowerWeaponAttacks.TestDebugRay(this);
        ProjectileUtils.SpawnDirectHitTargetFacingProjectile(ProjectileQueuePool,ProjectileExitPoint,ProjectileFinalPointV2,transform.rotation, Damage);
    }
    
    public bool CanAttackField {
        get => canattackFieldPH;
    }

    public override GameObject referenceGOforRotation {get;set;}
    public override bool ShouldRotate {get {
        if (CanAttack()) {
            if (InAttackState == true) {
                return true;
                }
            }
                return false;
        }
        set {}
    }
    public override void PostStart() {
        onAttackInitiate += DisableOrbitingInRotator;
        onAttackInitiate += StartAsyncRotation;
        onAttackCease += EnableOrbitingInRotator;
        onAttackCease += StopAsyncRotation;
    }


    public override void InitComponent()
    {
        throw new NotImplementedException();
    }

    public override List<Effect> GetEffectList()
    {
        throw new NotImplementedException();
    }

    public override void UpdateEffect(Effect ef, List<Effect> appliedEffects)
    {
        throw new NotImplementedException();
    }

    public override List<TagDetector> GetRangeDetectors()
    {
        throw new NotImplementedException();
    }

    public override void UpdateRange(float RangeSizeDelta, List<TagDetector> detectors)
    {
        throw new NotImplementedException();
    }
}
