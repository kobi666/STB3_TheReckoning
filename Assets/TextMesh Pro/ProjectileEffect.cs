using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using System.Linq;
using Sirenix.Serialization;

[System.Serializable]
[BoxGroup]
public class ProjectileEffect
{
    public float ProjectileLifeTime = 3f;    //public ProjectileMovementFunction MovementFunctions = new ProjectileMovementFunction();
    public bool OnHitEffect;

    [HideLabel, ShowIf("OnHitEffect"), TypeFilter("GetFilteredTypeList")] [OdinSerialize]
    public List<Effect> onHitEffects = new List<Effect>()
    {
        new Damage()
    };
    
    
    public bool TriggersOnCollision;

    [ShowInInspector, ShowIf("TriggersOnCollision")]
    public int HitCounter { get; set; } = 1;
    
    
    
    public bool OnPositionReachedEffect;

    [HideLabel, ShowIf("OnPositionReachedEffect")] [TypeFilter("GetFilteredTypeList")] [OdinSerialize]
    public List<Effect> onPositionReachedEffects = new List<Effect>()
    {
        new Damage()
    };

    
    
    
    
    
    //public Effect onPositionReachedEffect = new Damage();
    
    public bool Homing;
    
    public bool AOE;

    [ShowInInspector, ShowIf("AOE")]
    public float EffectRadius { get; set; } = 0.2f;
    private static IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(Effect).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(Effect).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
}
