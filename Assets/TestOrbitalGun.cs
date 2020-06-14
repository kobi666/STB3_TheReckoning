using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestOrbitalGun : OrbitalWeapon
{
    // Start is called before the first frame update
    
    public override void MainAttackFunction() {
        TowerWeaponAttacks.TestDebugRay(this);
    }
    
    public override GameObject referenceGOforRotation {get;set;}
    public override bool ShouldRotate {get; set;}
    public override void PostStart() {
        Debug.LogWarning("Thiasdaslkdjaklsd");
        onAttackInitiate += DisableOrbitingInRotator;
        onAttackCease += EnableOrbitingInRotator;
    }


    

}
