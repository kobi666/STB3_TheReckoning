using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmptyTowerSlot : TowerController
{
    public void PlaceTestTower1() {
        Debug.Log("1");
        ParentSlotController.PlaceNewTower(TowerArsenal.arsenal.TestTower1.TowerPrefab);
    }

    public override TowerSlotAction NorthAction() {
        TowerSlotAction tc = new TowerSlotAction(null,"place test tower 1",null,PlaceTestTower1);
        tc.ActionCost = 75;
        return tc;
    }

    public override TowerSlotAction EastAction() {
        TowerSlotAction tc = new TowerSlotAction(null,"",null,PlaceTestTower2);
        tc.ActionCost = 75;
        return tc;
    }

    public override TowerSlotAction SouthAction() {
        TowerSlotAction tc = new TowerSlotAction(null,"",null,PlaceTestTowerSouth);
        tc.ActionCost = 75;
        return tc;
    }

    public override TowerSlotAction WestAction() {
        TowerSlotAction tc = new TowerSlotAction(null,"",null,PlaceTestTower4);
        tc.ActionCost = 75;
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

    

    public void PlaceTestTower2() {
       
       ParentSlotController.PlaceNewTower(TowerArsenal.arsenal.TestTower2.TowerPrefab);
    }
    public void PlaceTestTower3() {
        
        ParentSlotController.PlaceNewTower(TowerArsenal.arsenal.EmptyTowerSlot.TowerPrefab);
    }
    public void PlaceTestTower4() {
        
        ParentSlotController.PlaceNewTower(TowerArsenal.arsenal.TestTower4.TowerPrefab);
    }

    public void PlaceTestTowerSouth()
    {
        ParentSlotController.PlaceNewTower(TowerArsenal.arsenal.TestTower3.TowerPrefab);
    }

    public override void PostAwake() {
        // TowerActions.ButtonNorth = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower1, TowerArsenal.arsenal.TestTower1.TowerSprite, PlaceTestTower1);
        
        // TowerActions.ButtonEast = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.EmptyTowerSlot, TowerArsenal.arsenal.TestTower2.TowerSprite, PlaceTestTower2);
        
        // TowerActions.ButtonSouth = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower3, TowerArsenal.arsenal.TestTower3.TowerSprite, PlaceTestTower3);
        
        // TowerActions.ButtonWest = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower4, TowerArsenal.arsenal.TestTower4.TowerSprite, PlaceTestTower4);
        
    }


    // Start is called before the first frame update
    public override void PostStart() {
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
