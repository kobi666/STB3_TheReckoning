using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class OnTargetReachedProjectile : Projectile
{
    public event Action<IEnumerator, ProjectileData> onUpdateMovementCoroutine;
    public void OnUpdateMovementCoroutine(IEnumerator newMovementCoroutine, ProjectileData projectileData) {
        onUpdateMovementCoroutine?.Invoke(newMovementCoroutine, projectileData);
    }
    public IEnumerator MovementCoroutine = null;

    public abstract void OnTargetReachedAction();

    public event Action onTargetPositionReached;
    public void OnTargetPositionReached() {
        onTargetPositionReached?.Invoke();
    }

    protected void Awake() {
        base.Awake();
        onTargetPositionReached += OnTargetReachedAction;
        onTargetPositionReached += delegate {PlayOnHitAnimation(null);};
        onTargetPositionReached += delegate {gameObject.SetActive(false);};
    }


    protected void Update()
    {
        MovementFunction();
    }


    protected void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    

     

    

    
}
