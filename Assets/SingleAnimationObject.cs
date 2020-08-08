using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAnimationObject : AnimationController,IQueueable<SingleAnimationObject>
{
    
    public AnimationClip AnimationClip = null;
    public PoolObjectQueue<SingleAnimationObject> QueuePool {get;set;}

    public void OnEnqueue() {
        gameObject.SetActive(false);
    }

    public override void PostAwake() {

    }

    
    public void PlayOnceAndDisable() {
        if (AnimationClip != null) {
            PlayFiniteAnimationWithAction(AnimationClip, delegate {gameObject.SetActive(false);});
        }
        else if (AnimationClip == null) {
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        QueuePool?.ObjectQueue.Enqueue(this);
    }
    
}
