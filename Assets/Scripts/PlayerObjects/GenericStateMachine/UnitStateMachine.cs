using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UnitStateMachine : GenericStateMachine<UnitState,GenericUnitController>
{
    [ShowInInspector]
    public UnitState currentState
    {
        get => CurrentState;
    }

    public override string EmptyStateName { get; } = UnitStates.None.ToString();
}
