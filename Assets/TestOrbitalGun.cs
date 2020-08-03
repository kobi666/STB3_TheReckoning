using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestOrbitalGun : OrbitalWeapon
{
    // Start is called before the first frame update
    public override void MainAttackFunction() {
        TowerWeaponAttacks.TestDebugRay(this);
        ProjectileUtils.SpawnDirectHitTargetFacingProjectile(ProjectilePool,ProjectileExitPoint,ProjectileFinalPointV2,transform.rotation, TargetBank, Damage);
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


    

}
