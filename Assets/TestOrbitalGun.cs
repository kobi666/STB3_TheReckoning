using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestOrbitalGun : OrbitalWeapon
{
    // Start is called before the first frame update
    IEnumerator attackCoroutine;
    public override IEnumerator AttackCoroutine {get => attackCoroutine; set {attackCoroutine = value;}}
    public override GameObject referenceGOforRotation {get;set;}
    public override bool ShouldRotate {get; set;}

    public override void InitiateAttackSequence() {
        if (CanAttack) {
        ReStartAttacking(this, WeaponUtils.TestAttack(this, Target));
        }
        StopOrbiting();
    }

    public override void CeaseAttackSequence() {
        StopAttacking();
        ReStartOrbiting();
    }

    public override event Action onAttack;
    public override void OnAttack() {
        onAttack?.Invoke();
    }

    public override void PostStart() {
        
    }

    public override void PostAwake() {

    }

    private void Update() {
        
    }

}
