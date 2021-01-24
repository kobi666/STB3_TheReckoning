using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ComponentSpriteController : MonoBehaviour,IQueueable<ComponentSpriteController>
{
    

    public Type QueueableType { get; set; }
    public PoolObjectQueue<ComponentSpriteController> QueuePool { get; set; }
    public void OnEnqueue()
    {
        
    }
}
