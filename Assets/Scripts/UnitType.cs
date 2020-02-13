using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitType
{
    public static UnitState[] InitlizeStates(MonoBehaviour _monobehavior, StringAndBool[] stateTemplate) {
       UnitState[] _states = new UnitState[stateTemplate.Length];
       for(int i = 0 ; i <= stateTemplate.Length-1 ; i++) {
           if (stateTemplate[i].StateName != null) {
           _states[i] = new UnitState(stateTemplate[i].IsFinalState, stateTemplate[i].StateName, _monobehavior);
           }
           else {
               continue;
           }
       }
       return _states;
    }
    UnitState[] states;
    public UnitState[] States {get => states ; set {
        states = value;
    }}
    UnitState InitialState;

    public UnitType(StringAndBool[] stateTemplate, MonoBehaviour _monobehavior) {
        States = InitlizeStates(_monobehavior, stateTemplate);
    }

     
}
