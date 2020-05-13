using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine : MonoBehaviour
{
    public IEnumerator MovementCoroutine;
    public IEnumerator AttackCoroutine;
    public void InitilizeMovementCoroutine(IEnumerator mc) {
        if (MovementCoroutine != null) {
            StopCoroutine(MovementCoroutine);
            
        }
        MovementCoroutine = mc;
    }
    public void InitilizeAttackCoroutine(IEnumerator ac) {
        if (AttackCoroutine != null) {
            StopCoroutine(AttackCoroutine); 
        }
        AttackCoroutine = ac;
    }
    public bool StateInTransition = false;
    public void SetState(UnitState _newState) {
        if (CurrentState.IsFinalState != true) {
            if (StateChangeLocked != true) {
                StartCoroutine(StateChangeTransition(_newState));
                }
            else {
                if (CurrentState.IsFinalState == true) 
                {
                    Debug.Log(CurrentState.stateName + " is a final state");
                    }
                else 
                {
                Debug.Log("Could not change state from " + CurrentState.stateName + " to " + _newState.stateName + " because STATE CHANGE LOCK is active");
                    }
                }
            }
        }

    public void SetState(UnitState _newState, bool interrupt) {
        if (StateChangeLocked != true) {
        CurrentState.StateTransitionInterrupted = true;
        StartCoroutine(StateChangeTransition(_newState));
        }
        else {
            if (CurrentState.IsFinalState == true) {
                Debug.Log(CurrentState.stateName + " is a final state");
            }
            else {
            Debug.Log("Could not change state from " + CurrentState.stateName + " to " + _newState.stateName + " because STATE CHANGE LOCK is active");
            }
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
            InitilizeAttackCoroutine(null);
            InitilizeMovementCoroutine(null);
            CurrentState = _newState;
            StateInTransition = true;
//            Debug.Log("Changed to State :" + _newState.stateName);
            yield return StartCoroutine(CurrentState.InvokeExitStateFunctions());
            yield return StartCoroutine(_newState.InvokeEnterStateFunctions());
//            Debug.Log("State Transition from " + CurrentState.stateName + " to " + _newState.stateName + " Finished" );
            StateInTransition = false;
            if (_newState.isFinalState == true) {
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

//     public IEnumerator StateChangeTransition(UnitState _newState) {
//         if (StateChangeLocked == false && ConditionToChangeToNewState(CurrentState, _newState) == true) {
//             StateChangeLocked = true;
//             CurrentState = _newState;
// //            Debug.Log("Changed to State :" + _newState.stateName);
//             yield return StartCoroutine(CurrentState.InvokeExitStateFunctions());
//             yield return StartCoroutine(_newState.InvokeEnterStateFunctions());
// //            Debug.Log("State Transition from " + CurrentState.stateName + " to " + _newState.stateName + " Finished" );
            
//             if (_newState._isFinalState == true) {
//             StateChangeLocked = true;
//             }
//             else {
//             StateChangeLocked = false;
//             }
//         }
//         else {
//             Debug.Log("State change lock is : " + StateChangeLocked.ToString());
//         }
//         yield break;
//     }
    

    
    
    public UnitState CurrentState;

    
    }
