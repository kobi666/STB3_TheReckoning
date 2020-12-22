using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class SplineBehavior
{
    [Required]
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

    public event Action onBehaviorEnd;
    public void OnBehaviorEnd()
    {
        onBehaviorEnd?.Invoke();
    }

    public event Action<Effectable,Vector2> onBehaviorStart;
    public void OnBehaviorStart(Effectable initialEfffectable,Vector2 initialtargetPos)
    {
        onBehaviorStart?.Invoke(initialEfffectable,initialtargetPos);
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

    private event Action<Effectable,Vector2> onPosReachedAttack;

    void OnPosReachedAttack(Effectable ef,Vector2 targetPos)
    {
        if (positionReached)
        {
            onPosReachedAttack?.Invoke(ef,targetPos);
        }
    }
    private event Action<Effectable,Vector2> onConcurrentAttack; 
    
    

    public void Init()
    {
        FinalPoint = SplineController.FinalPoint;
        ExitPoint = SplineController.ExitPoint;
        TargetBank = SplineController.TargetBank;
        SplineMovement.Initialize(this);
        SplineMovement.onTargetPositionReached += SetOnPositionReachedTrue;
        onBehaviorStart += SplineMovement.OnMovementStart;
        onBehaviorEnd += SplineMovement.OnMovementEnd;
        foreach (var effect in SplineEffect)
        {
            effect.InitEffect(SplineController);
            onBehaviorStart += effect.OnEffectStart;
            onBehaviorEnd += effect.OnEffectEnd;
            if (effect.EffectStartsOnTargetPositionReached)
            {
                onPosReachedAttack += effect.OnEffectTrigger;
            }
            else
            {
                onConcurrentAttack += effect.OnEffectTrigger;
            }
        }
    }
    
    [ShowInInspector]
    private bool SplineBehaviorInProgress = false;
    

    public void InvokeSplineBehavior(Effectable targetEffectable, Vector2 targetPosition)
    {
        SplineMovement.MoveSpline(targetEffectable.transform.position);
        OnPosReachedAttack(targetEffectable,targetPosition);
        onConcurrentAttack?.Invoke(targetEffectable,targetPosition);
    }
}
