using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class OnTargetReachedProjectile : Projectile
{
    public IEnumerator movementCoroutine;

    public abstract void OnTargetReachedAction();

    public abstract IEnumerator MovementCoroutine {get;set;}
    
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

    
    
    protected void Start()
    {
        StartCoroutine(movementCoroutine);   
    }

     

    

    
}
