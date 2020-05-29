using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHasOrbitals
{
    Dictionary<string,(WeaponController,int)> Orbitals {get;set;}
    bool CanRotate {get;}

    
}

public interface IOrbital {
    float RoatationSpeed {get;set;}
    float DistanceFromOrbitalBase {get;set;}
}

