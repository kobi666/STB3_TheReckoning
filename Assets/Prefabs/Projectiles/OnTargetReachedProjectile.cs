using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class OnTargetReachedProjectile : Projectile
{
    public bool TargetReachedCalled = false;
    public void RunMovementFunctionUntilTargetReachedAndInvokeActionInTheEnd() {
        if (targetPositionSet == true) {
            if ((Vector2)transform.position != TargetPosition) {
                MovementFunction();
            }
            else if ((Vector2)transform.position == TargetPosition) {
                if (TargetReachedCalled == false) {
                    OnTargetPositionReached();
                    TargetReachedCalled = true;
                }
            }
        }
    }
    public abstract void OnTargetReachedAction();
    
    public event Action onTargetPositionReached;
    public void OnTargetPositionReached() {
        onTargetPositionReached?.Invoke();
    }

    public override void PostAwake() {
        onTargetPositionReached += delegate {PlayOnHitAnimation(null);};
    }

    void Update()
    {
        RunMovementFunctionUntilTargetReachedAndInvokeActionInTheEnd(); 
    }

    

    
}
