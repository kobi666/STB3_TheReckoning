using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ProjectileAttack : WeaponAttack
{
    [TypeFilter("GetProjectileAttacks")]
    [LabelText("Attack Function")]
    [SerializeReference]
    public ProjectileAttackProperties ProjectileAttackProperties = new ShootOneProjectile();
    
    private static IEnumerable<Type> GetProjectileAttacks()
    {
        var q = typeof(ProjectileAttackProperties).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(ProjectileAttackProperties))); // Excludes classes not inheriting from BaseClass
        return q;
    }


    public override void Attack(Effectable singleTarget, Vector2 SingleTargetPosition)
    {
        ProjectileAttackProperties.Attack(singleTarget,SingleTargetPosition);
    }

    public override void StopAttack()
    {
        //not implamented for now
    }
    
    
    public override List<Effect> GetEffects()
    {
        List<Effect> listEffect = new List<Effect>();
        foreach (var projEffect in ProjectileAttackProperties.Projectiles)
        {
            foreach (var ef in projEffect.projectileEffect.onHitEffects)
            {
                listEffect.Add(ef);
            }

            foreach (var ef in projEffect.projectileEffect.onPositionReachedEffects)
            {
                listEffect.Add(ef);
            }
        }

        return listEffect;
    }

    public override List<Effect> GetEffectList()
    {
        return GetEffects();
    }

    public override void UpdateEffect(Effect effectUpdate, List<Effect> appliedEffects)
    {
        if (effectUpdate != null){
            foreach (var effect in appliedEffects)
            {
                if (effectUpdate.EffectName() == effect.EffectName())
                {
                    effect.UpdateEffectValues(effectUpdate);
                }

                foreach (var projdata in ProjectileAttackProperties.Projectiles)
                {
                    projdata.UpdateProjectilePool();
                }
            }
        }
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
        foreach (var fp in ProjectileAttackProperties.GetFinalPoints())
        {
           fp.UpdatePositionAccordingToRadius(s); 
        }
    }

    public override List<ProjectileFinalPoint> GetFinalPoints()
    {
        return ProjectileAttackProperties.GetFinalPoints();
    }

    public override void SetInitialFinalPointPosition()
    {
        ProjectileAttackProperties.SetInitialFinalPointPosition();
    }

    public override List<ProjectileExitPoint> GetExitPoints()
    {
        return ProjectileAttackProperties.GetExitPoints();
    }

    public override void SetInitialExitPointPosition()
    {
        ProjectileAttackProperties.SetInitialExitPointPosition();
    }

    public override void InitlizeAttack(GenericWeaponController weapon)
    {
        ProjectileAttackProperties.InitializeAttack(weapon);
    }
}







