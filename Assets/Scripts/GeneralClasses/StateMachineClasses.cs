using System.Collections;
using UnityEngine;
using System;

public abstract class States {

}



[System.Serializable]
public class ObjectStateLegacy : IEquatable<ObjectStateLegacy> {

    public AnimationClip StateAnimation = null;
    public Color textColor;
    public bool StateTransitionInterrupted;

    public bool Equals(ObjectStateLegacy other)
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

      ObjectStateLegacy otherUnitStateLegacy = obj as ObjectStateLegacy;
      if (otherUnitStateLegacy == null)
         return false;
      else
         return Equals(otherUnitStateLegacy);
   }

    public static bool operator == (ObjectStateLegacy thisUnitStateLegacy, ObjectStateLegacy otherUnitStateLegacy)
   {
      if (((object)thisUnitStateLegacy) == null || ((object)otherUnitStateLegacy) == null) {
         return System.Object.Equals(thisUnitStateLegacy, otherUnitStateLegacy);
      }

      return thisUnitStateLegacy.Equals(otherUnitStateLegacy);
   }

   public static bool operator != (ObjectStateLegacy thisUnitStateLegacy, ObjectStateLegacy otherUnitStateLegacy)
   {
      if (((object)thisUnitStateLegacy) == null || ((object)otherUnitStateLegacy) == null) {
         return ! System.Object.Equals(thisUnitStateLegacy, otherUnitStateLegacy);
      }

      return ! thisUnitStateLegacy.Equals(otherUnitStateLegacy);
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
    public ObjectStateLegacy(bool _isfinalState, string _name, MonoBehaviour _self, Color color) {
        IsFinalState = _isfinalState;
        StateName = _name;
        exec = _self;
        textColor = color; 
    }

    MonoBehaviour exec;
        
    

    //public event Action ExitStateActions;
    //public event Action EnterStateActions;
    public bool isFinalState;
    public bool IsFinalState { get => isFinalState ; set {
        isFinalState = value;
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
        if (exec != null) {
        if (OnEnterState != null) {
        
        Delegate[] delegates = OnEnterState.GetInvocationList();
        for (int i = 0 ; i <  delegates.Length ; i++) {
                if (StateTransitionInterrupted == false) {
                    var x = delegates[i] as Func<IEnumerator>;
                    yield return exec.StartCoroutine(x.Invoke());
                }
                else {
                    break;
                }
            }
        }
        if (StateTransitionInterrupted == true) {
            StateTransitionInterrupted = false;
        }
        }
        yield break;
    }

    public IEnumerator InvokeExitStateFunctions() {
        if (exec != null) {
            if (OnExitState != null) {
            Delegate[] delegates = OnExitState.GetInvocationList();
            for (int i = 0 ; i <  delegates.Length ; i++) {
                    if (StateTransitionInterrupted == false) {
                        var x = delegates[i] as Func<IEnumerator>;
                        yield return exec.StartCoroutine(x.Invoke());
                    }
                    else {
                        break;
                    }
                }
            }
            if (StateTransitionInterrupted == true) {
                StateTransitionInterrupted = false;
            }
            }
        yield break;
    }

    

   
    


    
    

    
}


