using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectableTargetBank : TargetBank<Effectable>
{

    public override Effectable TryToGetTargetOfType(GameObject go)
    {
        Effectable ef = null;
        if (GameObjectPool.Instance.ActiveEffectables.Contains(go.name)) {
        ef = GameObjectPool.Instance.ActiveEffectables.Pool[go.name] ?? null;
        }
        return ef;
    }

    public string TryToGetTargetClosestToPosition(Vector2 pos)
    {
        string s = String.Empty;
        float distance = 9999f;
        foreach (Effectable ef in Targets.Values)
        {
            if (ef != null)
            {
                float vd = Vector2.Distance(pos, ef.transform.position);
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
