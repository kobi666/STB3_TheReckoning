using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix;
using Sirenix.OdinInspector;


[BoxGroup]
public class ProjectileBehaviorData
{
    public ProjectileMovementFunction MovementFunctions = new ProjectileMovementFunction();
    public Action<Effectable>[] singleTargetEffects = new Action<Effectable>[1];
    public Action<Effectable[]>[] multiTargerEffects = new Action<Effectable[]>[1];
    public bool isDirectHitProjectile;
    public bool isAOEProjectile;
    public bool isSingletargetProjectile;
    public bool isHomingProjectile;
}
