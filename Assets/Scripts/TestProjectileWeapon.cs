using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class TestProjectileWeapon : WeaponController
{
    
    Vector2 projectileExitPoint;
    public override Vector2 ProjectileExitPoint {
        get => ProjectileExitPoint;
    }
    public override event Action onAttack;
    public override void OnAttack() {
        onAttack?.Invoke();
    }

    public override void PostAwake() {

    }

    public override void PostStart() {
        projectileExitPoint = new Vector2(transform.position.x - SR.sprite.bounds.extents.x, transform.position.y);
    }
}
    

   
        
    

