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
    public void AddState(ObjectStateLegacy _unitstate) {
        if (_unitstate != null) {
        States.Add(_unitstate.stateName, _unitstate);
        }
    }
    public void RemoveState(ObjectStateLegacy _unitstate) {
        States.Remove(_unitstate.stateName);
    }
    public bool StateChangeLocked;
    
    

    public IEnumerator StateChangeTransition(ObjectStateLegacy newStateLegacy) {
        if (StateChangeLocked == false) {
            StateChangeLocked = true;
            yield return StartCoroutine(currentStateLegacy.InvokeExitStateFunctions());
            yield return StartCoroutine(newStateLegacy.InvokeEnterStateFunctions());
            Debug.Log("State Transition from " + currentStateLegacy.stateName + " to " + newStateLegacy.stateName + " Finished" );
            currentStateLegacy = newStateLegacy;
            if (newStateLegacy.isFinalState == true) {
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

    
    public Dictionary<string, ObjectStateLegacy> States = new Dictionary<string, ObjectStateLegacy>();
    public ObjectStateLegacy currentStateLegacy;

    public void InitilizeStateMachine(ObjectStateLegacy[] _states) {
        foreach(ObjectStateLegacy _unitstate in _states) {
            AddState(_unitstate);
        }
    }
}