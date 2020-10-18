using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProjectileAttackSequence
{
    public static SerializedMonoBehaviour exec = null;

    [ValueDropdown("attackSequences")] public MethodInfo AttackSequence;

    private static ValueDropdownList<MethodInfo> attackSequences = ProjectileAttackSequences.Sequences();

    

    public int BaseNumOfProjectiles;

    public virtual int NumOfProjectiles()
    {
        return BaseNumOfProjectiles;
    }

    public float assistingFloat1;
    public float assistingFloat2;

    public virtual float AssistingFloat1
    {
        get => assistingFloat1;
        set => assistingFloat1 = value;
    }

    public virtual float AssistingFloat2
    {
        get => assistingFloat1;
        set => assistingFloat1 = value;
    }

    public void StartAttackSequence(PoolObjectQueue<GenericProjectile> pool, Effectable targetEffectable,
        Vector2 originPosition,
        Vector2 targetPosition, Quaternion direction, float assistingfloat1, float assistingFloat2,
        int numOfProjectiles)

    {
        
        
    }

}

public delegate IEnumerator ProjectileAttackDelegateIEnum<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8,
    out IEnumerator>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
