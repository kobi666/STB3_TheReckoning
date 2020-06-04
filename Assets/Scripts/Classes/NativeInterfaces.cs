﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHasOrbitals
{
    Dictionary<string,(WeaponController,int)> Orbitals {get;set;}
    bool CanRotate {get;}

    
}

public interface IOrbital<T> {

    Transform OrbitalTransform {get;}
    string OrbitalName {get;}
    bool ShouldRotate {get;set;}
    Transform OrbitBase{get;set;}
    float RoatationSpeed {get;set;}
    float DistanceFromOrbitalBase {get;set;}
    GameObject referenceGOforRotation {get;set;}

    float AngleForOrbit {get;set;}

    void ReStartOrbiting();
    void StopOrbiting();
        
    IEnumerator OrbitingCoroutine {get;set;}
}

