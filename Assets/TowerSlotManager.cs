using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerSlotManager : MonoBehaviour
{
    TowerSlotManager self;
    [SerializeField]
    GameObject OldTowerSlot;
    [SerializeField]
    GameObject towerSlot = null;
    public GameObject TowerSlot {
        get => towerSlot;
        set {
            towerSlot = value;
        }
    }
    [SerializeField]
    TowerController towerSlotController;
    public TowerController TowerSlotController {
        get => towerSlotController;
        set {
            towerSlotController = value;
        }
    }

    public TowerSlotActions Actions {
        get => TowerSlotController.TowerActions;
    }
    
    public void ExecNorth() {
        Actions.ButtonNorth?.ExecuteFunction();
    }
    public event Action onExecNorth;
    public void OnExecNorth() {
        onExecNorth?.Invoke();
    }
    public void ExecEast() {
        Actions.ButtonEast?.ExecuteFunction();
    }
    public event Action onExecEast;
    public void OnExecEast() {
        onExecEast?.Invoke();
    }
    public void ExecSouth() {
        Actions.ButtonSouth?.ExecuteFunction();
    }
    public event Action onExecSouth;
    public void OnExecSouth() {
        onExecSouth?.Invoke();
    }
    public void ExecWest() {
        Actions.ButtonWest?.ExecuteFunction();
    }
    public event Action onExecWest;
    public void OnExecWest() {
        onExecWest?.Invoke();
    }

    event Action onReplaceTower;
    public void OnReplaceTower() {
        onReplaceTower?.Invoke();
    }

    public void TowerReplacementSequence(GameObject newTowerPrefab) {
        OldTowerSlot = TowerSlot;
        TowerSlot = Instantiate(newTowerPrefab, transform.position,Quaternion.identity, gameObject.transform);
        TowerSlot.name = (towerSlot.name + UnityEngine.Random.Range(10000, 99999).ToString());
        TowerSlotController = TowerSlot.GetComponent<TowerController>();
        Destroy(OldTowerSlot);
        OnReplaceTower();
    }


    void DBG() {
        Debug.Log("TestSSTT");
    }
    
    void Start()
    {
        self = gameObject.GetComponent<TowerSlotManager>();
        onExecNorth += ExecNorth;
        onExecEast += ExecEast;
        onExecSouth += ExecSouth;
        onExecWest += ExecWest;
        onReplaceTower += DBG;
        GetComponent<SpriteRenderer>().sprite = null;
        
        if (TowerSlot == null) {
           TowerReplacementSequence(TowerArsenal.arsenal.EmptyTowerSlot.TowerPrefab);
        }
        
    }

    
    
}
