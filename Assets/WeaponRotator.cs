﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotator : Rotator<OrbitalWeapon>
{
    public OrbitalGunsController parentTowerComponent;
    public TowerComponent ParentTowerComponent {get=> parentTowerComponent;set { ParentTowerComponent = value;}}
    // Start is called before the first frame update
    public override int MaxNumOfOrbitals {
        get => ParentTowerComponent.Data.MaxNumberOfOrbitals;
    }
}
