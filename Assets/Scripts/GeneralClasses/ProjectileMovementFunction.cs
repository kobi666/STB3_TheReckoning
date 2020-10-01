using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class ProjectileMovementFunction
{
    public delegate void MainMovementFunction(Transform projectileTransform, Vector2 originPos, Vector2 TargetPos,
        float speed,
        float assistingFloat1, ref float referenceFloat);

    public event Action<Transform, Vector2, Vector2, float, float> AdditionalMovementActions;
}

[System.Serializable]
public delegate void ProjectileMovementDelegate(Transform projectileTransform, Vector2 originPos, Vector2 TargetPos,
    float speed,
    float assistingFloat1, ref float referenceFloat);


