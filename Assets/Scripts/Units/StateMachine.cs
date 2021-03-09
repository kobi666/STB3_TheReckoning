using System.Collections;
using UnityEngine;
using System;

public class StateMachine : MonoBehaviour
{
    public IEnumerator MovementCoroutine;
    public IEnumerator AttackCoroutine;
    public event Action onStateChange;
    public void OnStateChange() {
        onStateChange?.Invoke();
    }
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
    public void SetState(ObjectStateLegacy newStateLegacy) {
        
            if (StateChangeLocked != true) {
                StartCoroutine(StateChangeTransition(newStateLegacy));
                }
            else {
                if (currentStateLegacy.IsFinalState == true) 
                {
                    Debug.Log(currentStateLegacy.stateName + " is a final state");
                    }
                else 
                {
                Debug.Log("Could not change state from " + currentStateLegacy.stateName + " to " + newStateLegacy.stateName + " because STATE CHANGE LOCK is active");
                    }
                }
            
        }

    public void SetState(ObjectStateLegacy newStateLegacy, bool interrupt) {
        if (StateChangeLocked != true) {
        currentStateLegacy.StateTransitionInterrupted = true;
        StartCoroutine(StateChangeTransition(newStateLegacy));
        }
        else {
            if (currentStateLegacy.IsFinalState == true) {
                Debug.Log(currentStateLegacy.stateName + " is a final state");
            }
            else {
            Debug.Log("Could not change state from " + currentStateLegacy.stateName + " to " + newStateLegacy.stateName + " because STATE CHANGE LOCK is active");
            }
        }
    }
   
   
    public bool StateChangeLocked;
    
    public bool ConditionToChangeToNewState (ObjectStateLegacy _cur, ObjectStateLegacy _new) {
        if (currentStateLegacy.ExitEventIsNotEmpty()) {
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

    public IEnumerator StateChangeTransition(ObjectStateLegacy newStateLegacy) {
        if (StateChangeLocked == false && ConditionToChangeToNewState(currentStateLegacy, newStateLegacy) == true) {
            OnStateChange();
            InitilizeAttackCoroutine(null);
            InitilizeMovementCoroutine(null);
            currentStateLegacy = newStateLegacy;
            StateInTransition = true;
//            Debug.Log("Changed to State :" + _newState.stateName);
            yield return StartCoroutine(currentStateLegacy.InvokeExitStateFunctions());
            yield return StartCoroutine(newStateLegacy.InvokeEnterStateFunctions());
//            Debug.Log("State Transition from " + CurrentState.stateName + " to " + _newState.stateName + " Finished" );
            StateInTransition = false;
            if (newStateLegacy.isFinalState == true) {
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
    

    
    
    public ObjectStateLegacy currentStateLegacy;

    
    }
