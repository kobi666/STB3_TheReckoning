using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Animancer;

public abstract class AreaOfEffectController : MonoBehaviour,IQueueable<AreaOfEffectController>
{
    public Type QueueableType {get;set;}
    public Effector Effector;
    EffectableTargetBank TargetBank;
    public event Action onEffectTrigger;
    public AOEAnimationController AOEAnimationController;
    public void OnEffectTrigger() {
        onEffectTrigger?.Invoke();
    }

    public event Action<Effectable[]> onApplyEffect;
    public void OnApplyEffect(Effectable[] effectableTargets) {
        onApplyEffect?.Invoke(effectableTargets);
    }

    public void ApplyEffectsOnTargets() {
        int counter = 0;
        Effectable[] targets = new Effectable[TargetBank.Targets.Count];
        foreach (KeyValuePair<string,Effectable> item in TargetBank.Targets)
        {
            if (item.Value != null) {
            targets[counter] = item.Value;
            counter += 1;
            }
        }
        for (int i = 0; i < targets.Length; i++) {
            if (targets[i] != null) {
                OnApplyEffect(targets);
            }
        }
    }
    PoolObjectQueue<AreaOfEffectController> queuePool;
    public PoolObjectQueue<AreaOfEffectController> QueuePool {get => queuePool ;set {queuePool = value;}}
    public void OnEnqueue(){}
    public void OnDequeue()
    {
        
    }

    public abstract void PostStart();

    
    void Awake()
    {
        AOEAnimationController = GetComponent<AOEAnimationController>() ?? null;
        TargetBank = GetComponent<EffectableTargetBank>() ?? null;
        Effector = GetComponent<Effector>() ?? null;
        onEffectTrigger += AOEAnimationController.PlayEffectAnimation;
        onEffectTrigger += ApplyEffectsOnTargets;
    }
    void Start()
    {
        PostStart();
    }

    void OnDisable()
    {
        TargetBank.Targets.Clear();
        QueuePool?.ObjectQueue.Enqueue(this);
    }

}
