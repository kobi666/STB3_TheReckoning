using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestOrbitalGunsController : OrbitalGunsController
{
    // Start is called before the first frame update
    
    public override event Action<EnemyUnitController> onEnemyEnteredRange;
    public override void OnEnemyEnteredRange(EnemyUnitController ec) {
        onEnemyEnteredRange?.Invoke(ec);
    }
    public override WeaponRotator Rotator {get; set;}
    public override List<OrbitalWeapon> OrbitalGuns {get; set;}


    public override void PostAwake() {
        
        Rotator = GetComponent<WeaponRotator>() ?? null;
        Rotator.parentTowerComponent = this;
        OrbitalGuns =  new List<OrbitalWeapon>();
        TestInput.instance.onW += AddOrbitalGun;
    }
}
