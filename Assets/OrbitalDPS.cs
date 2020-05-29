using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class OrbitalDPS : TowerComponent
{
    public abstract Vector2[] OrbitalPositions {get;}
    
    public Vector2 myPos {get {
        return transform.position;
    }}
    public abstract WeaponController[] Orbitals {get;set;}

    
    

    public override void PostStart() {
    }
    
}
