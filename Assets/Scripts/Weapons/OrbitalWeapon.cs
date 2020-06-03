using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class OrbitalWeapon : WeaponController, IOrbital<OrbitalWeapon>
{
    public string OrbitalName {get => name;}
    public Transform OrbitalTransform {get => transform;}
    public abstract bool ShouldRotate {get;set;}
    public abstract Transform OrbitBase{get;set;}
    public abstract float RoatationSpeed {get;set;}
    public abstract float DistanceFromOrbitalBase {get;set;}
    public abstract GameObject referenceGOforRotation {get;set;}

    float angleForOrbit;
    public float AngleForOrbit {
        get => angleForOrbit;
        set {
            angleForOrbit = value;
            if (value > 360f) {
                angleForOrbit = 0.0f;
            }
            if (value < 0) {
                angleForOrbit = 360f;
            }
        }
    }

    

    public Vector2 AngleToPosition(float angle) {
        return WeaponUtils.DegreeToVector2(angle);
    }

    public virtual void StartOrbiting(IEnumerator placeHolderCoroutine) {
        placeHolderCoroutine = Gutils.OrbitAroundTransformNoRotation(transform, OrbitBase, RoatationSpeed, Vector3.back);
        StartCoroutine(placeHolderCoroutine);
    }
    public virtual void StopOrbiting() {
        StopCoroutine(RotationCoroutine);
    }
    public abstract IEnumerator RotationCoroutine {get;set;}
    
}
