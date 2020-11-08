using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

public abstract class BeamAttackFunction
{
    
    public EffectableTargetBank TargetBank;
    public abstract void Attack(Effectable singleTarget, Vector2 SingleTargetPosition);
    
    [OdinSerialize] public List<BeamBehavior> Projectiles = new List<BeamBehavior>();
    
    public abstract void InitlizeAttack();
    public bool LocksOnToTarget;
    
}
