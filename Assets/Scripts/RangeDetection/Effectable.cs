using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public abstract class Effectable : MonoBehaviour,IActiveObject<Effectable>,ITargetable
{
    bool OnFirstStart = true;
    public bool canOnlyBeHitOnce;
    public bool CanOnlyBeHitOnce {get => canOnlyBeHitOnce;set {canOnlyBeHitOnce = value;}}
    ActiveObjectPool<Effectable> activePool;
    public ActiveObjectPool<Effectable> ActivePool {
        get => activePool;
        set { activePool = value;}
    }

    bool externalTargetableLock;
    public bool ExternalTargetableLock {get => externalTargetableLock; set
    {
        externalTargetableLock = value; IsTargetable();}}

    public abstract bool IsTargetable();
    public event Action<bool> onTargetableStateChange;

    void UpdateTargetableState(bool targetableState)
    {
        if (targetableState == true)
        {
            GameObjectPool.Instance.AddTargetable(name);
        }
        else
        {
            GameObjectPool.Instance.RemoveTargetable(name);
        }
    }

    public abstract void ApplyDamage(int damageAmount);
    public abstract void ApplyExplosion(float explosionValue);
    public abstract void ApplyPoision(int poisionAmount, float poisionDuration);
    public abstract void ApplyFreeze(float FreezeAmount, float TotalFreezeProbability);

    void OnEnable()
    {
        if (OnFirstStart == false) {
        ActivePool.AddObjectToActiveObjectPool(this);
        }
    }

    
    void Start()
    {
        ActivePool = GameObjectPool.Instance.ActiveEffectables;
        ActivePool.AddObjectToActiveObjectPool(this);
        onTargetableStateChange += UpdateTargetableState;
        PostStart();
    }

    public abstract void PostStart();

    
    void OnDisable()
    {
        UpdateTargetableState(false);
        GameObjectPool.Instance.RemoveObjectFromAllPools(name,name);
    }
}
