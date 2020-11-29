using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public abstract class SplineAttackProperties
{
    public EffectableTargetBank TargetBank;
    public abstract void Attack(Effectable singleTarget, Vector2 SingleTargetPosition);

    public abstract List<SplineBehavior> Splines { get; set;}


    public abstract void InitlizeAttack();
    public bool LocksOnToTarget;

}
