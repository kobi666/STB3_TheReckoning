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
    //IEnumerator attackCoroutine;
    public override IEnumerator AttackCoroutine(WeaponController wp) {
        yield return StartCoroutine(WeaponUtils.TestAttack(wp, wp.Target));
        yield break;
    }
    public override event Action onAttack;
    public override void OnAttack() {
        onAttack?.Invoke();
    }

    public override void PostAwake() {

    }

    public override void PostStart() {
        
    }
}
    

   
        
    

