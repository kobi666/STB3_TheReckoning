using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UnitStateMachine : GenericStateMachine<UnitState,GenericUnitController>
{
    public override string EmptyStateName { get; } = UnitStates.None.ToString();
}
