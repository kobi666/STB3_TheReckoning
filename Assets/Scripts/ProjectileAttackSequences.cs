﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using System.Reflection;
using Sirenix.Utilities;
using UnityEngine.Assertions;

public class ProjectileAttackSequences 
{
    public static ValueDropdownList<MethodInfo> Sequences()
    {
        IList<Type> paramTypes = new List<Type>()
        {
            typeof(PoolObjectQueue<GenericProjectile>),
            typeof(Effectable),
            typeof(Vector2),
            typeof(Vector2),
            typeof(Quaternion),
            typeof(float),
            typeof(float),
            typeof(int)
            
        };
        ValueDropdownList<MethodInfo> list = new ValueDropdownList<MethodInfo>();
        var methods = typeof(ProjectileAttackSequences).GetMethods()
            .Where(x => x.ReturnType == typeof(IEnumerator))
            .Where(x => x.HasParamaters(paramTypes))
            .Select(x => x).ToList();

        foreach (var method in methods)
        {
            list.Add(method.Name, method);
        }
        return list;
    }
    
    

    
    
    
    
    public static IEnumerator SingleProjectile(PoolObjectQueue<GenericProjectile> pool, Effectable targetEffectable,
        Vector2 originPosition,
        Vector2 targetPosition, Quaternion direction, float assistingfloat1, float assistingFloat2, int assistingInt)
    {
        GenericProjectile proj = pool.GetInactive();
        proj.TargetPosition = targetPosition;
        proj.transform.position = originPosition;
        proj.EffectableTarget = targetEffectable ?? null;
        proj.transform.rotation = direction;
        proj.Activate();
        yield break;
    }
    
    
    
    public static IEnumerator ThreeProjectilesInARow(PoolObjectQueue<GenericProjectile> pool, Effectable targetEffectable,
        Vector2 originPosition,
        Vector2 targetPosition, Quaternion direction, float timeBetweenProjectiles, float assistingFloat2, int assistingInt)
    {
        GenericProjectile proj = pool.GetInactive();
        proj.TargetPosition = targetPosition;
        proj.transform.position = originPosition;
        proj.EffectableTarget = targetEffectable;
        proj.transform.rotation = direction;
        proj.Activate();
        yield break;
    }


    
}
