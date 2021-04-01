using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using System;

[System.Serializable]
public class SplineAttack : WeaponAttack
{
    [TypeFilter("GetSplineAttacks")] [LabelText("Attack Function")][SerializeReference]
    public SplineAttackProperties SplineAttackProperties = new OneSplineFromOriginToTarget();


    private static IEnumerable<Type> GetSplineAttacks()
    {
        var q = typeof(SplineAttackProperties).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(SplineAttackProperties)));
        
        return q;
    }
    
    public override void Attack(Effectable singleTarget, Vector2 SingleTargetPosition)
    {
        SplineAttackProperties.StartAsyncSplineAttack(singleTarget, SingleTargetPosition);
    }

    public override void StopAttack()
    {
        StopSplineAttack();
    }

    public override List<Effect> GetEffects()
    {
        List<Effect> listeffect = new List<Effect>();
        foreach (var sb in SplineAttackProperties.SplineBehaviors )
        {
            foreach (var se in sb.SplineEffect)
            {
                foreach (var mtef in se.MainTargetEffects)
                {
                    listeffect.Add(mtef);
                }

                foreach (var opef in se.OnPathTargetsEffects)
                {
                    listeffect.Add(opef);
                }
            }
        }

        return listeffect;
    }

    public override void SetEffectList(List<Effect> effects)
    {
        Debug.LogWarning("Redundent");
    }

    public override List<CollisionDetector> GetTagDetectors()
    {
        List<CollisionDetector> lr = new List<CollisionDetector>();
        lr.Add(parentWeaponController.RangeDetector);
        return lr;
    }

    public override void UpdateRange(float RangeSizeDelta, List<CollisionDetector> detectors)
    {
        float s = parentWeaponController.RangeDetector.GetSize() + RangeSizeDelta;
        parentWeaponController.RangeDetector.UpdateSize(s);
    }

    public override List<ProjectileFinalPoint> GetFinalPoints()
    {
        List<ProjectileFinalPoint> lpfp = new List<ProjectileFinalPoint>();
        foreach (var sb in SplineAttackProperties.SplineBehaviors)
        {
            foreach (var fp in sb.GetFinalPoints())
            {
                lpfp.Add(fp);
            }
        }

        return lpfp;
    }

    public override void SetInitialFinalPointPosition()
    {
        foreach (var sb in SplineAttackProperties.SplineBehaviors)
        {
            sb.SetInitialFinalPointPosition();
        }
        
    }

    public override List<ProjectileExitPoint> GetExitPoints()
    {
        List<ProjectileExitPoint> lpep = new List<ProjectileExitPoint>();
        foreach (var sb in SplineAttackProperties.SplineBehaviors)
        {
            foreach (var fp in sb.GetExitPoints())
            {
                lpep.Add(fp);
            }
        }

        return lpep;
    }

    public override void SetInitialExitPointPosition()
    {
        
    }

    public void StopSplineAttack()
    {
        SplineAttackProperties.StopAttack();
    }

    public override void InitlizeAttack(GenericWeaponController parentWeapon)
    {
        SplineAttackProperties.InitlizeProperties();
        SplineAttackProperties.SpecificPropertiesInit();
    }
}
