﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class States {

}



[System.Serializable]
public class UnitState : IEquatable<UnitState> {

    public bool Equals(UnitState other)
   {
      if (other == null)
         return false;

      if (this.stateName == other.stateName)
         return true;
      else
         return false;
   }

   

   public override bool Equals(System.Object obj)
   {
      if (obj == null)
         return false;

      UnitState otherUnitState = obj as UnitState;
      if (otherUnitState == null)
         return false;
      else
         return Equals(otherUnitState);
   }

    public static bool operator == (UnitState thisUnitState, UnitState otherUnitState)
   {
      if (((object)thisUnitState) == null || ((object)otherUnitState) == null) {
         return System.Object.Equals(thisUnitState, otherUnitState);
      }

      return thisUnitState.Equals(otherUnitState);
   }

   public static bool operator != (UnitState thisUnitState, UnitState otherUnitState)
   {
      if (((object)thisUnitState) == null || ((object)otherUnitState) == null) {
         return ! System.Object.Equals(thisUnitState, otherUnitState);
      }

      return ! thisUnitState.Equals(otherUnitState);
   }

   



    // Override EnterState() and ExitState() in new classes

    public bool ExitEventIsNotEmpty() {
        if (OnExitState != null) {
            return true;
        }
        else {return false;}
    }
    public bool EnterEventIsNotEmpty() {
        if (OnEnterState != null) {
            return true;
        }
        else {
            return false;
        }
    }
    public UnitState(bool _isfinalState, string _name, MonoBehaviour _self ) {
        IsFinalState = _isFinalState;
        StateName = _name;
        exec = _self;
    }

    MonoBehaviour exec;
        
    

    //public event Action ExitStateActions;
    //public event Action EnterStateActions;
    public bool _isFinalState;
    public virtual bool IsFinalState { get => _isFinalState ; set {
        _isFinalState = value;
    }}
    [SerializeField]
    public string stateName;
    [SerializeField]
    public virtual string StateName {get => stateName ; set {
        stateName = value;
    }}

    public event Func<IEnumerator> OnEnterState;
    public event Func<IEnumerator> OnExitState;

    public IEnumerator InvokeEnterStateFunctions() {
        if (OnEnterState != null) {
        
        Delegate[] delegates = OnEnterState.GetInvocationList();
        for (int i = 0 ; i <  delegates.Length ; i++) {
                var x = delegates[i] as Func<IEnumerator>;
                yield return exec.StartCoroutine(x.Invoke());
            }
        }
        yield break;
    }

    public IEnumerator InvokeExitStateFunctions() {
        if (OnExitState != null) {
        Delegate[] delegates = OnExitState.GetInvocationList();
        for (int i = 0 ; i <  delegates.Length ; i++) {
                var x = delegates[i] as Func<IEnumerator>;
                yield return exec.StartCoroutine(x.Invoke());
            }
        }
        yield break;
    }

    


    
    

    
}

