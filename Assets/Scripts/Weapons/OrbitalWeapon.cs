using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class OrbitalWeapon : WeaponController, IOrbital<OrbitalWeapon>
{
    public string OrbitalName {get => name;}
    public Transform OrbitalTransform {get => transform;}
    public abstract bool ShouldRotate {get;set;}
    
    Transform orbitBase;
    public Transform OrbitBase{get => orbitBase;set { orbitBase = value;}}
    public abstract float OrbitingSpeed {get;set;}
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

    public float AngleForRotation {
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

    public virtual IEnumerator DefaultOrbitCoroutine() {
        while (true) {
            AngleForOrbit += StaticObjects.instance.DeltaGameTime * OrbitingSpeed;
            transform.position = (Vector2)OrbitBase.position + (WeaponUtils.DegreeToVector2(AngleForOrbit) * DistanceFromOrbitalBase);
            yield return new WaitForFixedUpdate();
        }
    }

    public virtual void ReStartOrbiting() {
        StopOrbiting();
        //placeHolderCoroutine = Gutils.OrbitAroundTransformNoRotation(transform, OrbitBase, RoatationSpeed, Vector3.back);
        OrbitingCoroutine = DefaultOrbitCoroutine();
        StartCoroutine(OrbitingCoroutine);
    }
    public virtual void StopOrbiting() {
        if (OrbitingCoroutine != null) {
        StopCoroutine(OrbitingCoroutine);
        }
        OrbitingCoroutine = null;
    }
    IEnumerator orbitingCoroutine;
    public IEnumerator OrbitingCoroutine {get => orbitingCoroutine;set {orbitingCoroutine = value;}}
    
}
