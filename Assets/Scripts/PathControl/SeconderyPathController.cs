using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeconderyPathController : SplinePathController
{
    public bool DebugPath;

    protected void Start()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        base.Start();
        lineRenderer.enabled = DebugPath;
    }
}
