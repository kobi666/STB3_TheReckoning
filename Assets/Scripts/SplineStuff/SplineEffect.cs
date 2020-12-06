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

    private EffectableTargetBank TargetBank;
    private SplineController splineController;
    
    
    private static IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(Effect).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(Effect).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
    
    [DisableIf("EffectHappensAtInterval")]
    public bool EffectHappensOnce;
    [HideIf("EffectHappensOnce"), DisableIf("EffectHappensOnce")]
    public bool EffectHappensAtInterval;
    
    
    public bool EffectStartsOnTargetPositionReached = false;
    [ShowIf("EffectHappensAtInterval")] public float EffectInterval = 0.25f;

    [ShowIf("EffectHappensAtInterval"), ShowInInspector]
    private float EffectIntervalCounter = 0;
    public event Action onInterval;
    
    public bool AffectsTargetsOnPath;

    [HideLabel, ShowIf("AffectsTargetsOnPath")] [TypeFilter("GetFilteredTypeList")] [OdinSerialize]
    public List<Effect> OnPathTargetsEffects = new List<Effect>()
    {
        new Damage()
    };

    private event Action<Effectable> onPathEffect;
    private event Action<Effectable> onMainTargetEffect;
    private event Action<Effectable> onAttack;

    private void ApplyEffectOnMainTarget(Effectable ef)
    {
        if (TargetBank.Targets.ContainsKey(ef.name))
        {
            if (ef.IsTargetable())
            {
                onMainTargetEffect?.Invoke(ef);
            }
        }
    }

    private Effectable[] t_targets;

    private bool onPathEffectInProgress;
    private void ApplyEffectOnPathTargets(Effectable ef)
    {
        if (!onPathEffectInProgress)
        {
            onPathEffectInProgress = true;
            if (MainTargetEffect)
            {
                t_targets = TargetBank.Targets.Values.ToArray();
                foreach (var t in t_targets)
                {
                    if (t.name == ef.name)
                    {
                        continue;
                    }

                    if (t?.IsTargetable() ?? false)
                    {
                        onPathEffect?.Invoke(t);
                    }
                }
            }
            else
            {
                t_targets = TargetBank.Targets.Values.ToArray();
                foreach (var t in t_targets)
                    {
                        if (t?.IsTargetable() ?? false)
                        {
                            onPathEffect?.Invoke(t);
                        }
                    }
            }

            onPathEffectInProgress = false;
        }
    }

    private void EffectOnce(Effectable ef)
    {
        if (!m_SingleSingleEffectHappened)
        {
            OnAttack(ef);
        }
    }

    public void OnAttack(Effectable ef)
    {
        if (TargetBank.Targets.Any())
        {
            if (EffectHappensAtInterval)
            {
                EffectOnInterval(ef);
            }

            if (EffectHappensOnce)
            {
                EffectOnce(ef);
            }
        }
    }

    void EffectOnMainTarget(Effectable ef)
    {
        if (TargetBank.Targets.ContainsKey(ef.name))
        {
            if (ef.IsTargetable())
            {
                onPathEffect?.Invoke(ef);
            }
        }
    }

    public event Action onAttackStart;

    public void OnAttackStart()
    {
        onAttackStart?.Invoke();
    }
    public event Action onAttackEnd;

    public void OnAttackEnd()
    {
        onAttackEnd?.Invoke();
    }
    void EffectOnPathTargets(Effectable ef)
    {
        foreach (var target in TargetBank.Targets.Values)
        {
            if (target == null)
            {
                continue;
            }

            if (MainTargetEffect)
            {
                if (target.name == ef.name)
                {
                    continue;
                }
            }

            if (target.IsTargetable())
            {
                onPathEffect?.Invoke(target);
            }
        }
    }

    void EffectOnInterval(Effectable ef)
    {
        EffectIntervalCounter += StaticObjects.Instance.DeltaGameTime;
        if (EffectIntervalCounter >= EffectInterval)
        {
            onAttack?.Invoke(ef);
            EffectIntervalCounter = 0;
        }
    }

    public event Action<Effectable> onEffect;

    private void OnEffect(Effectable ef)
    {
        onEffect?.Invoke(ef);
    }
    
    private bool m_SingleSingleEffectHappened = false;

    public bool SingleEffectHappened
    {
        get => m_SingleSingleEffectHappened;
        set
        {
            m_SingleSingleEffectHappened = true;
        }
    }

    

    public void InitEffect(SplineController sc)
    {
        splineController = sc;
        TargetBank = sc.TargetBank;
        
        if (MainTargetEffect)
        {
            foreach (var effect in MainTargetEffects)
            {
                onMainTargetEffect += effect.Apply;
                onMainTargetEffect += OnEffect;
            }

            onAttack += ApplyEffectOnMainTarget;
        }
        if (AffectsTargetsOnPath)
        {
            foreach (var effect in OnPathTargetsEffects)
            {
                onPathEffect += effect.Apply;
                onPathEffect += OnEffect;
            }
            onAttack += ApplyEffectOnPathTargets;
        }

        if (EffectHappensOnce)
        {
            onEffect += delegate(Effectable effectable) { SingleEffectHappened = true;};
        }

        onAttackEnd += delegate { TargetBank.Targets.Clear();};
        onAttackEnd += delegate { SingleEffectHappened = false; };

    }
    
    
    
    
    

}
