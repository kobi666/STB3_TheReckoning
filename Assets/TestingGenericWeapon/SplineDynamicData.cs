using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineDynamicData
{
    public Effectable MainTarget;
    public Vector2? TargetPosition;
    public Effectable[] Targets;

    public void Clear()
    {
        MainTarget = null;
        TargetPosition = null;
        Targets = null;
    }
}
