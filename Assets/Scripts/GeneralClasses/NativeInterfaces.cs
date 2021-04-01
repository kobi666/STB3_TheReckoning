using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ITypeTag
{
    string TypeTag {get;}
}

public interface ITargetBank<T>
{
    SortedList<string,T> Targets {get;}
}

public interface IOrbital<T> {

    Transform OrbitalTransform {get;}
    string OrbitalName {get;}
    bool ShouldRotate {get;set;}
    Transform OrbitBase{get;set;}
    float OrbitingSpeed {get;set;}
    float DistanceFromOrbitalBase {get;set;}
    GameObject referenceGOforRotation {get;set;}

    float AngleForOrbit {get;set;}

    void ReStartOrbiting();
    void StopOrbiting();
        
    IEnumerator OrbitingCoroutine {get;set;}
}

public interface IQueueable<T> where T : Component, IQueueable<T> {
    Type QueueableType {get;set;}
    PoolObjectQueue<T> QueuePool {get;set;}
    void OnEnqueue();
    void OnDequeue();
}

public interface ITargetable {
    bool ExternalTargetableLock {get;set;}
    bool IsTargetable();
    event Action<bool> onTargetableStateChange;
}

public interface IActiveObject<T> where T : IhasGameObjectID,IActiveObject<T> {
    ActiveObjectPool<T> ActivePool {get;set;}
}

public interface IInGameObject<T> {
    void Register(T t);
    void UnRegister(T t);
}

