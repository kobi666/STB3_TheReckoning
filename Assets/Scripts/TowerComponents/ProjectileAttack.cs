﻿using System;
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
    public ProjectileAttackProperties AttackProperties = new ShootOneProjectile();
    
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
        AttackProperties.Attack(singleTarget,SingleTargetPosition);
    }

    public override void StopAttack()
    {
        //not implamented for now
    }
    
    
    public override List<Effect> GetEffects()
    {
        List<Effect> listEffect = new List<Effect>();
        foreach (var projEffect in AttackProperties.Projectiles)
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

    public override void InitlizeAttack(GenericWeaponController weapon)
    {
        AttackProperties.InitializeAttack(weapon);
    }
}







