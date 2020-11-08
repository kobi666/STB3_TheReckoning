using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BeamBehavior : MonoBehaviour
{
    public BeamDynamicData BeamDynamicData;
    public ProjectileExitPoint ExitPoint;
    public BeamController BeamController;
    public float beamDuration;
    public (float, float) BeamWidthStartEnd { get; set; } = (0.2f, 0.2f);
    public abstract void RenderBeam(Vector2 originPosition, Vector2 targetPosition);

    public bool SingleTargetBeam;
    public bool AreaEffectBeam;
    [ShowIf("AreaEffectBeam")] public float EffectRadius; 
    
    public bool RailBeam;
    [ShowIf("RailBeam")] public int MaxTargets;
    public bool MaterialBeam;
    
}
