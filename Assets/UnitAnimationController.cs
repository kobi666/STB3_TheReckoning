using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Animancer;

[RequireComponent(typeof(AnimancerComponent))]
public class UnitAnimationController : AnimationController
{
    // Start is called before the first frame update
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

    




    public AnimationClip WalkingAnimation;
    public AnimationClip IdleAnimation;
    public AnimationClip DirectBattleAttackAnimation;
    public AnimationClip DeathAnimation;
    public AnimationClip SpecialAttackAnimation1;
    public AnimationClip SpecialAttackAnimation2;
    public AnimationClip ShootingAnimation;


    public override void PostAwake() {
        onDirectBattleAttack += delegate {PlaySingleAnimation(DirectBattleAttackAnimation);};
        onWalking += delegate {PlayLoopingAnimation(WalkingAnimation);};
        onDeath += delegate {PlayFiniteAnimation(DeathAnimation);};
        onIdle += delegate {PlayLoopingAnimation(IdleAnimation);};
        animancer = GetComponent<AnimancerComponent>();
        
        
    }

    private void OnEnable() {
        
    }
}
