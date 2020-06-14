using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestOrbitalGun : OrbitalWeapon
{
    // Start is called before the first frame update
    WeaponRotator rotator;
    public override WeaponRotator Rotator {get => rotator; set { rotator = value;}}
    public override void MainAttackFunction() {
        TowerWeaponAttacks.TestDebugRay(this);
    }
    
    public override GameObject referenceGOforRotation {get;set;}
    public override bool ShouldRotate {get; set;}
    public override void PostStart() {
        Rotator = transform.parent.GetComponent<WeaponRotator>() ?? null;
        onAttackInitiate += DisableOrbitingInRotator;
        onAttackCease += EnableOrbitingInRotator;
    }


    

}
