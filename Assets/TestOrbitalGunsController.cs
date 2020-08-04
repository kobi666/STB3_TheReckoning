using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestOrbitalGunsController : OrbitalGunsControllerGeneric
{
    // Start is called before the first frame update
    public override WeaponRotator Rotator {get; set;}
    public override List<OrbitalWeaponGeneric> OrbitalGuns {get; set;}


    public override void PostAwake() {
        
        Rotator = GetComponent<WeaponRotator>() ?? null;
        Rotator.parentTowerComponent = this;
        OrbitalGuns =  new List<OrbitalWeaponGeneric>();
        
    }

    private void Start() {
        //TestInput.instance.onW += AddOrbitalGun;
        AddOrbitalGun();
    }
}
