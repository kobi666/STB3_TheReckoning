using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine
{
    public event Action _onStateChange;
    public bool StateChangeLocked;
    public void OnStateChange() {
        if (_onStateChange != null) {
            _onStateChange.Invoke();
        }
        else {
            Debug.Log("No actions subscribed to \\_onStateChange");
        }
    }
    public Dictionary<string, State> States = new Dictionary<string, State>();
    public State CurrentState;
    public State FormerState;
   
}

public class state {
    public virtual void StateTransition() {
        Debug.Log("Transitioning..");
    }

    public virtual void SetState() {
        Debug.Log("State set");
    }
}

public class GeneralEnemyUnitStateMachine {

}

public class GeneralPlayerUnitStateMachine {

}
