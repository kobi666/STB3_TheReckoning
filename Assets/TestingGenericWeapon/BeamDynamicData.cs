using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamDynamicData
{
    public Effectable Target;
    public Vector2? TargetPosition;

    public void Clear()
    {
        Target = null;
        TargetPosition = null;
    }
}
