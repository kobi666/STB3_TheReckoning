using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Linq;

[System.Serializable]
[BoxGroup]
public class ProjectileEffect 
{
    public float ProjectileLifeTime = 3f;    //public ProjectileMovementFunction MovementFunctions = new ProjectileMovementFunction();
    public bool OnHitEffect;

    [HideLabel, ShowIf("OnHitEffect"), TypeFilter("GetFilteredTypeList")][SerializeReference]
    public List<Effect> onHitEffects = new List<Effect>()
    {
        new Damage()
    };
    
    
    public bool TriggersOnCollision;
    [ShowIf("TriggersOnCollision")]
    public int HitCounter = 1;

    
    [ShowIf("TriggersOnCollision")]
    public bool TriggersOnSpecificTarget = false;
    
    
    public bool OnPositionReachedEffect;

    [HideLabel, ShowIf("OnPositionReachedEffect")] [TypeFilter("GetFilteredTypeList")][SerializeReference]
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
            .Where(x => x.IsSubclassOf(typeof(Effect)));
        
        return q;
    }
}
