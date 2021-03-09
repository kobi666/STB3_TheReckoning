using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerUnitAfterEffects : MonoBehaviour
{
    public static PlayerUnitAfterEffects instance;

    SortedList<string, float> bonusMeleeAttackRateMultipliers = new SortedList<String, float>();
    public float BonusMeleeAttackRateMultiplier {
        get {
            float marm = 0.0f;
            foreach (var item in bonusMeleeAttackRateMultipliers)
            {
                marm += item.Value;
            }
            return marm;
        }
    }
    
    public float MeleeAttackRateWithMultiplier(float baseAttackRate) {
            return baseAttackRate * (1.0f + BonusMeleeAttackRateMultiplier);
        }
        
            
        
    




    private void Awake() {
        instance = this;
    }
    
}
