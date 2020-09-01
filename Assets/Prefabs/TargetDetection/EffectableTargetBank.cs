﻿using System.Collections;
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
    public override void PostStart() {
        
    }

}
