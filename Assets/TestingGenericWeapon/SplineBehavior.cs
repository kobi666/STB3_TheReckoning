using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class SplineBehavior
{
    public SplineController SplineController;
    [HideInInspector]
    public ProjectileExitPoint ExitPoint;
    [HideInInspector]
    public ProjectileFinalPoint FinalPoint;
    private EffectableTargetBank TargetBank;

    public AnimationClip OnHitSingleAnimation;
    public AnimationClip OnHitContinuousAnimation;

    [TypeFilter("GetSplineEffects")] [OdinSerialize][GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    public List<SplineEffect> SplineEffect;
    [TypeFilter("GetSplineMovements")] [OdinSerialize][GUIColor(0, 1, 0)][BoxGroup]
    public SplineMovementFunction SplineMovement;

    public void OnAttackEnd()
    {
        foreach (var effect in SplineEffect)
        {
            effect.OnAttackEnd();
        }
        SplineMovement.OnAttackEnd();
    }

    public void OnAttackStart()
    {
        foreach (var effect in SplineEffect)
        {
            effect.OnAttackStart();
        }
        SplineMovement.OnAttackStart();
    }

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

    private bool positionReached = false;
    void SetOnPositionReachedTrue()
    {
        positionReached = true;
    }

    private event Action<Effectable> onPosReachedAttack;

    void OnPosReachedAttack(Effectable ef)
    {
        if (positionReached)
        {
            onPosReachedAttack?.Invoke(ef);
        }
    }
    private event Action<Effectable> onConcurrentAttack; 
    
    

    public void Init()
    {
        FinalPoint = SplineController.FinalPoint;
        ExitPoint = SplineController.ExitPoint;
        TargetBank = SplineController.TargetBank;
        SplineMovement.Initialize(this);
        SplineMovement.onTargetPositionReached += SetOnPositionReachedTrue;
        foreach (var effect in SplineEffect)
        {
            effect.InitEffect(SplineController);
            if (effect.EffectStartsOnTargetPositionReached)
            {
                onPosReachedAttack += effect.OnAttack;
            }
            else
            {
                onConcurrentAttack += effect.OnAttack;
            }
        }
    }
    
    [ShowInInspector]
    private bool SplineBehaviorInProgress = false;
    /*public async void InitiateAsyncSplineBehavior()
    {
        if (SplineBehaviorInProgress == false)
        {
            SplineBehaviorInProgress = true;
            resetTimer();
            while (SplineDynamicData.MainTarget != null)
            {
                if (splineBehaviorTimer > 0)
                {
                    
                }
            }
        }
    }*/

    public void ConcurrentSplineBehavior(Effectable targetEffectable, Vector2 targetPosition)
    {
        SplineMovement.MoveSpline(targetEffectable.transform.position);
        OnPosReachedAttack(targetEffectable);
        onConcurrentAttack?.Invoke(targetEffectable);
    }
}
