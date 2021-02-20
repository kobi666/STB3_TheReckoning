﻿using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent),typeof(Animator),typeof(AnimationController))]
public class EffectAnimationController : MonoBehaviour,IQueueable<EffectAnimationController>
{
    [HideInInspector] public bool extEffectInUse;
    private int sortingOrderInt;
    public static string DefaultName = "EffectAnimationController";
    public SpriteRenderer SpriteRenderer;
    public AnimancerComponent AnimancerComponent;
    public AnimationController AnimationController;
    public Type QueueableType { get; set; }
    public PoolObjectQueue<EffectAnimationController> QueuePool { get; set; }

    public AnimationClip AnimationClip {
    
        get => AnimationController.Clip;
        set => AnimationController.Clip = value;
    }


    public void MoveToPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    private bool animationInProgress = false;

    public bool AnimationInProgress
    {
        get => animationInProgress;
        set
        {
            if (animationInProgress == true)
            {
                if (value == false)
                {
                    AnimationController.animancer.Stop();
                }
            }

            animationInProgress = value;
        }
    }

    public void StopEffectAnimation()
    {
        AnimationController.animancer.Stop();
    }

    public void PlayAnimationOnce()
    {
        AnimationController.PlaySingleAnimation(AnimationClip);
    }

    public void PlayLoopingAnimation()
    {
        AnimationController.PlayLoopingAnimation();
    }

    private float AnimationTimeCounter = 0;
    private float MaxAnimationTime = 0;
    public async void PlayAnimationForDuration(float AnimationTimeSeconds)
    {
        AnimationTimeCounter = 0;
        MaxAnimationTime = AnimationTimeSeconds;
        if (!AnimationInProgress)
        {
            AnimationInProgress = true;
            AnimationController.PlayLoopingAnimation(AnimationClip);
            while (AnimationTimeCounter <= MaxAnimationTime)
            {
                AnimationTimeCounter += StaticObjects.Instance.DeltaGameTime;
            }

            AnimationInProgress = false;
        }
    }

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        AnimancerComponent = GetComponent<AnimancerComponent>();
        AnimationController = GetComponent<AnimationController>();
        sortingOrderInt = SortingLayer.NameToID("Projectiles");
        SpriteRenderer.sortingLayerID = sortingOrderInt;
    }

    public void PlayOnceAtPosition(Vector2 targetPos)
    {
        transform.position = targetPos;
        PlayAnimationOnce();
    }

    public void OnEnqueue()
    {
       gameObject.SetActive(false); 
    }

    public void OnDequeue()
    {
        
    }
}