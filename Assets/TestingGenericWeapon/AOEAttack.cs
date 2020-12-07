using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AOEAttack : WeaponAttack
{
    public List<AOEBehavior> AoeBehaviors;
    public override void Attack(Effectable singleTarget, Vector2 SingleTargetPosition)
    {
        throw new System.NotImplementedException();
    }

    public override void InitlizeAttack()
    {
        throw new System.NotImplementedException();
    }
}
