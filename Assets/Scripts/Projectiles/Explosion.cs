using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Explosion : AreaOfEffectController
{
    void TestExplosion(Effectable[] effectables) {
        for (int i = 0 ; i < effectables.Length ; i++) {
           effectables[i].ApplyDamage(500); 
        }
    }

    
    public override void PostStart() {
        onApplyEffect += TestExplosion;
        ACT.instance.TB1 += OnEffectTrigger;
    }


}
