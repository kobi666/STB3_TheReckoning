using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;




public abstract class TestOrbitalWeapon : TestWeaponController, IOrbital<TestOrbitalWeapon>
{
    WeaponRotator rotator;
    public WeaponRotator Rotator {get => rotator;set { rotator = value;}}
    [SerializeField]
    public float OrbitingSpeed {get => Data.OrbitingSpeed; set {Data.OrbitingSpeed = value;}}

    [SerializeField]
    public float RotationSpeed {get => Data.RotationSpeed; set {Data.RotationSpeed =value;}}
    public string OrbitalName {get => name;}
    public Transform OrbitalTransform {get => transform;}
    public abstract bool ShouldRotate {get;set;}
    
    public Transform OrbitBase{get => Data.OrbitBase;set { Data.OrbitBase = value;}}
    public float DistanceFromOrbitalBase {get => Data.DistanceFromRotatorBase;set { DistanceFromOrbitalBase = value;}}
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

    bool AsyncRotationInProgress = false;

    public void StopAsyncRotation() {
        AsyncRotationInProgress = false;
    }
    public async void StartAsyncRotation() {
        if (AsyncRotationInProgress == true) {
            AsyncRotationInProgress = false;
            await Task.Yield();
        }
        AsyncRotationInProgress = true;
        while (ShouldRotate && AsyncRotationInProgress == true) {
            DefaultRotationFunction();
            await Task.Yield();
        }
        AsyncRotationInProgress = false;
        InAttackState = false;
    }


    public virtual void DefaultRotationFunction() {
        Vector2 vecToTarget = Target.transform.position - transform.position;
        float angleToTarget = Mathf.Atan2(vecToTarget.y, vecToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, StaticObjects.instance.DeltaGameTime * Data.RotationSpeed);
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

    public void EnableOrbitingInRotator() {
        try {
        Rotator?.EnableRotationForOrbital(name);
        }
        catch(Exception e) {
            Debug.LogWarning(e.Message);
        }
        if (Rotator == null) {
            Debug.LogWarning("Rotator is Null!!");
        }
    }

    public void DisableOrbitingInRotator() {
        Rotator?.DisableRotationForOrbital(name);
        if (Rotator == null) {
            Debug.LogWarning("Rotator is Null!!");
        }
    }

    public virtual void StartRotatingTowardsTarget() {
        StopRotating();
        RotationCoroutine = WeaponUtils.RotateTowardsEnemyTargetUnit(transform, Data.EnemyTarget, RotationSpeed);
        StartCoroutine(RotationCoroutine);
    }

    public virtual void StartIdleRotation() {
        StopOrbiting();
        RotationCoroutine = WeaponUtils.IdleRotation();
    }

    public virtual void StopRotating() {
        if (RotationCoroutine != null) {
            StopCoroutine(RotationCoroutine);
        }
        RotationCoroutine = null;
    }


    IEnumerator orbitingCoroutine;
    public IEnumerator OrbitingCoroutine {get => orbitingCoroutine;set {orbitingCoroutine = value;}}

    IEnumerator rotationCoroutine;

    public IEnumerator RotationCoroutine {get => rotationCoroutine; set {rotationCoroutine = value;}}
    
}
