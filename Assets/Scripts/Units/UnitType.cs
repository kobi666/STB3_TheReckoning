using UnityEngine;

public class UnitType
{
    // public static UnitState[] InitlizeStates(MonoBehaviour _monobehavior, StringAndBool[] stateTemplate) {
    //    UnitState[] _states = new UnitState[stateTemplate.Length];
    //    for(int i = 0 ; i <= stateTemplate.Length-1 ; i++) {
    //        if (stateTemplate[i].StateName != null) {
    //        _states[i] = new UnitState(stateTemplate[i].IsFinalState, stateTemplate[i].StateName, _monobehavior);
    //        }
    //        else {
    //            continue;
    //        }
    //    }
    //    return _states;
    // }
    public NormalUnitStates states;
    public NormalUnitStates States {get => states ; set {
        states = value;
    }}

    public UnitType(MonoBehaviour _monobehavior, StateMachine _stateMachine) {
        States = new NormalUnitStates(_monobehavior);
        _stateMachine.currentStateLegacy = States.Default;
    }

     
}
