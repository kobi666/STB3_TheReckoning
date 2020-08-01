using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public interface IExplosionEffect
{
    bool CanBeAffectedByExplosion();
    void OnExplosion(int damage, float explosionPhysicsValue);
}

public interface IFreezable
{
    bool CanBeFrozen();
    void Freeze(float freezeAmount);
    void Freeze(float freezeAmount, bool permanent);
}

public interface IVanishable
{
    bool CanVanish();
    void Vanish();
}




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
    PoolObjectQueue<T> Pool {get;set;}
}

