using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OldStateMachine : MonoBehaviour
{

    public void SetState(string _newStateName) {
        if (StateChangeLocked != true) {
        StartCoroutine(StateChangeTransition(States[_newStateName]));
        }
        else {Debug.Log("State change lock is active");
        }
    }
    public void AddState(UnitState _unitstate) {
        if (_unitstate != null) {
        States.Add(_unitstate.stateName, _unitstate);
        }
    }
    public void RemoveState(UnitState _unitstate) {
        States.Remove(_unitstate.stateName);
    }
    public bool StateChangeLocked;
    
    

    public IEnumerator StateChangeTransition(UnitState _newState) {
        if (StateChangeLocked == false) {
            StateChangeLocked = true;
            yield return StartCoroutine(CurrentState.InvokeExitStateFunctions());
            yield return StartCoroutine(_newState.InvokeEnterStateFunctions());
            Debug.Log("State Transition from " + CurrentState.stateName + " to " + _newState.stateName + " Finished" );
            CurrentState = _newState;
            if (_newState.isFinalState == true) {
            StateChangeLocked = true;
            }
            else {
            StateChangeLocked = false;
            }
        }
        else {
            Debug.Log("State Change is Locked");
        }
        yield break;
    }

    
    public Dictionary<string, UnitState> States = new Dictionary<string, UnitState>();
    public UnitState CurrentState;

    public void InitilizeStateMachine(UnitState[] _states) {
        foreach(UnitState _unitstate in _states) {
            AddState(_unitstate);
        }
    }
}