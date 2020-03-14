using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTowerSlot : TowerController
{
    public void PlaceTestTower1(GameObject targetSlot, GameObject TowerSlotParent) {
       TowerUtils.PlaceTowerInSlot(TowerArsenal.arsenal.TestTower1, targetSlot, TowerSlotParent);
    }

    public void PlaceTestTower2(GameObject targetSlot, GameObject TowerSlotParent) {
        TowerUtils.PlaceTowerInSlot(TowerArsenal.arsenal.TestTower2, targetSlot, TowerSlotParent);
    }
    public void PlaceTestTower3(GameObject targetSlot, GameObject TowerSlotParent) {
        TowerUtils.PlaceTowerInSlot(TowerArsenal.arsenal.TestTower3, targetSlot, TowerSlotParent);
    }
    public void PlaceTestTower4(GameObject targetSlot, GameObject TowerSlotParent) {
        TowerUtils.PlaceTowerInSlot(TowerArsenal.arsenal.TestTower4, targetSlot, TowerSlotParent);
    }
    // Start is called before the first frame update
    public override void PostStart() {
        TowerActions.ButtonNorth = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower1, TowerArsenal.arsenal.TestTower1.TowerSprite);
        TowerActions.ButtonNorth.ActionFunctions += PlaceTestTower1;
        TowerActions.ButtonEast = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower2, TowerArsenal.arsenal.TestTower2.TowerSprite);
        TowerActions.ButtonEast.ActionFunctions += PlaceTestTower2;
        TowerActions.ButtonSouth = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower3, TowerArsenal.arsenal.TestTower3.TowerSprite);
        TowerActions.ButtonSouth.ActionFunctions += PlaceTestTower3;
        TowerActions.ButtonWest = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower4, TowerArsenal.arsenal.TestTower4.TowerSprite);
        TowerActions.ButtonWest.ActionFunctions += PlaceTestTower4;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
