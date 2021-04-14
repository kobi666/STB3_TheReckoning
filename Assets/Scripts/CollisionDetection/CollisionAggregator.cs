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
        
        if (TargetsAndTargetCount.ContainsKey(targetCollisionID))
        {
            TargetsAndTargetCount[targetCollisionID]++;
        }
        else
        {
            TargetsAndTargetCount.Add(targetCollisionID,1);
            OnTargetEnter(targetCollisionID);
        }
    }

    public void OnChildDetectorRemove(int targetCollisionID, string callerName)
    {
        if (TargetsAndTargetCount.ContainsKey(targetCollisionID))
        {
            TargetsAndTargetCount[targetCollisionID]--;
            if (TargetsAndTargetCount[targetCollisionID] <= 0)
            {
                TargetsAndTargetCount.Remove(targetCollisionID);
                OnTargetExit(targetCollisionID,callerName);
            }
        }
    }
    
}
