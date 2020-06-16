using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IQueueable<Projectile>
{

    PoolObjectQueue<Projectile> pool;
    public PoolObjectQueue<Projectile> Pool {get => pool;set{pool = value;}}
    // Start is called before the first frame update
    UnitController targetUnit;
    public UnitController TargetUnit { get => targetUnit ; set { targetUnit = value;}}

    Vector2 targetPosition;
    public Vector2 TargetPosition { get => targetPosition; set {targetPosition = value;}}

    public abstract void OnDisableActions();

    private void OnDisable() {
        TargetPosition = transform.position;
        TargetUnit = null;
        Pool.ObjectQueue.Enqueue(this);
        OnDisableActions();
    }

}
