using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class TestProjectileWeapon : WeaponController
{
    public override void InitiateAttackSequence() {

    }
    public override void CeaseAttackSequence() {

    }
    IEnumerator attackCoroutine;
    public override IEnumerator AttackCoroutine {get => attackCoroutine ; set {attackCoroutine = value;}}
    public override event Action onAttack;
    public override void OnAttack() {
        onAttack?.Invoke();
    }

    public override void PostAwake() {

    }

    public override void PostStart() {
        
    }
}
    

   
        
    

