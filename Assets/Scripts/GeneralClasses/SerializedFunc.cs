using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Reflection;
using System;
using Object = System.Object;


[System.Serializable]
public class SerializedFunc<T> 
{
    [ShowInInspector]
    public Func<T>[] Funcs = new Func<T>[0];

  
}

[Serializable]
public abstract class SerializedFunc<T1,T2> 
{
    [ShowInInspector]
    public Func<T1,T2>[] Funcs = new Func<T1, T2>[0];
}

[Serializable]
public  class SerializedFunc<T1,T2,T3> 
{
    [ShowInInspector]
    public Func<T1,T2,T3>[] Funcs = new Func<T1, T2, T3>[0];
}

[Serializable]
public  class SerializedFunc<T1,T2,T3,T4> 
{
    [ShowInInspector]
    public Func<T1,T2,T3,T4>[] Funcs = new Func<T1, T2, T3, T4>[0];
 
}

[System.Serializable]
public class SerializedBoolFunc2T : SerializedFunc<Object,Object, bool> 
    
{
    public bool Invoke(Object t1, Object t2)
    {
        bool tempT = false;
        if (Funcs != null) {
            foreach (var func in Funcs)
            {
                tempT = func.Invoke(t1, t2);
            }
        }

        return tempT;

    }
}

[System.Serializable]
public class SerializedBoolFunc1T : SerializedFunc<Component, bool>
{
    public bool Invoke(Component t1)
    {
        bool tempT = false;
        if (Funcs != null) {
            foreach (var func in Funcs)
            {
                tempT = func.Invoke(t1);
            }
        }
        return tempT;

    }
}
