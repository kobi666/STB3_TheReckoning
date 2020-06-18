using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetFacingStraightShotDirectHitProjectile : DirectHitProjectile
{
    
    public override void MovementFunction() {
        if (targetPositionSet == true) {
            ProjectileUtils.MoveUntilReachedTargetPosition(transform,TargetPosition,speed,OnTargetPositionReached);
        }
    }

    public override void AdditionalOnDisableActions() {
        TargetBank = null;
    }

    private void Awake() {
        
        onTargetPositionReached += delegate {gameObject.SetActive(false);};
    }

    public override void PostStart() {
      onHit += delegate(EnemyUnitController ec) { ec.LifeManager.DamageToUnit(Damage);};
      Debug.LogWarning(Damage);  
    }

    private void Update() {
        MovementFunction();
    }
    
}
