using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;


public class test3 : SerializedMonoBehaviour
{
    [OdinSerialize]
    private Action myEvent;

    [ShowInInspector]
    
    
    public event Action MyEvent
    {
        add { myEvent += value; }
        remove { myEvent -= value; }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
