using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFacingStraightShotDirectHitProjectile : DirectHitProjectile
{
    // Start is called before the first frame update
   public override void MovementFunction() {
       ProjectileUtils.MoveUntilReachedTargetPosition(transform, TargetPosition,speed,OnTargetPositionReached);
   }

   private void Awake() {
       onTargetPositionReached += delegate {Debug.LogWarning("Reached Position");};
       onTargetPositionReached += delegate {gameObject.SetActive(false);};
   }
}
