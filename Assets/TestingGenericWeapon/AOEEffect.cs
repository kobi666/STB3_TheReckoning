using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class AOEEffect 
{
    [TypeFilter("GetFilteredTypeList")][OdinSerialize]
    public List<Effect> Effects;

    private List<GenericAOEController> AoeControllers;
    
    private static IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(Effect).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(Effect).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }

    public bool EffectStacks;
    
    private float intervalCounter;
    private float DurationCounter = 0;
    
    public bool EffectOverTime;
    
    [ShowIf("EffectOverTime")]
    public float EffectDuration;
    [ShowIf("EffectOverTime")]
    public float EffectInterval;

    public bool MaxTargets;
    [ShowIf("MaxTargets")] [HideLabel] public int maxTargets;

    public List<Effectable> Targets;
    public List<string> TargetsNames;

    private void GetTargets()
    {
        Targets.Clear();
        if (EffectStacks) {
            foreach (var aoeController in AoeControllers)
            {
                foreach (var ef in aoeController.TargetBank.Targets.Values)
                {
                    if (ef != null)
                    {
                        Targets.Add(ef);
                    }
                }
            }
        }

        if (!EffectStacks)
        {
            TargetsNames.Clear();
            foreach (var aoeController in AoeControllers)
            {
                foreach (var ef in aoeController.TargetBank.Targets.Values)
                {
                    if (ef != null && !TargetsNames.Contains(ef.name))
                    {
                        Targets.Add(ef);
                        TargetsNames.Add(ef.name);
                    }
                }
            }
        }
    }
    
    public void ApplyEffectOnce()
    {
        GetTargets();
        foreach (var ef in Targets)
            {
                OnEffect(ef);
            }
    }
    private bool EffectInProgress = false;
    
    public async void ApplyEffectForDuration()
    {
        if (!EffectInProgress)
        {
            EffectInProgress = true;
            DurationCounter = 0;
            intervalCounter = 0;
            while (DurationCounter < EffectDuration)
            {
                DurationCounter += StaticObjects.Instance.DeltaGameTime;
                intervalCounter += StaticObjects.Instance.DeltaGameTime;
                if (intervalCounter > EffectInterval)
                {
                    GetTargets();
                    intervalCounter = 0;
                    foreach (var effectable in Targets)
                    {
                        OnEffect(effectable);
                    }
                }
                await Task.Yield();
            }
            EffectInProgress = false;
        }
    }
    
    public event Action<Effectable> onEffect;

    public void OnEffect(Effectable ef)
    {
        if (ef.IsTargetable())
        {
            onEffect?.Invoke(ef);
        }
    }

    public void InitEffect(List<GenericAOEController> aoes)
    {
        AoeControllers = aoes;
        foreach (var effect in Effects)
        {
            onEffect += effect.Apply;
        }
    }
    
}
