using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DirectHitProjectile : Projectile
{
    public SortedList<string, EnemyUnitController> PossibleTargets {get => TargetBank.Targets;}

    public EnemyTargetBank TargetBank;
    
    public override void MovementFunction() {
        
    }

    public event Action onTargetPositionReached;
    public void OnTargetPositionReached() {
        onTargetPositionReached?.Invoke();
    }

    public override void OnDisableActions() {
        TargetBank = null;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (PossibleTargets.ContainsKey(other.name)) {
            if (PossibleTargets[other.name].IsTargetable())
            OnHit(PossibleTargets[other.name]);
        }
    }

    private void Update() {
        MovementFunction();
    }
    
}
