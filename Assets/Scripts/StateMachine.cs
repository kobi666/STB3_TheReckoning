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
        else {
  //          Debug.Log("State change lock is active");
        }
    }
   
    public bool StateChangeLocked;
    
    public bool ConditionToChangeToNewState (UnitState _cur, UnitState _new) {
        if (CurrentState.ExitEventIsNotEmpty()) {
            if (_new.EnterEventIsNotEmpty()) {
                return true;
            }
            else {
                Debug.Log(_new.stateName + " Enter Sequence Is Empty");
                return false;
            }
        }
        else {
            Debug.Log(_cur.stateName + " Exit Sequence Is Empty");
            return false;
        }
    }

    public IEnumerator StateChangeTransition(UnitState _newState) {
        if (StateChangeLocked == false && ConditionToChangeToNewState(CurrentState, _newState) == true) {
            StateChangeLocked = true;
            yield return StartCoroutine(CurrentState.InvokeExitStateFunctions());
            yield return StartCoroutine(_newState.InvokeEnterStateFunctions());
//            Debug.Log("State Transition from " + CurrentState.stateName + " to " + _newState.stateName + " Finished" );
            CurrentState = _newState;
            if (_newState._isFinalState == true) {
            StateChangeLocked = true;
            }
            else {
            StateChangeLocked = false;
            }
        }
        else {
            Debug.Log("State change lock is : " + StateChangeLocked.ToString());
        }
        yield break;
    }
    

    
    
    public UnitState CurrentState;

    
    }
