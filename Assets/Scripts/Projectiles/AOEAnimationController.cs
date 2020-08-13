using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using System;

public class AOEAnimationController : AnimationController
{
    public AnimationClip EffectAnimation;

    public void PlayEffectAnimation() {
        if (EffectAnimation != null) {
            PlaySingleAnimation(EffectAnimation);
        }
    } 
    
    

    public override void PostAwake() {

    }
    void Start()
    {
        
    }
    
}
