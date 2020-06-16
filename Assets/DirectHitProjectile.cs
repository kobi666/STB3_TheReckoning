using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class DirectHitProjectile : Projectile
{
    public SortedList<string, EnemyUnitController> PossibleTargets {get => TargetBank.Targets;}

    public EnemyTargetBank TargetBank;
    
    public abstract void MovementFunction();

    public event Action<EnemyUnitController> onHit;
    public void OnHit(EnemyUnitController ec) {
        onHit?.Invoke(ec);
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
