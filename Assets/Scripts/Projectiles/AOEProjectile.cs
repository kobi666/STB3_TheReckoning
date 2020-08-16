using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  System;

public abstract class AOEProjectile : OnTargetReachedProjectile
{

    protected void OnDisable()
    {
        placeholderAction = null;
        TargetBank?.Targets.Clear();
        base.OnDisable();
    }
    
    
    
    
    //action coming from spawner, added to other actions set on this AOE projectile
    public Action<Effectable> placeholderAction;

    public event Action<Effectable> onTargetReachEffect;

    public void OnTargetReachEffect(Effectable effectable)
    {
        onTargetReachEffect?.Invoke(effectable);
    }
    
    
    void OnTargetReachAreaOfEffect()
    {
        if (placeholderAction != null)
        {
            onTargetReachEffect += placeholderAction;
        }
        foreach (var effectable in TargetBank.Targets)
        {
            if (effectable.Value != null)
            {
                OnTargetReachEffect(effectable.Value);
            }
        }
        onTargetReachEffect -= placeholderAction;
    }
    
    
    public RangeDetector RangeDetector;
    
    public EffectableTargetBank TargetBank;
    
    public override void OnTargetReachedAction() {
        //SpriteRenderer.enabled = false;
        gameObject.SetActive(false);
        PlayOnHitAnimationAtPosition(null,TargetPosition);
        OnTargetReachAreaOfEffect();
    }
    // Start is called before the first frame update
    protected void Awake()
    {
        base.Awake();
        TargetBank = GetComponent<EffectableTargetBank>() ?? null;
    }
    
    protected void Start()
    {
        base.Start();
        TargetBank = GetComponent<EffectableTargetBank>() ?? null;
    }

    
    
}
