using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;


[Serializable]
public abstract class GenericStateMachine<T,TS> : MonoBehaviour where T : ObjectState<TS> where TS : IHasStateMachine
{
    public bool DebugStateMachine;
    private bool runLock = true;
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
    [HideInInspector] 
    public T InterruptState;

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
                Debug.Log("Empty state entered or empty state name on : " + gameObject.name);
                return;
            }
            SMObject.OnEnterStateDefaultBehavior();
            CurrentState.OnStateEnterActions();
            CurrentState.StateIsRunning = true;
            if (CurrentState.StateEnterConditions() && runLock)
            {
                CanExecCurrentState = true;
                NextState = null;
                while ( /*runLock &&*/ (CurrentState.RunningStateConditions()))
                {
                    if (CanExecCurrentState)
                    {
                        CurrentState.InStateActions();
                        await Task.Delay(1);
                    }
                    else
                    {
                        break;
                    }
                }

                CurrentState.OnStateExitActions();
                
            }
            CurrentState.StateIsRunning = false;
            T nextState = NextStateResolver();
            if (DebugStateMachine)
            {
                Debug.LogError( name + " Current State : " + CurrentState.StateName +  " |||  Next State : " + nextState.StateName);
            }
            if (!CurrentState.FinalState) {
            ChangeState(nextState);
            }
        }
        
        
    }

    public T NextStateResolver()
    {
        if (InterruptState == null)
        {
            if (CurrentState.AutomaticNextState != EmptyStateName) {
                if (States.ContainsKey(CurrentState.AutomaticNextState))
                {
                    return States[CurrentState.AutomaticNextState];
                }
            }
        }

        if (InterruptState != null)
        {
            if (InterruptState.StateName == EmptyStateName)
            {
                if (CurrentState.AutomaticNextState != EmptyStateName) {
                    if (States.ContainsKey(CurrentState.AutomaticNextState))
                    {
                        if (CurrentState.AutomaticNextState != EmptyStateName)
                        {
                            //Debug.LogWarning("");
                            T t = States[CurrentState.AutomaticNextState];
                            return t;
                        }
                    }
                }
            }
        }

        if (InterruptState != null)
        {
            if (InterruptState.StateName != EmptyStateName) {
                if (States.ContainsKey(InterruptState?.StateName))
                {
                    return InterruptState;
                }
            }
        }
        return DefaultState;
    }

    public event Action onStateChange;

    public void OnStateChange()
    {
        onStateChange?.Invoke();
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
            OnStateChange();
            Debug.Log("state name" + CurrentState.StateName + " : " + name);
            ExecuteCurrentState();
            if (InterruptState != null) {
                if (state.StateName == InterruptState.StateName)
                {
                    InterruptState = null;
                }
            }

            NextState = null;
        }
        else
        {
            throw new Exception("Same State Loop! " + CurrentState.StateName);
        }
    }

    public void SetState(T nextState)
    {
        NextState = nextState;
        CanExecCurrentState = false;
    }

    protected void OnDisable()
    {
        runLock = false;
    }

    public void SetState(string stateName)
    {
        if (States.ContainsKey(stateName))
        {
            if (stateName != EmptyStateName)
            {
                InterruptState = States[stateName];
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

        InterruptState = null;
        
        if (InitOnStartup) ExecuteCurrentState();
    }
    
    
    
    
    
    
}
