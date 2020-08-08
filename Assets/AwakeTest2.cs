using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeTest2 : AwakeTest
{
    protected void Awake()
    {
        Debug.LogWarning(this.GetType());   
        base.Awake();
    }
}
