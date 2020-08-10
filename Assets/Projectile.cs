using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Projectile : MonoBehaviour, IQueueable<Projectile>,IActiveObject<Projectile>
{
    public Type QueueableType {get;set;}
    public ProjectileData Data = new ProjectileData();
    public void OnEnqueue() {
        
    }

    public SingleAnimationObject OnHitAnimation;

    PoolObjectQueue<SingleAnimationObject> onHitAnimationQueuePool;

    public Dictionary<string,Effectable> ActiveTargets => GameObjectPool.Instance.ActiveEffectables.Pool;
    ActiveObjectPool<Projectile> activePool;
    public ActiveObjectPool<Projectile> ActivePool { get => activePool; set { activePool = value;}}
    string TypeTag = "Projectile";
    public abstract void MovementFunction();
    public event Action<Effectable> onHit;
    public void OnHit(Effectable ef) {
        onHit?.Invoke(ef);
    }
    //public EffectableTargetBank TargetBank {get;set;}
    PoolObjectQueue<Projectile> queuePool;
    public PoolObjectQueue<Projectile> QueuePool {get => queuePool;set{queuePool = value;}}
    // Start is called before the first frame update
    public Effectable EffectableTarget { get => Data.EffectableTarget ; set { Data.EffectableTarget = value;}}

    public Vector2 TargetPosition { get => Data.TargetPosition; set {
        Data.TargetPosition = value;
        targetPositionSet = true;
        }}
    public bool targetPositionSet = false;
    
    public float speed {get => Data.Speed ; set {Data.Speed = value;}}
    public int Damage { get => Data.Damage ; set {Data.Damage = value;}}

    public abstract void AdditionalOnDisableActions();

    public void PlayOnHitAnimation(Effectable ef) {
        SingleAnimationObject sao = onHitAnimationQueuePool.Get();
        sao.transform.position = transform.position;
        sao.gameObject.SetActive(true);
        sao.PlayOnceAndDisable();
    }


    void OnEnable()
    {
        ActivePool?.AddObjectToActiveObjectPool(this);
    }

    public abstract void PostAwake();
    protected void  Awake()
    {
        if (OnHitAnimation != null) {
        onHitAnimationQueuePool = GameObjectPool.Instance.GetSingleAnimationObjectQueue(OnHitAnimation);
        }
        gameObject.tag = TypeTag; 
        onHit += PlayOnHitAnimation;
        PostAwake();
    }

    protected void OnDisable() {
        TargetPosition = transform.position;
        targetPositionSet = false;
        EffectableTarget = null;
        QueuePool?.ObjectQueue.Enqueue(this);
        GameObjectPool.Instance.RemoveObjectFromAllPools(name);
        AdditionalOnDisableActions();
    }

    

}
