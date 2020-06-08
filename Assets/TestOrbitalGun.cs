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

    public override event Action onAttack;
    public override void OnAttack() {
        onAttack?.Invoke();
    }

    public override void PostStart() {
        //onAttackInitiate += 
    }

    public override void PostAwake() {

    }

    private void Update() {
        
    }

}
