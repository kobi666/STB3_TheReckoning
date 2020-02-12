using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine : MonoBehaviour
{
    public void SetState(UnitState _newState) {
        if (StateChangeLocked != true) {
        StartCoroutine(StateChangeTransition(_newState));
        }
        else {Debug.Log("State change lock is active");
        }
    }
    public void AddState(UnitState _unitstate) {
        States.Add(_unitstate.stateName, _unitstate);
    }
    public void RemoveState(UnitState _unitstate) {
        States.Remove(_unitstate.stateName);
    }
    public event Action _onStateChange;
    public bool StateChangeLocked;
    public void OnStateChange(UnitState _newState) {
        if (_onStateChange != null) {
            _onStateChange.Invoke();
        }
        else {
            Debug.Log("No actions subscribed to \\_onStateChange");
        }
    }
    

    public IEnumerator StateChangeTransition(UnitState _newState) {
        if (StateChangeLocked == false) {
            StateChangeLocked = true;
            yield return StartCoroutine(CurrentState.InvokeExitStateFunctions());
            yield return StartCoroutine(_newState.InvokeEnterStateFunctions());
            Debug.Log("State Transition from " + CurrentState.stateName + " to " + _newState.stateName + " Finished" );
            CurrentState = _newState;
            if (_newState._isFinalState == true) {
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
}