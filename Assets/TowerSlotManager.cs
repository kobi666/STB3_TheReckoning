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
    GameObject towerSlot = null;
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
    

    
    
    void Start()
    {
        if (!TowerSlotOccupied) {
           TowerSlot = TowerUtils.PlaceTowerInSlotGO(TowerArsenal.arsenal.EmptyTowerSlot, gameObject);
        }
    }

    
    
}
