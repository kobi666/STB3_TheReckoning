using System.Collections.Generic;
using UnityEngine;

public class PoolObjectQueue<T> where T : Component, IQueueable<T>
{

    public string PlaceholderName;
    int objectCounter = 0;
    int ObjectCounter {
        get {
            objectCounter += 1;
            return objectCounter;
        }
    }
    public Queue<T> ObjectQueue;
    public T ObjectPrefab;
    GameObject PoolParentObject;
    public T Get() {
        if (ObjectQueue.Count < 5) {
            AddObjectsToQueue(5);
        }
        T t = ObjectQueue.Dequeue();
        t.OnDequeue();
        t.gameObject.SetActive(true);
        return t;
    }

    public T GetInactive() {
        if (ObjectQueue.Count < 5) {
            AddObjectsToQueue(5);
        }
        T t = ObjectQueue.Dequeue();
        t.gameObject.SetActive(false);
        t.gameObject.SetActive(true);
        return t;
    }

    void AddObjectsToQueue(int numOfObjects) {
        for (int i = 0 ; i < numOfObjects ; i++) 
        {
                T PooledObject = GameObject.Instantiate(ObjectPrefab);
                PooledObject.transform.parent = PoolParentObject.transform;
                PooledObject.name = ObjectPrefab.name + "_" + ObjectCounter;
                PooledObject.QueuePool = this;
                PooledObject.gameObject.SetActive(false);
                PooledObject.OnEnqueue();
                ObjectQueue.Enqueue(PooledObject);
        }
    }

    public PoolObjectQueue(T objectPrefab, int initialNumberOfObjects, GameObject poolPlaceHolder) {
        PoolParentObject = poolPlaceHolder;
        PlaceholderName = poolPlaceHolder.name;
        ObjectPrefab = objectPrefab;
        ObjectQueue = new Queue<T>();
        AddObjectsToQueue(initialNumberOfObjects);
    }
}
