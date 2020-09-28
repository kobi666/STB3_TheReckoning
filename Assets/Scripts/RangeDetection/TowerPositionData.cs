using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class TowerPositionData {
    [SerializeField]
    float distance;
    public float Distance {
        get => distance;
        set {
            distance = value;
        }
    }

    public int DiscoveryRangeCycleNumber = 99;

    public float ProximityScore(float discoveryRange)
    {
        return distance + (DiscoveryRangeCycleNumber * discoveryRange);
    }

    public TowerSlotController TowerSlotController;
    
    [SerializeField]
    GameObject towerSlotGO;
    
    [SerializeField]
    Vector2 towerPosition;
    public GameObject TowerSlotGo {
        get => towerSlotGO;
        set {
            if (value == null) {
                //Debug.Log("Tower set to null");
            }
            towerSlotGO = value;
        }
    
    }
    public Vector2 TowerPosition {
        get => towerPosition;
        set {
            towerPosition = value;
        }
    }

    public int ClockWiseIndex;
    

    public int CounterClockwiseIndex;
    

    

    public TowerPositionData(GameObject _towerGO, float _distance) {
        TowerSlotGo = _towerGO;
        if (_towerGO != null) {
            TowerPosition = (Vector2)_towerGO.transform.position;
        }
        Distance = _distance;
    }

    public TowerPositionData(TowerSlotController tsc, float _distance)
    {
        if (tsc != null)
        {
            TowerSlotGo = tsc.gameObject;
            TowerSlotController = tsc;
        }

        Distance = _distance;
    }
    
    public TowerPositionData(GameObject _towerGO, float _distance, int _cycleNumber) {
        TowerSlotGo = _towerGO;
        DiscoveryRangeCycleNumber = _cycleNumber;
        if (_towerGO != null) {
            TowerPosition = (Vector2)_towerGO.transform.position;
        }
        Distance = _distance;
    }
    
    public TowerPositionData(GameObject _towerGO, TowerSlotController _towerSlotController, float _distance, int _cycleNumber) {
        TowerSlotGo = _towerGO;
        TowerSlotController = _towerSlotController;
        DiscoveryRangeCycleNumber = _cycleNumber;
        if (_towerGO != null) {
            TowerPosition = (Vector2)_towerGO.transform.position;
        }
        Distance = _distance;
    }
}