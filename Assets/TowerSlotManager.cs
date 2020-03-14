using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
                towerController = TowerSlot.GetComponent<TowerController>();
            }
        }
    }
    public TowerController towerController;

   
    public TowerSlotActions SlotActions {
        get => towerController.TowerActions;
    }

    public TowerSlotActions DefaultActions;
    
    public void ExecNorth() {
        SlotActions.ButtonNorth?.ExecuteFunction(TowerSlot, gameObject);
    }
    public event Action onExecNorth;
    public void OnExecNorth() {
        onExecNorth?.Invoke();
    }
    public void ExecEast() {
        SlotActions.ButtonEast?.ExecuteFunction(TowerSlot,gameObject);
    }
    public event Action onExecEast;
    public void OnExecEast() {
        onExecEast?.Invoke();
    }
    public void ExecSouth() {
        SlotActions.ButtonSouth?.ExecuteFunction(TowerSlot, gameObject);
    }
    public event Action onExecSouth;
    public void OnExecSouth() {
        onExecSouth?.Invoke();
    }
    public void ExecWest() {
        SlotActions.ButtonWest?.ExecuteFunction(TowerSlot, gameObject);
    }
    public event Action onExecWest;
    public void OnExecWest() {
        onExecWest?.Invoke();
    }


    
    
    void Start()
    {
        if (!TowerSlotOccupied) {
           TowerSlot = TowerUtils.PlaceTowerInSlotGO(TowerArsenal.arsenal.EmptyTowerSlot, gameObject);
        }
        onExecNorth += ExecNorth;
        onExecEast += ExecEast;
        onExecSouth += ExecSouth;
        onExecWest += ExecWest;

    }

    
    
}
