using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.Serialization;
using Random = UnityEngine.Random;

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

    public override List<TagDetector> GetRangeDetectors()
    {
        List<TagDetector> lr = new List<TagDetector>();
        lr.Add(parentWeaponController.RangeDetector);
        return lr;
    }

    public override void UpdateRange(float RangeSizeDelta, List<TagDetector> detectors)
    {
        float s = parentWeaponController.RangeDetector.GetSize() + RangeSizeDelta;
        parentWeaponController.RangeDetector.SetSize(s);
        foreach (var fp in ProjectileAttackProperties.GetFinalPoints())
        {
           fp.UpdatePositionAccordingToRadius(s); 
        }
    }

    public override void InitlizeAttack(GenericWeaponController weapon)
    {
        ProjectileAttackProperties.InitializeAttack(weapon);
    }
}







