using System;
using UnityEngine;

[Serializable]
public class UnityPredicate<T>
{
   
    [SerializeField]
    private Component target;

    // will be set by property drawer to verify the proper method is used
    // (e.g: validate return type + arg).
        
    [SerializeField]
    private string methodName;

    public bool Evaluate(T obj)
    {
        if (target == null)
        {
            return false;
        }
            
        // Use reflection to get the MethodInfo and invoke it on target
        var mi = target.GetType().GetMethod(methodName);

        var result = mi.Invoke(target, new object[] { obj });

        return (bool) result;
    }

    public static implicit operator Predicate<T>(UnityPredicate<T> unityPred)
    {
        return unityPred.Evaluate;
    }
}