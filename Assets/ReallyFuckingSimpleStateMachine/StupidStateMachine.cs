using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StupidStateMachine : MonoBehaviour
{

    public bool StateMachineIsRunning;

    public async void StartStatefulBehavior()
    {
        StateMachineIsRunning = true;

        StateMachineIsRunning = false;
    }
    
}



