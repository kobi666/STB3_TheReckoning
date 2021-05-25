using System;
using Sirenix.OdinInspector;

[Serializable]
public abstract class ObjectState<T> where T: IHasStateMachine
{
    [LabelText("Default State?")]
    public bool DefaultState;
    
    [LabelText("Initial State?")]
    public bool InitialState;
    public bool StateIsRunning = false;
    public bool FinalState;

    public abstract string AutomaticNextState { get; set; }

    [PropertyOrder(-1)][GUIColor(232 , 0 , 254)]
    public abstract string StateName { get; }

    public T SMObject;
    
    public event Action onStateEnterActions;

    public void OnStateEnterActions()
    {
        onStateEnterActions?.Invoke();
    }
    public event Action onStateExitActions;

    public void OnStateExitActions()
    {
        onStateExitActions?.Invoke();
    }
    public event Action inStateActions;

    public void InStateActions()
    {
        inStateActions?.Invoke();
    }

    public event Func<bool> stateExitConditions;
    public event Func<bool> stateEnterConditions;

    public bool StateEnterConditions()
    {
        if (RunningStateConditions()) {
        return stateEnterConditions?.Invoke() ?? true;
        }

        return false;
    }

    public bool RunningStateConditions()
    {
        return stateExitConditions?.Invoke() ?? false;
    }
    

    public abstract void InitState(T t);

    public void Init(T t)
    {
        SMObject = t;
        InitState(SMObject);
    }
}


public interface IHasStateMachine
{
    void OnEnterStateDefaultBehavior();
}





