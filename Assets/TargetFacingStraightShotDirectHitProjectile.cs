using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetFacingStraightShotDirectHitProjectile : DirectHitProjectile
{
    public override void MovementFunction() {
        if (targetPositionSet == true) {
            ProjectileUtils.MoveStraightUntilReachedTargetPosition(transform,TargetPosition,speed,OnTargetPositionReached);
        }
    }

    public override void AdditionalOnDisableActions() {
        //TargetBank = null;
    }

    public override void PostAwake() {
        onTargetPositionReached += delegate {gameObject.SetActive(false);};
    }

    public override void PostStart() {
      onHit += delegate(Effectable ef) { ef.ApplyDamage(Damage);};
    }

    private void Update() {
        MovementFunction();
    }
    
}
