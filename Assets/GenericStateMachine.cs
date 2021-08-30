using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;


[Serializable]
public abstract class GenericStateMachine<T,TS> : MonoBehaviour where T : ObjectState<TS> where TS : IHasStateMachine
{
    public bool DebugStateMachine;
    public bool runLock = true;
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

    private bool OverrideStateChangeLock = false;
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
                try
                {
                    do
                    {
                        if (CanExecCurrentState)
                        {
                            CurrentState.InStateActions();
                            await UniTask.Yield();
                        }
                        else
                        {
                            break;
                        }
                    } while (CurrentState.RunningStateConditions());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                

                CurrentState.OnStateExitActions();
                
            }
            CurrentState.StateIsRunning = false;
            T nextState = NextStateResolver();
            /*if (DebugStateMachine)
            {
                Debug.LogError( name + " Current State : " + CurrentState.StateName +  " |||  Next State : " + nextState.StateName);
            }*/
            if (CurrentState.FinalState)
            {
                if (!OverrideStateChangeLock)
                {
                    return;
                }
            }
            
            
            ChangeState(nextState);
            
        }
        
        
    }

    public void RestartStateMachine()
    {
        
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

    private int changeStateCounter = 0;
    public void ChangeState(T state)
    {
        if (runLock) {
        changeStateCounter++;
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
//            Debug.LogWarning(Time.time + " Moving from " + CurrentState.StateName + " to " + PreviousState.StateName + 
  //                           " on unit " + name + " , change counter: " + changeStateCounter);
            if (InterruptState != null) {
                if (state.StateName == InterruptState.StateName)
                {
                    InterruptState = null;
                }
            }

            NextState = null;
            ExecuteCurrentState();
            CheckAndSwitchOffOverride();
        }
        
        else
        {
            // Need to fix same state loop on walking on path
            Debug.LogWarning("Same State Loop! " + CurrentState.StateName + " to " + state.StateName );
        }
        }
    }

    public void SetState(T nextState)
    {
        NextState = nextState;
        CanExecCurrentState = false;
    }

    public bool CheckAndSwitchOffOverride()
    {
        if (OverrideStateChangeLock)
        {
            OverrideStateChangeLock = false;
        }

        return OverrideStateChangeLock;
    }
    
    public async void SetStateForce(T nextState)
    {
        NextState = nextState;
        CanExecCurrentState = false;
        runLock = false;
        OverrideStateChangeLock = true;
        while (CurrentState.StateIsRunning)
        {
            await Task.Yield();
        }
        ChangeState(NextState);
        runLock = true;
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
