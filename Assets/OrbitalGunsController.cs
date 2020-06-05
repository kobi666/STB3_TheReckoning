﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(WeaponRotator))]
public abstract class OrbitalGunsController : TowerComponent
{
    public abstract WeaponRotator Rotator {get; set;}
    public abstract List<OrbitalWeapon> OrbitalGuns {get; set;}

    

    public virtual void AddOrbitalGun() {
        if (OrbitalGuns.Count < Data.MaxNumberOfOrbitals) {
            TowerUtils.AddOrbitalGun(this, Rotator, Data.OrbitalGunPrefab);
        }
    }

    


    

    
}