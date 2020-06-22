using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DirectHitProjectile : Projectile
{

    public SortedList<string, EnemyUnitController> PossibleTargets {get => TargetBank.Targets ?? null;}

    public int HitCounter = 1;
    public event Action onHitCounterZero;
    public void OnHitCounterZero() {
        onHitCounterZero?.Invoke();
    }
    
    public override void MovementFunction() {

    }

    public event Action onTargetPositionReached;
    public void OnTargetPositionReached() {
        onTargetPositionReached?.Invoke();
    }

    public override void AdditionalOnDisableActions() {
        TargetBank = null;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (PossibleTargets.ContainsKey(other.name)) {
            if (PossibleTargets[other.name].IsTargetable())
            OnHit(PossibleTargets[other.name]);
            HitCounter -= 1;
            if (HitCounter == 0) {
                OnHitCounterZero();
            }
        }
    }
    public virtual void PostStart() {

    }
    private void Start() {
        onHitCounterZero += delegate {gameObject.SetActive(false);};
        PostStart();
    }

    private void Update() {
        MovementFunction();
    }

    private void OnEnable() {
        HitCounter = 1;
    }
    
}
