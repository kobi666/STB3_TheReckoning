public class UnitStateMachine : GenericStateMachine<UnitState,GenericUnitController>
{
    public override string EmptyStateName { get; } = UnitStates.None.ToString();
}
