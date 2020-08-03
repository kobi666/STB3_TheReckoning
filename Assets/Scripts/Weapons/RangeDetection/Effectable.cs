using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Effectable : MonoBehaviour,IActiveObject<Effectable>,ITargetable
{
    public bool canOnlyBeHitOnce;
    public bool CanOnlyBeHitOnce {get => canOnlyBeHitOnce;set {canOnlyBeHitOnce = value;}}
    ActiveObjectPool<Effectable> activePool;
    public ActiveObjectPool<Effectable> ActivePool {
        get => activePool;
        set { activePool = value;}
    }

    bool externalTargetableLock;
    public bool ExternalTargetableLock {get => externalTargetableLock; set {externalTargetableLock = value;}}

    public abstract bool IsTargetable();

    public abstract void ApplyDamage(int damageAmount);
    public abstract void ApplyExplosion(float explosionValue);
    public abstract void ApplyPoision(int poisionAmount, float poisionDuration);
    public abstract void ApplyFreeze(float FreezeAmount, float TotalFreezeProbability);

    void Awake()
    {
        
    }

    void OnEnable()
    {
        ActivePool.AddObjectToActiveObjectPool(this);
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        ActivePool.RemoveObjectFromPool(name);
    }
}
