using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

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
    // Start is called before the first frame update
    public AnimancerComponent animancer;
    public abstract void PostAwake();
    private void Awake() {
        animancer = GetComponent<AnimancerComponent>();
        PostAwake();
    }
}
