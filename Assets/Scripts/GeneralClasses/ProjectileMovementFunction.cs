using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class ProjectileMovementFunction
{
    [ShowInInspector]
    public ProjectileMovementDelegate[] ProjectileMovmentFunctions = new ProjectileMovementDelegate[1];
    
    
    public Func<Vector2, Vector2, Vector2> StaticAssistingPosFunc;
    public Vector2 assistingPos = Vector2.zero;
    public Vector2 GetStaticAssistingPos(Vector2 originPos, Vector2 targetPos)
    {
        if (StaticAssistingPosFunc == null)
        {
            return assistingPos;
        }
        else
        {
            return StaticAssistingPosFunc.Invoke(originPos, targetPos);
        }
    }
    
    
    public float assistingFloat1;
    public float assistingFloat2;
    
    

    public void Invoke(Transform projectileTransform, Vector2 originPos, Vector2 targetPos, float speed,
        ref float progressCounter)
    {
        foreach (var mf in ProjectileMovmentFunctions)
        {
            mf?.Invoke(projectileTransform,originPos,targetPos, assistingPos, speed,assistingFloat1, assistingFloat2, ref progressCounter);
        }
        //ProjectileMovmentFunction?.Invoke(projectileTransform,originPos,targetPos, assistingPos, speed,assistingFloat1, assistingFloat2, ref progressCounter);
    }
    
}

[System.Serializable]
public delegate void ProjectileMovementDelegate(Transform projectileTransform, Vector2 originPos, Vector2 TargetPos, Vector2 assistingPos,
    float speed,
    float assistingFloat1, float assistinfloat2, ref float referenceFloat);





