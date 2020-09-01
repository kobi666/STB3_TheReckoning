using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  System;
using System.Linq;

public abstract class AOEProjectile : OnTargetReachedProjectile
{
    private bool EffectOnceLock = false;
    protected void OnDisable()
    {
        //placeholderAction = null;
        base.OnDisable();
    }

    protected void OnEnable()
    {
        EffectOnceLock = false;
        TargetBank?.Targets.Clear();
        TargetBank?.debugTargetNames.Clear();
    }
    
    
    
    
    //action coming from spawner, added to other actions set on this AOE projectile
    public Action<Effectable> placeholderAction;

    public event Action<Effectable> onTargetReachEffect;

    public void OnTargetReachEffect(Effectable effectable)
    {
        onTargetReachEffect?.Invoke(effectable);
    }
    
    
    public void OnTargetReachAreaOfEffect()
    {
        if (placeholderAction != null)
        {
            onTargetReachEffect += placeholderAction;
        }

        
        Effectable[] effectables = TargetBank.Targets.Values.ToArray();
        
        foreach (var effectable in effectables)
        {
            if (effectable != null)
            {
                OnTargetReachEffect(effectable);
            }

        }
        
        onTargetReachEffect -= placeholderAction;
    }


    public RangeDetector RangeDetector
    {
        get => TargetBank.RangeDetector;
    }
    
    public EffectableTargetBank TargetBank;
    
    public override void OnTargetReachedAction() {
        //SpriteRenderer.enabled = false;
        if (EffectOnceLock == false)
        {
            gameObject.SetActive(false);
            PlayOnHitAnimationAtPosition(null, TargetPosition);
            OnTargetReachAreaOfEffect();
            EffectOnceLock = true;
        }
    }
    // Start is called before the first frame update
    protected void Awake()
    {
        base.Awake();
        TargetBank = GetComponent<EffectableTargetBank>() ?? null;
        
    }
    
    protected void Start()
    {
        TargetBank = GetComponent<EffectableTargetBank>() ?? null;
    }

    protected void Update()
    {
        base.Update();
        if ((Vector2) transform.position == TargetPosition)
        {
            OnTargetPositionReached();
        }
    }

    
    
}
