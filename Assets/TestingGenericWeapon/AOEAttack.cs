using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AOEAttack : WeaponAttack
{
    public List<AOEBehavior> AoeBehaviors;
    public float AOESize = 1;
    public abstract void AdditionalinitBehaviors();
    public override void InitlizeAttack(GenericWeaponController weapon)
    {
        foreach (var aoeBehavior in AoeBehaviors)
        {
            aoeBehavior.AOESize = AOESize;
            aoeBehavior.initBehavior();
        }
        AdditionalinitBehaviors();
    }
    
    
    //need to refactor and convert to event based stop behavior
    public void StopAOEAttack()
    {
        foreach (var aoeBehavior in AoeBehaviors)
        {
            foreach (var effect in aoeBehavior.Effects)
            {
                effect.EffectInProgress = false;
            }
        }
    }
}

public class TriggerAOEOnce : AOEAttack
{
    public override void Attack(Effectable singleTarget, Vector2 SingleTargetPosition)
    {
        foreach (var aoeBehavior in AoeBehaviors)
        {
            aoeBehavior.StartAOEBehavior();
        }
    }

    public override void AdditionalinitBehaviors()
    {
        
    }
    
    
}
