using System;
using UnityEngine;

public class ComponentSpriteController : MonoBehaviour,IQueueable<ComponentSpriteController>
{
    

    public Type QueueableType { get; set; }
    public PoolObjectQueue<ComponentSpriteController> QueuePool { get; set; }
    public void OnEnqueue()
    {
        
    }

    public void OnDequeue()
    {
        
    }
}
