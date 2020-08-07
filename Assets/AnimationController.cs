using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using System;

public abstract class AnimationController : MonoBehaviour
{
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
        }
    }
    // Start is called before the first frame update
    public AnimancerComponent animancer;
    public abstract void PostAwake();
    private void Awake() {
        animancer = GetComponent<AnimancerComponent>() ?? null;
        PostAwake();
    }

    
    void OnDisable()
    {
        CurrentAnimationState = null;
    }
}
