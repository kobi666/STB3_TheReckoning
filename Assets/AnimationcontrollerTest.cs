using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

public class AnimationcontrollerTest : MonoBehaviour
{
    [SerializeField]
    AnimancerComponent animancer;
    public AnimationClip moveAnimation;
    public AnimationClip AttackAnimation;
    public AnimationClip IdleAnimation;
    // Start is called before the first frame update
    void Start()
    {
        animancer = GetComponent<AnimancerComponent>() ?? null;
        animancer.Play(IdleAnimation);
        ACT.instance.TB1 += delegate {animancer.Play(AttackAnimation);};
        ACT.instance.TB1 += delegate {Debug.LogWarning("THIS HAPPENED");};
    }

    // Update is called once per frame
    
}
