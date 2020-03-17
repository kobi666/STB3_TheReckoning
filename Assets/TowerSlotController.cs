﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlotController : MonoBehaviour
{
    public Dictionary<Vector2, TowerUtils.TowerPositionData> TowerSlotsByDirections8 = new Dictionary<Vector2, TowerUtils.TowerPositionData>();
    public DebugTowerPositionData[] TowersDebug = new DebugTowerPositionData[8];
    // Start is called before the first frame update
    public GameObject OldTowerObject;
    [SerializeField]
    GameObject towerObject;
    public GameObject TowerObject {
        get => towerObject;
        set {
            towerObject = value;
        }
    }

    public TowerController TowerObjectController;
    public TowerSlotActions Actions;

    public void ExecNorth() {
        Actions.ButtonNorth?.ExecuteFunction();
    }

    public void ExecEast() {
        Actions.ButtonEast?.ExecuteFunction();
    }

    public void ExecSouth() {
        Actions.ButtonSouth?.ExecuteFunction();
    }

    public void ExecWest() {
        Actions.ButtonWest?.ExecuteFunction();
    }

    public void PlaceTower(GameObject TowerPrefab) {
        OldTowerObject = TowerObject;
        TowerObject = Instantiate(TowerPrefab, transform.position, Quaternion.identity, gameObject.transform);
        TowerObject.name = (TowerPrefab.name + UnityEngine.Random.Range(10000, 99999).ToString());
        TowerObjectController = TowerObject.GetComponent<TowerController>();
        TowerObjectController.SlotManagerObject = gameObject;
        TowerObjectController.SlotManager = TowerObjectController.SlotManagerObject.GetComponent<TowerSlotController>();
        Actions = TowerObjectController.TowerActions;

        if (OldTowerObject != null) {
            Destroy(OldTowerObject);
        }
    }
   

    private void Start() {
        TowerSlotsByDirections8 = TowerUtils.CardinalTowersNoAnglesLoop(gameObject, SelectorTest2.instance.TowerSlotsWithPositions, TowerUtils.Cardinal8);
        for(int i = 0 ; i < 8 ; i++) {
            TowersDebug[i].GO = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].TowerSlotGo;
            TowersDebug[i].Direction = TowerUtils.Cardinal8.directionNamesClockwise[i];
            TowersDebug[i].Position = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].TowerPosition;
            TowersDebug[i].Distance = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].Distance;
        }

        if (TowerObject == null) {
            PlaceTower(TowerArsenal.arsenal.EmptyTowerSlot.TowerPrefab);
        }
    }

}

