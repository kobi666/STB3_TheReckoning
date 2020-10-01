using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix;

public class ProjectileCreationData
{
    public GenericProjectile projectileBase;
    public ProjectileMovementDelegate MovementFunction;
    public Action<Effectable>[] singleTargetEffects;
    public Action<Effectable[]>[] multiTargerEffects;
    public bool isDirectHitProjectile;
    public bool isAOEProjectile;
    public bool isSingletargetProjectile;
    public bool isHomingProjectile;
}
