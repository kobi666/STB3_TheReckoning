using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;


[System.Serializable]
public abstract class Effect
{
    public abstract void Apply(Effectable ef);
}


[System.Serializable]
[HideLabel]
public class Damage : Effect
{
    [ShowInInspector]
    public DamageRange DamageRange = new DamageRange();
   
   
    
    public override void Apply(Effectable ef)
    {
        ef.ApplyDamage(DamageRange.RandomDamage());
    }
}

[System.Serializable]
public class DoSomethingWithPrefab : Effect
{
    [ShowInInspector] public GameObject somePrefab;
    
    public override void Apply(Effectable ef)
    {
        EffectableUnit efu = ef as EffectableUnit;
        if (efu.unitController != null)
        {
            if (somePrefab != null)
            {
                GameObject someGo = GameObject.Instantiate(somePrefab);
            }
        }
    }
}
