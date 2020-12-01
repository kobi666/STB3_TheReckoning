using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using System.Linq;
using Sirenix.Serialization;

public class SplineEffect
{
    public bool MainTargetEffect = true;
    [HideLabel, ShowIf("MainTargetEffect"), TypeFilter("GetFilteredTypeList")] [OdinSerialize]
    public List<Effect> MainTargetEffects = new List<Effect>()
    {
        new Damage()
    };
    
    private static IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(Effect).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(Effect).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }

    public bool EffectHappensOnce;
    [ShowIf("EffectHappensOnce")]
    public bool EffectStartsOnTargetPositionReached;
    public bool EffectHappensAtInterval;
    
    public bool AffectsTargetsOnPath;
    [ShowIf("AffectsTargetsOnPath")] public int MaxNumberOfTargets = 1;
    
    [HideLabel, ShowIf("EffectsTargetsInPath"), TypeFilter("GetFilteredTypeList")] [OdinSerialize]
    public List<Effect> OnPathTargetsEffects = new List<Effect>()
    {
        new Damage()
    };
    
    
    

}
