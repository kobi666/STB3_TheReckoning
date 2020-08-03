using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectPool<T> where T : Component,IActiveObject<T>
{

    Dictionary<string, T> pool = new Dictionary<string, T>();
    public Dictionary<string,T> Pool {
        get => pool;
    }

    public void AddObjectToActiveObjectPool(T t) {
        if (!Pool.ContainsKey(t.name)) {
            Pool.Add(t.name, t);
        }
        Debug.LogWarning(Pool.Count);
    }

    public void RemoveObjectFromPool(string objectName) {
        if (Pool.ContainsKey(objectName)) {
            Pool.Remove(objectName);
        }
    }

    
}
