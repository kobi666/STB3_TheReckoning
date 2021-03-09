using UnityEngine;
using Animancer;

public class ExplosionController : MonoBehaviour
{
    SpriteRenderer SR;
    AnimancerComponent animancer;
    EnemyTargetBank targetBank;
    public AnimationClip ExplosionAnimation;
    private void Awake() {
        targetBank = GetComponent<EnemyTargetBank>() ?? null;
        animancer = GetComponent<AnimancerComponent>() ?? null;
    }

}
