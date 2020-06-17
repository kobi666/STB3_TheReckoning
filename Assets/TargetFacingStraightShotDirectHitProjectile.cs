using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetFacingStraightShotDirectHitProjectile : Projectile
{
    public SortedList<string, EnemyUnitController> PossibleTargets {get => TargetBank.Targets;}
    
    public override void MovementFunction() {
        if (targetPositionSet == true) {
            ProjectileUtils.MoveUntilReachedTargetPosition(transform,TargetPosition,speed,OnTargetPositionReached);
        }
    }

    public event Action onTargetPositionReached;
    public void OnTargetPositionReached() {
        onTargetPositionReached?.Invoke();
    }

    public override void AdditionalOnDisableActions() {
        TargetBank = null;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (PossibleTargets.ContainsKey(other.name)) {
            if (PossibleTargets[other.name].IsTargetable())
            OnHit(PossibleTargets[other.name]);
        }
    }

    private void Awake() {
        onTargetPositionReached += delegate {gameObject.SetActive(false);};
    }

    private void Update() {
        MovementFunction();
    }
    
}
