using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Projectile : MonoBehaviour, IQueueable<Projectile>
{
    public abstract void MovementFunction();
    public event Action<EnemyUnitController> onHit;
    public void OnHit(EnemyUnitController ec) {
        onHit?.Invoke(ec);
    }
    PoolObjectQueue<Projectile> pool;
    public PoolObjectQueue<Projectile> Pool {get => pool;set{pool = value;}}
    // Start is called before the first frame update
    UnitController targetUnit;
    public UnitController TargetUnit { get => targetUnit ; set { targetUnit = value;}}

    Vector2 targetPosition;
    public Vector2 TargetPosition { get => targetPosition; set {targetPosition = value;}}

    public float speed = 5;

    public abstract void OnDisableActions();

    private void OnDisable() {
        TargetPosition = transform.position;
        TargetUnit = null;
        Pool.ObjectQueue.Enqueue(this);
        OnDisableActions();
    }

}
