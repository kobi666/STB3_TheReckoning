﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponRotator))]
public abstract class OrbitalGunsController : TowerComponent
{
    public abstract WeaponRotator Rotator {get; set;}
    public abstract List<OrbitalWeaponGeneric> OrbitalGuns {get; set;}

    

    public virtual void AddOrbitalGun() {
        if (OrbitalGuns.Count < Data.orbitalData.maxNumberOfOrbitals) {
            //TowerUtils.AddOrbitalGun(this, Rotator, Data.OrbitalGunPrefab);
        }
    }

    


    

    
}
