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
    public SplineDynamicData SplineDynamicData = new SplineDynamicData();
    public SplineController SplineController;
    [HideInInspector]
    public ProjectileExitPoint ExitPoint;
    [HideInInspector]
    public ProjectileFinalPoint FinalPoint;
    private EffectableTargetBank TargetBank;
    
    public bool SpecificTarget;
    public bool SplineEndInstantlyReachesTarget;
    [HideIf("SplineInstantlyReachesTarget")]
    public bool SplineTravelsToTarget;
    [ShowIf("SplineTravelsToTarget")]
    public float SplineTravelSpeed = 0.1f;
    
    [HideIf("SpecificTarget")]
    public bool HitsTargetsAlongSpline;
    [ShowIf("HitsTargetsAlongBeam")] public int MaxTargets;
    [ShowIf("HitsTargetsAlongBeam")] public int TargetCounter = 0;
    
    [TypeFilter("GetSplineEffects")] [OdinSerialize]
    public List<SplineEffect> SplineEffect;
    [TypeFilter("GetSplineMovements")] [OdinSerialize]
    public SplineMovementFunction SplineMovement;

    public float SplineDuration;

    [ShowInInspector]
    float splineBehaviorTimer;

    public void resetTimer()
    {
        splineBehaviorTimer = SplineDuration;
    }

    public void resetTimer(float nonDefaultDuration)
    {
        splineBehaviorTimer = nonDefaultDuration;
    }
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
    
    

    public void Init(SplineController sc)
    {
        SplineController = sc;
        FinalPoint = sc.FinalPoint;
        ExitPoint = sc.ExitPoint;
        TargetBank = sc.TargetBank;
    }
    
    [ShowInInspector]
    private bool SplineAttackInProgress = false;
    public async void InitiateSplineAttack()
    {
        if (SplineAttackInProgress == false)
        {
            SplineAttackInProgress = true;
            resetTimer();
            while (SplineDynamicData.MainTarget != null)
            {
                if (splineBehaviorTimer > 0)
                {
                    
                }
            }
        }
    }
}
