using UnityEngine;

public class AOEAnimationController : AnimationController
{
    public AnimationClip EffectAnimation;

    public void PlayEffectAnimation() {
        if (EffectAnimation != null) {
            PlaySingleAnimation(EffectAnimation);
        }
    } 
    
    void Start()
    {
        
    }
    
}
