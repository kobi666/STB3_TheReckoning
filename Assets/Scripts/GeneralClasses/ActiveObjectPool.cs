using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectPool<T> where T : Component,IActiveObject<T>
{

    public bool Contains(string objectName)
    {
        return Pool.ContainsKey(objectName);
    }
    
    Dictionary<string, T> pool = new Dictionary<string, T>();
    public Dictionary<string,T> Pool {
        get => pool;
        
    }

    public void AddObjectToActiveObjectPool(T t) {
        if (!Pool.ContainsKey(t.name)) {
            Pool.Add(t.name, t);
            onObjectAdded?.Invoke(t.name);
        }
        
    }

    public event Action<string> onObjectDisabled;
    public event Action<string> onObjectAdded;

    public void RemoveObjectFromPool(string objectName) {
        if (Pool.ContainsKey(objectName)) {
            Pool.Remove(objectName);
            onObjectDisabled?.Invoke(objectName);
        }
    }

    
}
