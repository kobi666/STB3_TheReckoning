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
    [OdinSerialize]
    public ProjectileAttackProperties AttackProperties = new ShootOneProjectile();
    
    private static IEnumerable<Type> GetProjectileAttacks()
    {
        var q = typeof(ProjectileAttackProperties).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(ProjectileAttackProperties).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }


    public override void Attack(Effectable singleTarget, Vector2 SingleTargetPosition)
    {
        AttackProperties.Attack(singleTarget,SingleTargetPosition);
    }

    public override void InitlizeAttack(GenericWeaponController weapon)
    {
        AttackProperties.InitializeAttack(weapon);
    }
}







