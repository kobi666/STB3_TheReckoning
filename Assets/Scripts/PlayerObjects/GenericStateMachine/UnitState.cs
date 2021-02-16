using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public enum UnitStates
{
    None,
    Idle,
    MovingToBattlePos,
    InDirectBattle,
    AssistingBattle,
    MovingAlongPath,
    Death
}

[Serializable]
public class UnitState : ObjectState<GenericUnitController>
{
    [SerializeReference][TypeFilter("GetBehaviors")]
    public List<UnitBehavior> OnEnterBehvaior;

    [SerializeReference][TypeFilter("GetBehaviors")]
    public List<UnitBehavior> InStateBehavior;
    
    [SerializeReference][TypeFilter("GetBehaviors")]
    public List<UnitBehavior> OnExitBehavior;
    
    private bool extraStateConditions = false;

    [SerializeReference] [TypeFilter("GetConditions")][ShowIf("extraStateConditions")]
    public List<UnitStateCondition> SpecialEnterConditions;

    
    [SerializeReference][TypeFilter("GetConditions")][ShowIf("extraStateConditions")]
    public List<UnitStateCondition> SpecialExitConditions;
    
    public UnitStates stateName;
    public override string StateName { get => stateName.ToString(); }
    public override void InitState(GenericUnitController t)
    {
        foreach (var behavior in OnEnterBehvaior)
        {
            onStateEnterActions += behavior.InvokeBehavior;
        }

        foreach (var behavior in InStateBehavior)
        {
            inStateActions += behavior.InvokeBehavior;
            stateExitConditions += behavior.ExecCondition;
        }

        foreach (var behavior  in OnExitBehavior)
        {
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

[Serializable]
public abstract class UnitBehavior
{
    public UnitData UnitData;
    public GenericUnitController UnitObject;
    public abstract void Behavior();

    public void Init(GenericUnitController unit)
    {
        UnitData = unit.Data;
        UnitObject = unit;
        
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



