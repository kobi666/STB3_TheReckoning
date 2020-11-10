using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

public abstract class SplineAttackFunction
{
    
    public EffectableTargetBank TargetBank;
    public abstract void Attack(Effectable singleTarget, Vector2 SingleTargetPosition);
    
    [OdinSerialize] public List<SplineBehavior> Beams = new List<SplineBehavior>();
    
    public abstract void InitlizeAttack();
    public bool LocksOnToTarget;

}
