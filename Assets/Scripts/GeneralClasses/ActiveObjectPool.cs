using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectPool<T> where T : IhasGameObjectID,IActiveObject<T>
{

    public bool Contains(int GameObjectID)
    {
        return Pool.ContainsKey(GameObjectID);
    }
    
    Dictionary<int, T> pool = new Dictionary<int, T>();
    public Dictionary<int,T> Pool {
        get => pool;
        
    }

    public void AddObjectToActiveObjectPool(T t) {
        if (!Pool.ContainsKey(t.GameObjectID)) {
            Pool.Add(t.GameObjectID, t);
            onObjectAdded?.Invoke(t.GameObjectID);
        }
        
    }

    public event Action<int> onObjectDisabled;
    public event Action<int> onObjectAdded;

    public void RemoveObjectFromPool(int gameObjectID) {
        if (Pool.ContainsKey(gameObjectID)) {
            Pool.Remove(gameObjectID);
            onObjectDisabled?.Invoke(gameObjectID);
        }
    }

    
}
