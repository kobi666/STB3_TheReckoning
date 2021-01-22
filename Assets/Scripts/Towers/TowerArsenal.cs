using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerArsenal : MonoBehaviour
{
    [HideInInspector]
    public TowerItemLegacy EmptyTowerSlotLegacy;
    [HideInInspector]
    public TowerItemLegacy TestTower1;
    [HideInInspector]
    public TowerItemLegacy TestTower2;
    [HideInInspector]
    public TowerItemLegacy TestTower3;
    [HideInInspector]
    public TowerItemLegacy TestTower4;

    public TowerPlacementItem EmptyTowerSlot;
    public TowerPlacementItem BaseTowerNorth;
    public TowerPlacementItem BaseTowerEast;
    public TowerPlacementItem BaseTowerSouth;
    public TowerPlacementItem BaseTowerWest;
    
        

    public static TowerArsenal arsenal;
    private void Awake() {
        arsenal = this;
    }
    private void Start() {
        
    }
}
