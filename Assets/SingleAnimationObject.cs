using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAnimationObject : AnimationController,IQueueable<SingleAnimationObject>
{
    
    public AnimationClip OnStartAnimation = null;
    public PoolObjectQueue<SingleAnimationObject> QueuePool {get;set;}

    public void OnEnqueue() {

    }

    public override void PostAwake() {

    }

    
    void Start()
    {
        if (OnStartAnimation != null) {
            PlayFiniteAnimationWithAction(OnStartAnimation, delegate {QueuePool?.ObjectQueue.Enqueue(this);});
        }
        else if (OnStartAnimation == null) {
            QueuePool?.ObjectQueue.Enqueue(this);
        }
    }
    
}
