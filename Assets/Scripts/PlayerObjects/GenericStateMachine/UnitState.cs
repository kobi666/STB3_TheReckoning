using System;
using System.Collections;
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
    Death,
    Testing
}




[Serializable][BoxGroup]//[GUIColor(1f,0f,0f)]
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
            behavior.Init(SMObject);
            onStateEnterActions += behavior.InvokeBehavior;
            
        }

        foreach (var behavior in InStateBehavior)
        {
            behavior.Init(SMObject);
            inStateActions += behavior.InvokeBehavior;
            stateExitConditions += behavior.ExecCondition;
        }

        foreach (var behavior  in OnExitBehavior)
        {
            behavior.Init(SMObject);
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
    public GenericUnitController UnitObject;
    public EffectableTargetBank TargetBank;
    public abstract void Behavior();

    public void Init(GenericUnitController unit)
    {
        UnitData = unit.Data;
        UnitObject = unit;
        TargetBank = UnitObject.EffectableTargetBank;
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



