using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDiscoveryPoint : MonoBehaviour,IQueueable<PathDiscoveryPoint>,IActiveObject<PathDiscoveryPoint>
{
    
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


    public ActiveObjectPool<PathDiscoveryPoint> activePool;
    public ActiveObjectPool<PathDiscoveryPoint> ActivePool { get => activePool;
        set { activePool = value; }
    }
}
