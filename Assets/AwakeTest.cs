using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AwakeTest : MonoBehaviour
{
    protected void Awake()
    {
        Debug.LogWarning(this.GetType());
    }
}
