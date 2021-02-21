using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

[Serializable]
public class AOEEffect : IHasEffectAnimation
{
    [TypeFilter("GetFilteredTypeList")][SerializeReference]
    public List<Effect> Effects;
    private List<GenericAOEController> AoeControllers;
    
    private static IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(Effect).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(Effect))); // Excludes classes not inheriting from BaseClass
        
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
    private int targetsCounter;

    public List<(Effectable,bool)> Targets = new List<(Effectable,bool)>();
    public List<string> TargetsNames = new List<string>();

    private void GetTargets()
    {
        if (Targets != null)
        {
            Targets.Clear();
        }
        if (EffectStacks) {
            foreach (var aoeController in AoeControllers)
            {
                foreach (var ef in aoeController.TargetBank.Targets.Values)
                {
                    if (ef.Item1 != null)
                    {
                        Targets.Add(ef);
                    }
                }
            }
        }

        if (!EffectStacks)
        {
            TargetsNames?.Clear();
            foreach (var aoeController in AoeControllers)
            {
                foreach (var ef in aoeController.TargetBank.Targets.Values)
                {
                    if (TargetsNames != null) {
                        if (ef.Item1 != null && !TargetsNames.Contains(ef.Item1.name))
                        {
                            Targets.Add(ef);
                            TargetsNames.Add(ef.Item1.name);
                        }
                    }
                }
            }
        }
    }
    
    public async void ApplyEffectOnce()
    {
        //change later to field based delay
        await Task.Delay(50);
        GetTargets();
        if (Targets != null)
        {
            foreach (var ef in Targets)
            {
                //onEffect?.Invoke(ef);
                OnEffect(ef.Item1,ef.Item1.transform.position);
            }
        }
    }
    public bool EffectInProgress = false;

    public event Action onAOEEffect;

    public void OnAOEEffect()
    {
        targetsCounter = 0;
        onAOEEffect?.Invoke();
    }
    public async void ApplyEffectForDuration()
    {
        //change later to field based delay
        await Task.Delay(5);
        if (!EffectInProgress)
        {
            EffectInProgress = true;
            DurationCounter = 0;
            intervalCounter = 0;
            while (DurationCounter < EffectDuration && EffectInProgress == true)
            {
                DurationCounter += StaticObjects.DeltaGameTime;
                intervalCounter += StaticObjects.DeltaGameTime;
                if (intervalCounter > EffectInterval)
                {
                    GetTargets();
                    if (Targets != null) {
                        intervalCounter = 0;
                        foreach (var effectable in Targets)
                        {
                            OnEffect(effectable.Item1,effectable.Item1.transform.position);
                        }
                    }
                }
                await Task.Yield();
            }
            EffectInProgress = false;
        }
    }
    
    public event Action<Effectable,Vector2> onEffect;
    

    public void OnEffect(Effectable ef,Vector2 targetPos)
    {
        if (ef.IsTargetable())
        {
            if (MaxTargets) {
                if (targetsCounter <= maxTargets) {
                onEffect?.Invoke(ef,targetPos);
                targetsCounter += 1;
                }
            }

            if (!MaxTargets)
            {
                onEffect?.Invoke(ef,targetPos);
            }
        }
    }

    public void InitEffect(List<GenericAOEController> aoes)
    {
        AoeControllers = aoes;
        foreach (var aoeController in AoeControllers)
        {
            aoeController.Detector.gameObject.SetActive(false);
            aoeController.Detector.gameObject.SetActive(true);
        }
        foreach (var effect in Effects)
        {
            onEffect += effect.Apply;
        }

        if (EffectOverTime)
        {
            onAOEEffect += ApplyEffectForDuration;
        }

        if (!EffectOverTime)
        {
            onAOEEffect += ApplyEffectOnce;
        }

        InitEffectAnimation();
    }

    void PlayAOEEffectAnimationOverTime()
    {
        foreach (var eac in EffectAnimationControllers)
        {
            eac.PlayAnimationForDuration(EffectDuration);
        }
    }

    void PlayEffectAnimationOnce()
    {
        foreach (var eac in EffectAnimationControllers)
        {
            eac.PlayAnimationOnce();
        }
    }

    public AnimationClip AnimationClip = null;
    [ItemCanBeNull] public List<EffectAnimationController> EffectAnimationControllers { get; set; } = new List<EffectAnimationController>();
    public void InitEffectAnimation()
    {
        if (AnimationClip != null)
        {
            foreach (var aoec in AoeControllers)
            {
                EffectAnimationController eac = GameObjectPool.Instance.GetEffectAnimationQueue().Get();
                EffectAnimationControllers.Add(eac);
                var transform = aoec.transform;
                eac.transform.position = transform.position;
                eac.transform.localScale = new Vector2(aoec.Range,aoec.Range);
                eac.transform.parent = transform;
            }

            foreach (var eac in EffectAnimationControllers)
            {
                eac.AnimationClip = AnimationClip;
            }
            
             
            if (EffectOverTime)
            {
                onAOEEffect += PlayAOEEffectAnimationOverTime;
            }

            if (!EffectOverTime)
            {
                onAOEEffect += PlayEffectAnimationOnce;
            }
        }
    }
}
