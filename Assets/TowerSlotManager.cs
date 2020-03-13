using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlotManager : MonoBehaviour
{

    public bool TowerSlotOccupied {
        get {
            if (TowerSlot != null) {
                return true;
            }
            else {
                return false;
            }
        }
    }
    GameObject towerSlot;
    public GameObject TowerSlot {
        get => towerSlot;
        set {
            if (value == null) {
                towerSlot = value;
                towerController = null;
                
            }
            else {
                towerSlot = value;
                towerController = towerSlot.GetComponent<TowerController>();
                
            }
        }
    }
    public TowerController towerController;

    public TowerSlotActions SlotActions {
        get {
            if (towerController == null) {
                return TowerUtils.DefaultSlotActions;
            }
            else {
                return towerController.TowerActions;
            }
        }
    }

    public TowerSlotActions DefaultActions;
    

    public void PlaceTowerInSlot(TowerItem tower, GameObject towerSlot, GameObject self) {
        if (TowerSlotOccupied != true) 
        {
            towerSlot = Instantiate(tower.TowerPrefab,self.transform.position, Quaternion.identity, self.transform);
        }
    }

    public void PlaceTestTower1() {
        PlaceTowerInSlot(TowerArsenal.arsenal.TestTower1, TowerSlot, gameObject);
    }

    public void PlaceTestTower2() {
        PlaceTowerInSlot(TowerArsenal.arsenal.TestTower2, TowerSlot, gameObject);
    }
    public void PlaceTestTower3() {
        PlaceTowerInSlot(TowerArsenal.arsenal.TestTower3, TowerSlot, gameObject);
    }
    public void PlaceTestTower4() {
        PlaceTowerInSlot(TowerArsenal.arsenal.TestTower4, TowerSlot, gameObject);
    }

    // Start is called before the first frame update
    
    
    void Start()
    {
        DefaultActions.ButtonNorth = new TowerSlotAction("Place " + TowerArsenal.arsenal.TestTower1.TowerPrefab.name, TowerArsenal.arsenal.TestTower1.TowerPrefab.GetComponent<SpriteRenderer>().sprite, PlaceTestTower1);
        DefaultActions.ButtonEast = new TowerSlotAction("Place " + TowerArsenal.arsenal.TestTower2.TowerPrefab.name, TowerArsenal.arsenal.TestTower2.TowerPrefab.GetComponent<SpriteRenderer>().sprite, PlaceTestTower2);
        DefaultActions.ButtonSouth = new TowerSlotAction("Place " + TowerArsenal.arsenal.TestTower3.TowerPrefab.name, TowerArsenal.arsenal.TestTower3.TowerPrefab.GetComponent<SpriteRenderer>().sprite, PlaceTestTower3);
        DefaultActions.ButtonNorth = new TowerSlotAction("Place " + TowerArsenal.arsenal.TestTower4.TowerPrefab.name, TowerArsenal.arsenal.TestTower4.TowerPrefab.GetComponent<SpriteRenderer>().sprite, PlaceTestTower4);
    }

    
    
}
