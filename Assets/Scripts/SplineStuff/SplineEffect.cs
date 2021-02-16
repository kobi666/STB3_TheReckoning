using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using System.Linq;
using Sirenix.Serialization;

[System.Serializable]
public class SplineEffect
{
    public bool MainTargetEffect = true;
    [HideLabel, ShowIf("MainTargetEffect"), TypeFilter("GetFilteredTypeList")][SerializeReference]
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
    [HideIf("EffectHappensOnce")]
    public bool EffectHappensAtInterval;
    
    
    public bool EffectStartsOnTargetPositionReached = false;
    [ShowIf("EffectHappensAtInterval")] public float EffectInterval = 0.25f;

    [ShowIf("EffectHappensAtInterval"), ShowInInspector]
    private float EffectIntervalCounter = 0;
    public event Action onInterval;
    
    public bool AffectsTargetsOnPath;

    [HideLabel, ShowIf("AffectsTargetsOnPath")] [TypeFilter("GetFilteredTypeList")][SerializeReference]
    public List<Effect> OnPathTargetsEffects = new List<Effect>()
    {
        new Damage()
    };

    private event Action<Effectable,Vector2> onPathEffect;
    private event Action<Effectable,Vector2> onMainTargetEffect;
    private event Action<Effectable,Vector2> onAttack;

    private void ApplyEffectOnMainTarget(Effectable ef,Vector2 targetPos)
    {
        if (TargetBank.Targets.ContainsKey(ef.name))
        {
            if (ef.IsTargetable())
            {
                onMainTargetEffect?.Invoke(ef,targetPos);
            }
        }
    }

    private (Effectable,bool)[] t_targets;

    private bool onPathEffectInProgress;
    private void ApplyEffectOnPathTargets(Effectable ef,Vector2 targetPos)
    {
        if (!onPathEffectInProgress)
        {
            onPathEffectInProgress = true;
            if (MainTargetEffect)
            {
                t_targets = TargetBank.Targets.Values.ToArray();
                foreach (var tef in t_targets)
                {
                    if (tef.Item1.name == ef.name)
                    {
                        continue;
                    }

                    if (tef.Item1?.IsTargetable() ?? false)
                    {
                        onPathEffect?.Invoke(tef.Item1,targetPos);
                    }
                }
            }
            else
            {
                t_targets = TargetBank.Targets.Values.ToArray();
                foreach (var t in t_targets)
                    {
                        if (t.Item1?.IsTargetable() ?? false)
                        {
                            onPathEffect?.Invoke(t.Item1,targetPos);
                        }
                    }
            }

            onPathEffectInProgress = false;
        }
    }

    private void EffectOnce(Effectable ef,Vector2 targetPos)
    {
        if (!m_SingleSingleEffectHappened)
        {
            onAttack(ef,targetPos);
        }
    }

    public void OnEffectTrigger(Effectable ef,Vector2 targetPos)
    {
        if (TargetBank.Targets.Any())
        {
            if (EffectHappensAtInterval)
            {
                EffectOnInterval(ef,targetPos);
            }

            if (EffectHappensOnce)
            {
                EffectOnce(ef,targetPos);
            }
        }
    }

    void EffectOnMainTarget(Effectable ef,Vector2 targetPos)
    {
        if (TargetBank.Targets.ContainsKey(ef.name))
        {
            if (ef.IsTargetable())
            {
                onPathEffect?.Invoke(ef,targetPos);
            }
        }
    }

    public event Action onEffectStart;

    public void OnEffectStart(Effectable ef, Vector2 pos)
    {
        onEffectStart?.Invoke();
    }
    public event Action onEffectEnd;

    public void OnEffectEnd()
    {
        onEffectEnd?.Invoke();
    }
    void EffectOnPathTargets(Effectable ef,Vector2 targetPos)
    {
        foreach (var target in TargetBank.Targets.Values)
        {
            if (target.Item1 == null)
            {
                continue;
            }

            if (MainTargetEffect)
            {
                if (target.Item1.name == ef.name)
                {
                    continue;
                }
            }

            if (target.Item1.IsTargetable())
            {
                onPathEffect?.Invoke(target.Item1,targetPos);
            }
        }
    }

    void EffectOnInterval(Effectable ef,Vector2 targetPos)
    {
        EffectIntervalCounter += StaticObjects.Instance.DeltaGameTime;
        if (EffectIntervalCounter >= EffectInterval)
        {
            onAttack?.Invoke(ef,targetPos);
            EffectIntervalCounter = 0;
        }
    }

    public event Action<Effectable,Vector2> onEffect;

    private void OnEffect(Effectable ef,Vector2 targetPos)
    {
        onEffect?.Invoke(ef, targetPos);
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
            onEffect += delegate(Effectable effectable,Vector2 targetPos) { SingleEffectHappened = true;};
        }

        onEffectEnd += delegate { TargetBank.Targets.Clear();};
        onEffectEnd += delegate { SingleEffectHappened = false; };

    }

    


    
    
}
