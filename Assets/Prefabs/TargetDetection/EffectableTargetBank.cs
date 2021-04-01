using System;
using System.Linq;
using UnityEngine;

public class EffectableTargetBank : TargetBank<Effectable>
{

    public override Effectable TryToGetTargetOfType(int gameObjectID)
    {
        Effectable ef = null;
        if (GameObjectPool.Instance.ActiveEffectables.Contains(gameObjectID)) {
        ef = GameObjectPool.Instance.ActiveEffectables.Pool[gameObjectID] ?? null;
        }
        return ef;
    }

    public int TryToGetTargetClosestToPosition(Vector2 pos, int[] existingTargets)
    {
        int s = 0;
        float distance = 9999f;
        foreach ((Effectable ef,bool isTargetable) in Targets.Values)
        {
            if (ef != null)
            {
                if (existingTargets.Contains(ef.GameObjectID))
                {
                    continue;
                }   
                float vd = Vector2.Distance(pos, ef.transform.position);
                if (vd < distance)
                {
                    distance = vd;
                    s = ef.GameObjectID;
                }
            }
        }
        return s;
    }
    
    public string TryToGetTargetClosestToPosition(Vector2 pos, string[] existingTargets, float minimumDistance)
    {
        string s = String.Empty;
        float distance = 9999f;
        foreach ((Effectable ef,bool targetable) in Targets.Values)
        {
            if (ef != null)
            {
                if (existingTargets.Contains(ef.name))
                {
                    continue;
                }
                float vd = Vector2.Distance(pos, ef.transform.position);
                if (vd < minimumDistance)
                {
                    continue;
                }
                if (vd < distance)
                {
                    distance = vd;
                    s = ef.name;
                }
            }
        }
        return s;
    }

    

    public override void PostStart()
    {
        
    }
}
