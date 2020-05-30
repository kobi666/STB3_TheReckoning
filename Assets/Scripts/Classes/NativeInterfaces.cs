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

    public Transform OrbitBase{get;set;}
    float RoatationSpeed {get;set;}
    float DistanceFromOrbitalBase {get;set;}
    GameObject referenceGOforRotation {get;set;}

    void StartRotating();
    void StopRotating();
}

