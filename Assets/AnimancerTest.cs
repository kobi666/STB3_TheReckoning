using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Animancer;

[RequireComponent(typeof(AnimancerComponent))]
public class AnimancerTest : MonoBehaviour
{
    // Start is called before the first frame update
    AnimancerComponent animancer;
    PlayerInput input;
    event Action onDirectBattleAttack;
    public void OnDirectBattleAttack() {
        onDirectBattleAttack?.Invoke();
    }
    event Action onWalking;
    public void OnWalking() {
        onWalking?.Invoke();
    }
    event Action onDeath;
    public void OnDeath() {
        onDeath?.Invoke();
    }
    event Action onSpecialAttack1;
    event Action onSpecialAttack2;

    event Action onShoot;

    event Action onIdle;
    public void OnIdle() {
        onIdle?.Invoke();
    }

    AnimancerState CurrentAnimationState;

    private void PlaySingleAnimation(AnimationClip clip) {
        if (clip != null) {
            AnimancerState a_state = animancer.Play(clip);
            a_state.Events.OnEnd += ReturnToCurrentStateFromSingleAnimation;
        }
    }

    private void ReturnToCurrentStateFromSingleAnimation() {
        if (CurrentAnimationState != null) {
        animancer.Play(CurrentAnimationState);
        }
        else {
            animancer.Stop();
        }
    }

    private void PlayLoopingAnimation(AnimationClip clip) {
        if (clip != null) {
        CurrentAnimationState = animancer.Play(clip);
        }
    }

    private void PlayFiniteAnimation(AnimationClip clip) {
        if (clip != null) {
        if (CurrentAnimationState != null) { CurrentAnimationState = null; animancer.Stop();}
        var state = animancer.Play(clip);
        state.Events.OnEnd += delegate { state.IsPlaying = false;};
        }
    }




    public AnimationClip WalkingAnimation;
    public AnimationClip IdleAnimation;
    public AnimationClip DirectBattleAttackAnimation;
    public AnimationClip DeathAnimation;
    public AnimationClip SpecialAttackAnimation1;
    public AnimationClip SpecialAttackAnimation2;
    public AnimationClip ShootingAnimation;


    private void Awake() {
        onDirectBattleAttack += delegate {PlaySingleAnimation(DirectBattleAttackAnimation);};
        onWalking += delegate {PlayLoopingAnimation(WalkingAnimation);};
        onDeath += delegate {PlayFiniteAnimation(DeathAnimation);};
        onIdle += delegate {PlayLoopingAnimation(IdleAnimation);};
        animancer = GetComponent<AnimancerComponent>();
        input = new PlayerInput();
        input.GamePlay.EastButton.performed += ctx => OnWalking();
        input.GamePlay.SouthButton.performed += ctx => OnDirectBattleAttack();
        input.GamePlay.WestButton.performed += ctx => OnDeath();
        input.GamePlay.NorthButton.performed += ctx => OnIdle();
    }

    private void OnEnable() {
        input.GamePlay.Enable();
    }
}
