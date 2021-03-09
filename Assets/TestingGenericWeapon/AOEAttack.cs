using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class AOEAttack : WeaponAttack
{
    public List<AOEBehavior> AoeBehaviors;
    public float AOESize = 1;

    public virtual void AdditionalinitBehaviors()
    {
        
    }
    public override void Attack(Effectable singleTarget, Vector2 SingleTargetPosition)
    {
        foreach (var ab in AoeBehaviors)
        {
              ab.StartAOEBehavior();          
        }
    }

    public override void StopAttack()
    {
        StopAOEAttack();
    }

    public override List<Effect> GetEffects()
    {
        List<Effect> listEffect = new List<Effect>();
        foreach (var aoebeahavior in AoeBehaviors)
        {
            foreach (var aoeEffect in aoebeahavior.Effects)
            {
                foreach (var effect in aoeEffect.Effects)
                {
                    listEffect.Add(effect);
                }
            }
        }

        return listEffect;
        
    }

    public override void SetEffectList(List<Effect> effects)
    {
        foreach (var aoebeahavior in AoeBehaviors)
        {
            foreach (var aoeEffect in aoebeahavior.Effects)
            {
                aoeEffect.Effects.Clear();
                aoeEffect.Effects = effects;
            }
        }
    }

    public override List<TagDetector> GetTagDetectors()
    {
        List<TagDetector> ltd = new List<TagDetector>();
        foreach (var aoeBehavior in AoeBehaviors)
        {
            foreach (var aoeController in aoeBehavior.AoeControllers)
            {
                ltd.Add(aoeController.Detector);
            }
        }

        return ltd;
    }

    public override void UpdateRange(float RangeSizeDelta, List<TagDetector> detectors)
    {
        foreach (var detector in detectors)
        {
            detector.UpdateSize(RangeSizeDelta);
        }
    }

    public override List<ProjectileFinalPoint> GetFinalPoints()
    {
        return new List<ProjectileFinalPoint>();
    }

    public override void SetInitialFinalPointPosition()
    {
        
    }

    public override List<ProjectileExitPoint> GetExitPoints()
    {
        return new List<ProjectileExitPoint>();
    }

    public override void SetInitialExitPointPosition()
    {
        
    }

    public override void InitlizeAttack(GenericWeaponController weapon)
    {
        AOESize = weapon.Data.componentRadius;
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

[Serializable]
public class TriggerAOEOnce : AOEAttack
{
    public override void AdditionalinitBehaviors()
    {
        
    }

    public override void Attack(Effectable singleTarget, Vector2 SingleTargetPosition)
    {
        foreach (var aoeBehavior in AoeBehaviors)
        {
            aoeBehavior.StartAOEBehavior();
        }
    }

    public override List<TagDetector> GetTagDetectors()
    {
        List<TagDetector> ds = new List<TagDetector>();
        foreach (var aoeb in AoeBehaviors)
        {
            foreach (var aoec in aoeb.AoeControllers)
            {
                ds.Add(aoec.Detector);
            }
        }

        return ds;
    }

    public override List<ProjectileFinalPoint> GetFinalPoints()
    {
        return new List<ProjectileFinalPoint>();
    }
    
    public static IEnumerable<Type> GetAOEAttacks()
    {
        var q = typeof(AOEAttack).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(AOEAttack))); // Excludes classes not inheriting from BaseClass
        
        return q;
    }

    

    public override List<ProjectileExitPoint> GetExitPoints()
    {
        return new List<ProjectileExitPoint>();
    }

    
    
    
}
