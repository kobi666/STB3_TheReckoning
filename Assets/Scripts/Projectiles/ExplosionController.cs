using UnityEngine;
using Animancer;

public class ExplosionController : MonoBehaviour
{
    SpriteRenderer SR;
    AnimancerComponent animancer;
    
    public AnimationClip ExplosionAnimation;
    private void Awake() {
        animancer = GetComponent<AnimancerComponent>() ?? null;
    }

}
