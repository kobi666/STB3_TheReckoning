using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using System;
using Sirenix.OdinInspector;

public class AnimationController : MonoBehaviour
{

    public AnimationClip Clip;
    
    public AnimancerState CurrentAnimationState;
    public void PlaySingleAnimation(AnimationClip clip) {
        if (clip != null) {
            AnimancerState a_state = animancer.Play(clip);
            a_state.Events.OnEnd += ReturnToCurrentStateFromSingleAnimation;
        }
    }

    public void ReturnToCurrentStateFromSingleAnimation() {
        if (CurrentAnimationState != null) {
        animancer.Play(CurrentAnimationState);
        }
        else {
            animancer.Stop();
        }
    }

    public void PlayLoopingAnimation(AnimationClip clip) {
        if (clip != null) {
        CurrentAnimationState = animancer.Play(clip);
        }
    }
    
    public void PlayLoopingAnimation() {
        if (Clip != null) {
            CurrentAnimationState = animancer.Play(Clip);
        }
    }

    public void PlayFiniteAnimation(AnimationClip clip) {
        if (clip != null) {
        CurrentAnimationState.Stop();
        var state = animancer.Play(clip);
        state.Events.OnEnd += delegate { state.IsPlaying = false;};
        }
    }

    public void PlayFiniteAnimationWithAction(AnimationClip clip, Action OnEndAction) {
        if (clip != null) {
        CurrentAnimationState?.Stop();
        var state = animancer.Play(clip);
        state.Events.OnEnd += delegate { state.IsPlaying = false;};
        state.Events.OnEnd += OnEndAction;
        //state.Events.OnEnd += delegate {gameObject.SetActive(false);};
        state.Events.OnEnd += delegate { state.Events.OnEnd = null;};
        }
    }
    // Start is called before the first frame update
    public AnimancerComponent animancer;
    
    protected void Awake() {
        Animancer.WarningType.EndEventInterrupt.Disable();
        animancer = GetComponent<AnimancerComponent>() ?? null;
    }

    
    void OnDisable()
    {
        CurrentAnimationState = null;
    }
}
