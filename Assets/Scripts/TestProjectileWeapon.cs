using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class TestProjectileWeapon : WeaponController
{
    IEnumerator attackCoroutine;
    public override IEnumerator AttackCoroutine {get => attackCoroutine ; set {attackCoroutine = value;}}
    public override event Action onAttack;
    public override void OnAttack() {
        onAttack?.Invoke();
    }

    public override void PostAwake() {

    }

    public override void PostStart() {
        ProjectileExitPoint = new Vector2(transform.position.x - SR.sprite.bounds.extents.x, transform.position.y);
    }
}
    

   
        
    

