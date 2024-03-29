﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum UnitStates
{
    None,
    Idle,
    
    MovingToBattlePos,
    InDirectBattle,
    AssistingBattle,
    MovingAlongPath,
    MoveToBasePosition,
    Death,
    Testing
}




[Serializable][BoxGroup]//[GUIColor(1f,0f,0f)]
public class UnitState : ObjectState<GenericUnitController>
{
    [SerializeReference][TypeFilter("GetSingleBehaviors")][GUIColor(1,0,0)]
    public List<UnitBehavior> OnEnterBehvaior;

    

    [SerializeReference][TypeFilter("GetConcurrentBehaviors")][GUIColor(0,1,0)]
    public List<UnitBehavior> InStateBehavior;
    
    [SerializeReference][TypeFilter("GetSingleBehaviors")][GUIColor(0,0.5f,1)]
    public List<UnitBehavior> OnExitBehavior;
    
    private bool extraStateConditions = false;

    [SerializeReference] [TypeFilter("GetConditions")][ShowIf("extraStateConditions")]
    public List<UnitStateCondition> SpecialEnterConditions;

    
    [SerializeReference][TypeFilter("GetConditions")][ShowIf("extraStateConditions")]
    public List<UnitStateCondition> SpecialExitConditions;
    
    [PropertyOrder(-1)][GUIColor(0.8f , 0 , 0.9f)][InfoBox("-------------------")]
    public UnitStates stateName;
    
    [ValidateInput("checkIfEmptyAutomatic", "Next State will Default")]
    public UnitStates automaticNextState;

    bool checkIfEmptyAutomatic()
    {
        return automaticNextState != UnitStates.None;
    }
    
    
    public override string AutomaticNextState
    {
        get => automaticNextState.ToString();
        set
        {
            automaticNextState = (UnitStates)Enum.Parse(typeof(UnitStates),value,true);
        }
    }
    

    public override string StateName { get => stateName.ToString(); }
    public override void InitState(GenericUnitController t)
    {
        foreach (var behavior in OnEnterBehvaior)
        {
            behavior.Init(SMObject,this);
            onStateEnterActions += behavior.InvokeBehavior;
            
        }

        foreach (var behavior in InStateBehavior)
        {
            behavior.Init(SMObject,this);
            inStateActions += behavior.InvokeBehavior;
            stateExitConditions += behavior.ExecCondition;
        }

        foreach (var behavior  in OnExitBehavior)
        {
            behavior.Init(SMObject,this);
            onStateExitActions += behavior.InvokeBehavior;
        }

        foreach (var condition in SpecialEnterConditions)
        {
            stateEnterConditions += condition.EvalCondition;
        }

        foreach (var condition in SpecialExitConditions)
        {
            stateExitConditions += condition.EvalCondition;
        }
    }

    private static IEnumerable<Type> GetBehaviors()
    {
        return UnitBehavior.GetUnitBehaviors();
    }

    private static IEnumerable<Type> GetConcurrentBehaviors()
    {
        return UnitConcurrentBehavior.GetConcurrentBehaviors();
    }

    private static IEnumerable<Type> GetSingleBehaviors()
    {
        return UnitSingleBehvaior.GetSingleBehaviors();
    }

    private static IEnumerable<Type> GetConditions()
    {
        return UnitStateCondition.GetUnitExitStateConditions();
    }
}

[Serializable]
public abstract class  UnitStateCondition
{
    public abstract bool EvalCondition();
    public static IEnumerable<Type> GetUnitExitStateConditions()
    {
        var q = typeof(UnitStateCondition).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(UnitStateCondition))); // Excludes classes not inheriting from BaseClass
        return q;
    }
    
}

public static class GetRandomGuiColor
{
    public static String GetS()
    {
        float a = Random.Range(0.1f, 1f);
        float b = Random.Range(0.1f, 1f);
        float c = Random.Range(0.1f, 1f);
        string s = a.ToString() + b.ToString() + c.ToString() + "1f";
        
        return s;
    }
}


[Serializable][FoldoutGroup("UnitState")]
public abstract class UnitBehavior
{
    public UnitData UnitData;
    [FoldoutGroup("Components")]
    public GenericUnitController UnitObject;
    [FoldoutGroup("Components")]
    public EffectableTargetBank TargetBank;
    [FoldoutGroup("Components")]
    public EffectableTargetBank AttackAreaTargetBank;
    [FoldoutGroup("Components")]
    public UnitBattleManager UnitBattleManager;
    public abstract void Behavior();
    [HideInInspector]
    public UnitState parentState;

    public void Init(GenericUnitController unit, UnitState parentState)
    {
        UnitData = unit.Data;
        UnitObject = unit;
        TargetBank = UnitObject.EffectableTargetBank;
        UnitBattleManager = UnitObject.UnitBattleManager;
        AttackAreaTargetBank = UnitBattleManager.MeleeWeapon.TargetBank;
        InitBehavior();
    }

    public abstract void InitBehavior();

    public void InvokeBehavior()
    {
        Behavior();
    }

    public abstract bool ExecCondition();
    
    
    public static IEnumerable<Type> GetUnitBehaviors()
    {
        var q = typeof(UnitBehavior).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(UnitBehavior))); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
}

[Serializable]
public abstract class UnitConcurrentBehavior : UnitBehavior
{
    public static IEnumerable<Type> GetConcurrentBehaviors()
    {
        var q = typeof(UnitConcurrentBehavior).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(UnitConcurrentBehavior))); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
}

[Serializable]
public abstract class UnitSingleBehvaior : UnitBehavior
{
    public static IEnumerable<Type> GetSingleBehaviors()
    {
        var q = typeof(UnitSingleBehvaior).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(UnitSingleBehvaior))); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
}



