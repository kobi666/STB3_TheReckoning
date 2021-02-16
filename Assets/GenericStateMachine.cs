using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public abstract class GenericStateMachine<T,TS> : MonoBehaviour where T : ObjectState<TS> where TS : IHasStateMachine
{
    public T CurrentState;
    public T NextState;
    public T PreviousState;
    public T DefaultState;
    public TS SMObject;
    
    public abstract string EmptyStateName { get; }

    public bool CanExecCurrentState = true;
    public async void ExecuteCurrentState()
    {
        if (CurrentState.StateIsRunning == false)
        {
            CurrentState.OnStateEnterActions();
            CurrentState.StateIsRunning = true;
            if (CurrentState.StateEnterConditions())
                CanExecCurrentState = true;
                while (CurrentState?.StateExitConditions() ?? false)
                {
                    if (CanExecCurrentState)
                    {
                        CurrentState.InStateActions();
                        await Task.Yield();
                    }
                    else
                    {
                        break;
                    }
                }
            CurrentState.OnStateExitActions();
            CurrentState.StateIsRunning = false;
        }
        ChangeState(NextStateResolver());
    }

    public T NextStateResolver()
    {
        if (NextState != null)
        {
            if (NextState.StateName != EmptyStateName) {
            return NextState;
            }
        }
        
        return DefaultState;
    }

    private void ChangeState(T State)
    {
        PreviousState = CurrentState;
        CurrentState = State;
        ExecuteCurrentState();
    }

    public void SetState(T nextState)
    {
        NextState = nextState;
        CanExecCurrentState = false;
    }

    public void Init(TS smObject, List<T> states)
    {
        SMObject = smObject;
        foreach (var state in states)
        {
            if (state.DefaultState)
            {
                DefaultState = state;
            }

            if (state.InitialState)
            {
                CurrentState = state;
            }
        }
        ExecuteCurrentState();
    }
    
    
    
    
    
    
}
