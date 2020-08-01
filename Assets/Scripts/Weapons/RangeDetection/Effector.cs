using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Effector : MonoBehaviour
{
    public abstract void OnDamage(int damageAmount);
    public abstract void OnExplosion(float explosionValue);
    public abstract void OnPoision(int poisionAmount, float poisionDuration);
    public abstract void OnFreeze(float FreezeAmount, float TotalFreezeProbability);
    
}
