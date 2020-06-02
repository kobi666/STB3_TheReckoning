using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOrbitalGunsController : OrbitalGunsController
{
    // Start is called before the first frame update
    

    public override WeaponRotator Rotator {get; set;}
    public override List<OrbitalWeapon> OrbitalGuns {get; set;}


    public override void PostStart() {
        
        Rotator = GetComponent<WeaponRotator>() ?? null;
        Rotator.parentTowerComponent = this;
        OrbitalGuns =  new List<OrbitalWeapon>();
        TestInput.instance.onW += AddOrbitalGun;
    }
}
