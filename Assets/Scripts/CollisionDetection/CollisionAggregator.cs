using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CollisionAggregator : CollisionDetector
{
    protected void Awake()
    {
        RegisterToGWCS = false;
        base.Awake();
    }
    
    public List<CollisionDetector> ChildCollisionDetectors = new List<CollisionDetector>();
    [ShowInInspector]
    private Dictionary<int,int> TargetsAndTargetCount = new Dictionary<int, int>();

    public void OnChildDetectorAdd(int targetCollisionID)
    {
        int targetGID = GameObjectPool.CollisionIDToGameObjectID[targetCollisionID].Item1;
        if (TargetsAndTargetCount.ContainsKey(targetGID))
        {
            TargetsAndTargetCount[targetGID]++;
        }
        else
        {
            TargetsAndTargetCount.Add(targetGID,1);
        }
    }

    public void OnChildDetectorRemove(int targetCollisionID, string callerName)
    {
        int targetGID = GameObjectPool.CollisionIDToGameObjectID[targetCollisionID].Item1;
        if (TargetsAndTargetCount.ContainsKey(targetGID))
        {
            TargetsAndTargetCount[targetGID]--;
            if (TargetsAndTargetCount[targetGID] <= 0)
            {
                TargetsAndTargetCount.Remove(targetGID);
            }
        }
    }
    
}
