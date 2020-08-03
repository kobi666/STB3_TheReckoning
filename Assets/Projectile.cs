using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Projectile : MonoBehaviour, IQueueable<Projectile>,IActiveObject<Projectile>
{
    public void OnEnqueue() {
        
    }
    ActiveObjectPool<Projectile> activePool;
    public ActiveObjectPool<Projectile> ActivePool { get => activePool; set { activePool = value;}}
    string TypeTag = "Projectile";
    public abstract void MovementFunction();
    public event Action<EnemyUnitController> onHit;
    public void OnHit(EnemyUnitController ec) {
        onHit?.Invoke(ec);
    }
    public EnemyTargetBank TargetBank {get;set;}
    PoolObjectQueue<Projectile> queuePool;
    public PoolObjectQueue<Projectile> QueuePool {get => queuePool;set{queuePool = value;}}
    // Start is called before the first frame update
    UnitController targetUnit;
    public UnitController TargetUnit { get => targetUnit ; set { targetUnit = value;}}

    Vector2 targetPosition = Vector2.zero;
    public Vector2 TargetPosition { get => targetPosition; set {
        targetPosition = value;
        targetPositionSet = true;
        }}
    public bool targetPositionSet = false;

    public float speed = 5;
    public int Damage;

    public abstract void AdditionalOnDisableActions();


    void OnEnable()
    {
        ActivePool.AddObjectToActiveObjectPool(this);
    }
    void Awake()
    {
        gameObject.tag = TypeTag;    
    }

    private void OnDisable() {
        TargetPosition = transform.position;
        targetPositionSet = false;
        TargetUnit = null;
        QueuePool.ObjectQueue.Enqueue(this);
        TargetBank = null;
        AdditionalOnDisableActions();
        GameObjectPool.Instance.RemoveObjectFromAllPools(name);
    }

    

}
