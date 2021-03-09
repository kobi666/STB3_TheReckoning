﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class GenericStateMachine<T,TS> : MonoBehaviour where T : ObjectState<TS> where TS : IHasStateMachine
{
    public bool InitOnStartup;
    
    [ShowInInspector]
    private string currentState
    {
        get => CurrentState.StateName;
    }

    [ShowInInspector]
    private string defaultState
    {
        get => DefaultState.StateName;
    }

    [ShowInInspector]
    private bool stateIsRunning
    {
        get => CurrentState.StateIsRunning;
    }
    
    
    [HideInInspector]
    public T CurrentState;
    [HideInInspector]
    public T NextState;
    [HideInInspector]
    public T PreviousState;
    [HideInInspector]
    public T DefaultState;

    public T InitialState;
    public TS SMObject;
    private Dictionary<string,T> States = new Dictionary<string,T>();
    
    public abstract string EmptyStateName { get; }

    public bool CanExecCurrentState = true;

    private int counter;
    private float debugCounter;
    public async void ExecuteCurrentState()
    {
        if (CurrentState.StateIsRunning == false)
        {
            if (CurrentState.StateName == EmptyStateName)
            {
                Debug.LogWarning(new Exception("Empty state entered or empty state name on : " + gameObject.name));
                return;
            }
            SMObject.OnEnterStateDefaultBehavior();
            CurrentState.OnStateEnterActions();
            CurrentState.StateIsRunning = true;
            if (CurrentState.StateEnterConditions())
                CanExecCurrentState = true;
                NextState = null;
                while (CurrentState?.RunningStateConditions() ?? false)
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
            T nextState = NextStateResolver();
            ChangeState(nextState);
        }
        
        
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

    public void ChangeState(T state)
    {
        PreviousState = CurrentState;
        CurrentState = state;
        if (PreviousState.StateName == CurrentState.StateName)
        {
            counter += 1;
        }
        else
        {
            counter = 0;
        }

        if (counter < 3)
        {
            ExecuteCurrentState();
        }
        else
        {
            throw new Exception("Same State Loop!");
        }
    }

    public void SetState(T nextState)
    {
        NextState = nextState;
        CanExecCurrentState = false;
    }

    public void SetState(string stateName)
    {
        if (States.ContainsKey(stateName))
        {
            if (stateName != EmptyStateName)
            {
                NextState = States[stateName];
                CanExecCurrentState = false;
            }
        }
    }

    public void Init(TS smObject, List<T> states)
    {
        SMObject = smObject;
        
        foreach (var state in states)
        {
            state.Init(SMObject);
            if (state.DefaultState)
            {
                DefaultState = state;
            }

            if (state.InitialState)
            {
                CurrentState = state;
                InitialState = state;
            }

            try
            {
                States.Add(state.StateName, state);
            }
            catch (Exception e)
            {
                throw new Exception("Same State Key exists twice");
            }
        }
        
        if (InitOnStartup) ExecuteCurrentState();
    }
    
    
    
    
    
    
}
