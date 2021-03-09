using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PlayerUnitPoolCreationData
{
    [SerializeField]
    public PlayerUnitController UnitPrefabBase;
    [SerializeField]
    public UnitDataLegacy unitDataLegacy = new UnitDataLegacy();
    
    [TypeFilter("GetEffects")][SerializeReference]
    public List<Effect> AttackEffects = new List<Effect>();

    private static IEnumerable<Type> GetEffects()
    {
        return Effect.GetEffects();
    }
}
