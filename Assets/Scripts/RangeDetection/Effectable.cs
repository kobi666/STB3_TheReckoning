using UnityEngine;
using System;
using Sirenix.OdinInspector;

public abstract class Effectable : MonoBehaviour,IActiveObject<Effectable>,ITargetable,IhasGameObjectID
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

    public void OnTargetStateChange(bool state)
    {
        onTargetableStateChange?.Invoke(state);
    }

    void UpdateTargetableState(bool targetableState)
    {
        if (targetableState == true)
        {
            GameObjectPool.Instance.AddTargetable(GameObjectID);
        }
        else
        {
            GameObjectPool.Instance.RemoveTargetable(GameObjectID);
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

    
    protected void Start()
    {
        ActivePool = GameObjectPool.Instance.ActiveEffectables;
        ActivePool.AddObjectToActiveObjectPool(this);
        onTargetableStateChange += UpdateTargetableState;
    }

    

    
    void OnDisable()
    {
        UpdateTargetableState(false);
        GameObjectPool.Instance?.RemoveObjectFromAllPools(GameObjectID,name);
    }

    public int GameObjectID { get => ParentMyGameObject.gameObjectID; }
    [Required]
    public MyGameObject ParentMyGameObject;
}
