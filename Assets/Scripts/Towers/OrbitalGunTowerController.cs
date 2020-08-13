using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalGunTowerController : TowerController
{
    OrbitalGunsControllerGeneric orbitalGunsController;
    
    public override void PostAwake() {
        orbitalGunsController = GetComponentInChildren<OrbitalGunsControllerGeneric>() ?? null;
    }
    public override void PostStart() {
        
    }

    public override bool NorthExecutionCondition(TowerComponent tc) {
        if (orbitalGunsController.OrbitalGuns.Count < tc.Data.MaxNumberOfOrbitals) {
            return true;
        }
        return false;
    }

    public override bool EastExecutionCondition(TowerComponent tc) {
        return false;
    }
    public override bool SouthExecutionCondition(TowerComponent tc) {
        return false;
    }

    public override bool WestExecutionCondition(TowerComponent tc) {
        return false;
    }

    public override TowerSlotAction NorthAction() {
        TowerSlotAction tc = new TowerSlotAction(orbitalGunsController,"Add Orbital Gun",null,orbitalGunsController.AddOrbitalGun);
        return tc;
    }

    public override TowerSlotAction EastAction() => new TowerSlotAction();
    public override TowerSlotAction SouthAction() => new TowerSlotAction();
    public override TowerSlotAction WestAction() => new TowerSlotAction();


}
