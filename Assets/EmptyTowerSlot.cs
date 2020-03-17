using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTowerSlot : TowerController
{
    public void PlaceTestTower1() {
        Debug.Log("1");
        SlotManager.PlaceTower(TowerArsenal.arsenal.TestTower1.TowerPrefab);
       
    }

    public void PlaceTestTower2() {
       Debug.Log("2");
       SlotManager.PlaceTower(TowerArsenal.arsenal.TestTower2.TowerPrefab);
    }
    public void PlaceTestTower3() {
        Debug.Log("3");
        SlotManager.PlaceTower(TowerArsenal.arsenal.TestTower3.TowerPrefab);
    }
    public void PlaceTestTower4() {
        Debug.Log("4");
        SlotManager.PlaceTower(TowerArsenal.arsenal.TestTower4.TowerPrefab);
    }

    private void Awake() {
        TowerActions.ButtonNorth = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower1, TowerArsenal.arsenal.TestTower1.TowerSprite);
        TowerActions.ButtonNorth.ActionFunctions += PlaceTestTower1;
        TowerActions.ButtonEast = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower2, TowerArsenal.arsenal.TestTower2.TowerSprite);
        TowerActions.ButtonEast.ActionFunctions += PlaceTestTower2;
        TowerActions.ButtonSouth = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower3, TowerArsenal.arsenal.TestTower3.TowerSprite);
        TowerActions.ButtonSouth.ActionFunctions += PlaceTestTower3;
        TowerActions.ButtonWest = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower4, TowerArsenal.arsenal.TestTower4.TowerSprite);
        TowerActions.ButtonWest.ActionFunctions += PlaceTestTower4;
    }


    // Start is called before the first frame update
    public override void PostStart() {
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
