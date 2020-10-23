using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using System.Linq;

[System.Serializable]
[BoxGroup]
[HideLabel]
public class ProjectileBehaviorData
{
    //public ProjectileMovementFunction MovementFunctions = new ProjectileMovementFunction();
    public bool OnHitEffect;
    [ShowInInspector,HideLabel,ShowIf("EffectOnDirectHit"),TypeFilter("GetFilteredTypeList")]
    public Effect onHitEffect = new Damage();
    
    
    public bool TriggersOnCollision;
    
    public bool AOE;

    [ShowInInspector,ShowIf("AOE")]
    public float EffectRadius
    {
        get;
        set;
    } 
    
    public bool OnPositionReachedEffect;
    [ShowInInspector,HideLabel, ShowIf("OnPositionReachedEffect")] [TypeFilter("GetFilteredTypeList")]
    public Effect onPositionReachedEffect = new Damage();
    
    public bool Homing;
    
    
    private static IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(Effect).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(Effect).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
}
