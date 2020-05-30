using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OrbitalWeapon : WeaponController
{
    
    public Transform OrbitBase{get;set;}
    public float RoatationSpeed {get;set;}
    public float DistanceFromOrbitalBase {get;set;}
    public GameObject referenceGOforRotation {get;set;}

    public void StartRotating() {
        
    }
    public void StopRotating() {

    }
    IEnumerator roationCoroutine;
    
    public override Vector2 ProjectileExitPoint {get => transform.position;}

    public override event Action onAttack;
    public override void OnAttack() {
        onAttack?.Invoke();
    }
    // Start is called before the first frame update
    public override void PostStart() {

    }
}
