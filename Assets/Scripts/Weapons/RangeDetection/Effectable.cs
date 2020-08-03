using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Effectable : MonoBehaviour,IActiveObject<Effectable>
{
    ActiveObjectPool<Effectable> activePool;
    public ActiveObjectPool<Effectable> ActivePool {
        get => activePool;
        set { activePool = value;}
    }

    public abstract void OnDamage(int damageAmount);
    public abstract void OnExplosion(float explosionValue);
    public abstract void OnPoision(int poisionAmount, float poisionDuration);
    public abstract void OnFreeze(float FreezeAmount, float TotalFreezeProbability);

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
