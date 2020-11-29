using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class SplineBehavior
{
    [ShowInInspector]
    private SplineDynamicData SplineDynamicData = new SplineDynamicData();
    public SplineController SplineController;
    public ProjectileExitPoint ExitPoint;
    public ProjectileFinalPoint FinalPoint;
    
    public bool SingleTarget;
    public bool SplineEndInstantlyReachesTarget;
    [HideIf("SplineInstantlyReachesTarget")]
    public bool SplineTravelsToTarget;
    [ShowIf("SplineTravelsToTarget")]
    public float SplineTravelSpeed = 0.1f;
    
    [HideIf("SingleTarget")]
    public bool HitsTargetsAlongSpline;
    [ShowIf("HitsTargetsAlongBeam")] public int MaxTargets;
    [ShowIf("HitsTargetsAlongBeam")] public int TargetCounter = 0;
    
    [TypeFilter("GetSplineEffects")] [OdinSerialize]
    public List<SplineEffect> SplineEffect;
    [TypeFilter("GetSplineMovements")] [OdinSerialize]
    public SplineMovementFunction SplineMovement;

    public float SplineDuration;
    private static IEnumerable<Type> GetSplineEffects()
    {
        var q = typeof(SplineEffect).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(SplineEffect).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
    
    private static IEnumerable<Type> GetSplineMovements()
    {
        var q = typeof(SplineMovementFunction).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(SplineMovementFunction).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
}
