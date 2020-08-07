using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Effector))]
public abstract class DirectHitProjectile : Projectile
{
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
        //TargetBank = null;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (GameObjectPool.Instance.ActiveEffectables?.Pool.ContainsKey(other.name) ?? false) {
            if (ActiveTargets[other.name].IsTargetable())
            OnHit(ActiveTargets[other.name]);
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
