using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OrbitalWeapon : WeaponController
{
    public OrbitalDPS ParentOrbitalComponent;
    public override Vector2 ProjectileExitPoint {get => transform.position;}

    public override event Action onAttack;
    public override void OnAttack() {
        onAttack?.Invoke();
    }
    // Start is called before the first frame update
    public override void PostStart() {

    }
}
