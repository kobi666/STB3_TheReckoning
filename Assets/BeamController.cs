using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BeamController : SerializedMonoBehaviour
{
    private LineRenderer LineRenderer;
    
    private void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
    }
}
