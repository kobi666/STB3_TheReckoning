using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamUtilities : MonoBehaviour
{
    public static void StraightBeamold(LineRenderer lineRenderer, Vector2 originPos, Vector2 Targetpos, float nullFloat)
    {
        lineRenderer.SetPosition(0, originPos);
        lineRenderer.SetPosition(1, Targetpos);
    }

    public static void StraightBeam(LineRenderer lineRenderer, Vector2 originPos, Vector2 targetPos, float beamRotationspeed)
    {
        lineRenderer.SetPosition(0, originPos);
        int beamEndIndex = lineRenderer.positionCount - 1;
        lineRenderer.SetPosition(beamEndIndex, targetPos);    
    }
    
    //Requires range detector
    public static void BeamDamageOnArea(EffectableTargetBank effectableTargetBank, int damage)
    {
        foreach (var effectable in effectableTargetBank.Targets)
        {
            
            if (effectable.Value == null)
            {
                continue;
            }
            effectable.Value.ApplyDamage(damage);
            
        }
    }


    public static void ContinuousEffectOnSingleTarget(Action<Effectable> effectFunction, float perSecondRate)
    {
        
    }
}
