using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDiscoveryPoint : MonoBehaviour,IQueueable<PathDiscoveryPoint>,IActiveObject<PathDiscoveryPoint>
{
    public float Proximity;
    public Type QueueableType { get; set; }
    
    

    public PoolObjectQueue<PathDiscoveryPoint> queuePool;
    public PoolObjectQueue<PathDiscoveryPoint> QueuePool 
        { 
            get => queuePool;
            set { queuePool = value; }
        }

    public void OnEnqueue()
    {
        
    }

    protected void OnDisable()
    {
        QueuePool?.ObjectQueue.Enqueue(this);
        GameObjectPool.Instance.RemoveObjectFromAllPools(name,name);
    }

    protected void OnEnable()
    {
        ActivePool.AddObjectToActiveObjectPool(this);
    }

    protected void Awake()
    {
        ActivePool = GameObjectPool.Instance.ActivePathDiscoveryPoints;
    }


    public ActiveObjectPool<PathDiscoveryPoint> activePool;
    public ActiveObjectPool<PathDiscoveryPoint> ActivePool { get => activePool;
        set { activePool = value; }
    }
}
