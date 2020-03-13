using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTowerSlot : TowerController
{

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
