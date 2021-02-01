using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[System.Serializable]
public class SplineBehavior : IhasExitAndFinalPoint,IHasEffectAnimation
{
    [Required]
    public SplineController SplineController;
    [HideInInspector]
    public ProjectileExitPoint ExitPoint;
    [HideInInspector]
    public ProjectileFinalPoint FinalPoint;
    private EffectableTargetBank TargetBank;

    [HideInInspector] public bool parentIsRotatingTowardsTarget = false;
    
    [ShowInInspector]
    public int testInt { get; set; }

    private Vector2 initialExitPointPos;
    private Vector2 initialFinalPointpos;

    [TypeFilter("GetSplineEffects")] [GUIColor(0.3f, 0.8f, 0.8f, 1f)][SerializeReference]
    public List<SplineEffect> SplineEffect = new List<SplineEffect>();
    [TypeFilter("GetSplineMovements")] [GUIColor(0, 1, 0)][BoxGroup][SerializeReference]
    public SplineMovementFunction SplineMovement = new StraightLaZor();

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
    
    public virtual void StopRenderingSpline()
    {
        SplineController.LineRenderer.enabled = false;
    }

    public virtual void StartRenderingSpline(Effectable ef, Vector2 pos)
    {
        SplineController.LineRenderer.enabled = true;
    }

    private void ResetSpline()
    {
        int splineLength = SplineController.BgCurve.Points.Length;
        for (int i = 0; i <= splineLength -1; i++)
        {
            
            if (SplineController.BgCurve.Points.Length > 2) {
                SplineController.BgCurve.Delete(SplineController.BgCurve.Points.Length -1);
            }
        }
        SplineController.BgCurve.Points[0].PositionLocal = ExitPoint.transform.position;
        SplineController.BgCurve.Points[1].PositionLocal = FinalPoint.transform.position;
    }

    public void Init()
    {
        FinalPoint = SplineController.FinalPoint;
        ExitPoint = SplineController.ExitPoint;
        initialFinalPointpos = FinalPoint.transform.position;
        initialExitPointPos = ExitPoint.transform.position;
        TargetBank = SplineController.TargetBank;
        SplineMovement.Initialize(this);
        SplineMovement.onTargetPositionReached += SetOnPositionReachedTrue;
        onBehaviorStart += SplineMovement.OnMovementStart;
        onBehaviorEnd += SplineMovement.OnMovementEnd;
        onBehaviorStart += delegate(Effectable effectable, Vector2 vector2) { SetInitialFinalPointPosition(); };
        onBehaviorStart += StartRenderingSpline;
        onBehaviorEnd += StopRenderingSpline;
        onBehaviorEnd += ResetSpline;
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

        InitEffectAnimation();
    }
    
    [ShowInInspector]
    private bool SplineBehaviorInProgress = false;
    

    public void InvokeSplineBehavior(Effectable targetEffectable, Vector2 targetPosition)
    {
        SplineMovement.MoveSpline(targetEffectable.transform.position);
        OnPosReachedAttack(targetEffectable,targetPosition);
        onConcurrentAttack?.Invoke(targetEffectable,targetPosition);
    }

    public List<ProjectileFinalPoint> GetFinalPoints()
    {
        List<ProjectileFinalPoint> lpfp = new List<ProjectileFinalPoint>();
        lpfp.Add(FinalPoint);
        return lpfp;
    }

    public void SetInitialFinalPointPosition()
    {
        {
            int index = 0;
            foreach (var fp in GetFinalPoints())
            {
                fp.transform.position = GetExitPoints()[index].transform.position;
                index += 1;
            }
        }
    }

    public List<ProjectileExitPoint> GetExitPoints()
    {
        List<ProjectileExitPoint> lpep = new List<ProjectileExitPoint>();
        lpep.Add(ExitPoint);
        return lpep;
    }

    public void SetInitialExitPointPosition()
    {
        
    }

    
    public AnimationClip SplineEndAnimation;
    public List<EffectAnimationController> EffectAnimationControllers { get; set; } = new List<EffectAnimationController>();
    public void InitEffectAnimation()
    {
        if (SplineEndAnimation != null)
        {
            EffectAnimationController eac = GameObjectPool.Instance.GetEffectAnimationQueue().Get();
            eac.AnimationClip = SplineEndAnimation;
            EffectAnimationControllers.Add(eac);
            SplineMovement.onFinalPointUpdate += EffectAnimationControllers[0].MoveToPosition;
            onBehaviorStart += delegate(Effectable effectable, Vector2 vector2) { eac.PlayLoopingAnimation();  };
            onBehaviorEnd += eac.StopEffectAnimation;
        }
    }
}
