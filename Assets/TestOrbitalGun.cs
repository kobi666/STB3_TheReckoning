using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestOrbitalGun : OrbitalWeapon
{
    // Start is called before the first frame update
    
    public override GameObject referenceGOforRotation {get;set;}
    public override float OrbitingSpeed {get => Data.RoatationSpeed; set { Data.RoatationSpeed = value;}}
    public override bool ShouldRotate {get; set;}

    public override event Action onAttack;
    public override void OnAttack() {
        onAttack?.Invoke();
    }

    public Vector2 projectileExitPoint;
    public override Vector2 ProjectileExitPoint {get => projectileExitPoint;}
    
    public override float DistanceFromOrbitalBase {get => Data.distanceFromRotatorBase;set {Data.distanceFromRotatorBase = value;}}

    public override void PostStart() {

    }

    public override void PostAwake() {

    }

}
