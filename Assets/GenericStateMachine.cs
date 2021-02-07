using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericStateMachine<T> : MonoBehaviour
{
    public ObjectState<T> CurrentState;
    public ObjectState<T> NextState;
    public ObjectState<T> PreviousState;
    public ObjectState<T> DefaultState;


    public void ExecuteState()
    {
        
    }
    
    
    
    
}
