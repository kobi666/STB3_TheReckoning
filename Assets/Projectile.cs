﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Projectile : MonoBehaviour, IQueueable<Projectile>,IActiveObject<Projectile>
{
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
    Effectable targetUnit;
    public Effectable TargetUnit { get => targetUnit ; set { targetUnit = value;}}

    Vector2 targetPosition = Vector2.zero;
    public Vector2 TargetPosition { get => targetPosition; set {
        targetPosition = value;
        targetPositionSet = true;
        }}
    public bool targetPositionSet = false;

    public float speed = 5;
    public int Damage;

    public abstract void AdditionalOnDisableActions();

    public void PlayOnHitAnimation(Effectable ef) {
        SingleAnimationObject sao = onHitAnimationQueuePool.Get();
        sao.transform.position = transform.position;
        sao.gameObject.SetActive(true);
        sao.PlayOnceAndEnqueue();
    }


    void OnEnable()
    {
        ActivePool.AddObjectToActiveObjectPool(this);
    }

    public abstract void PostAwake();
    void Awake()
    {
        if (OnHitAnimation != null) {
        onHitAnimationQueuePool = GameObjectPool.Instance.GetSingleAnimationObjectQueue(OnHitAnimation);
        }
        gameObject.tag = TypeTag; 
        onHit += PlayOnHitAnimation;
        PostAwake();
    }

    private void OnDisable() {
        TargetPosition = transform.position;
        targetPositionSet = false;
        TargetUnit = null;
        //TargetBank = null;
        QueuePool.ObjectQueue.Enqueue(this);
        GameObjectPool.Instance.RemoveObjectFromAllPools(name);
        AdditionalOnDisableActions();
    }

    

}
