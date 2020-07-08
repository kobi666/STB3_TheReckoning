using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnerTowerController : TowerController
{
    // Start is called before the first frame update
    public override void PostAwake() {

    }

    public override void PostStart() {
        
    }

    public override TowerSlotAction NorthAction() {
        TowerSlotAction tc = new TowerSlotAction();
        return tc;
    }

    public override TowerSlotAction EastAction() {
        TowerSlotAction tc = new TowerSlotAction();
        return tc;
    }

    public override TowerSlotAction SouthAction() {
        TowerSlotAction tc = new TowerSlotAction();
        return tc;
    }

    public override TowerSlotAction WestAction() {
        TowerSlotAction tc = new TowerSlotAction();
        return tc;
    }


    public override bool NorthExecutionCondition(TowerComponent tc) {
        return true;
    }
    public override bool EastExecutionCondition(TowerComponent tc) {
        return true;
    }

    public override bool SouthExecutionCondition(TowerComponent tc) {
        return true;
    }
    public override bool WestExecutionCondition(TowerComponent tc) {
        return true;
    }

}
