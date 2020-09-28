using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TowerSlotController : MonoBehaviour
{
    public event Action onPathDiscoveryEvent;
    public Vector2 lowestProximityPointFound;
    public void OnPathDiscoveryEvent()
    {
        onPathDiscoveryEvent?.Invoke();
    }

    void TryToFindHighestProximityPathDiscoveryPoint()
    {
        float proximity = 0;
        PathDiscoveryPoint[] points = PathDiscoveryTargetBank.Targets.Values.ToArray();
        foreach (PathDiscoveryPoint point in points)
        {
            if (proximity < point.Proximity)
            {
                lowestProximityPointFound = point.transform.position;
            }
        }
    }
    
    

    private PathDiscoveryTargetBank PathDiscoveryTargetBank;
    private RangeDetector RangeDetector;
    SpriteRenderer SR;
    public Dictionary<Vector2, TowerPositionData> TowerSlotsByDirections8 = new Dictionary<Vector2, TowerPositionData>();
    public DebugTowerPositionData[] TowersDebug = new DebugTowerPositionData[8];
    // Start is called before the first frame update
    public GameObject OldTowerObject;
    [SerializeField]
    GameObject towerObject;
    public GameObject TowerObject {
        get => towerObject;
        set {
            towerObject = value;
            TowerObjectController = value?.GetComponent<TowerController>() ?? null;
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
        //Debug.LogWarning(Actions.ButtonSouth.ActionDescription);
    }

    public void ExecWest() {
        Actions.ButtonWest?.ExecuteFunction();
    }

    public void PlaceNewTower(GameObject TowerPrefab) {
        if (TowerObject != null) {
            OldTowerObject = TowerObject;
            TowerObject = null;
        }
        GameObject newTower = Instantiate(TowerPrefab, transform.position, Quaternion.identity, gameObject.transform);
        TowerObject = newTower;
        TowerObject.name = (TowerPrefab.name + UnityEngine.Random.Range(10000, 99999).ToString());
        SelectorTest2.instance.SelectedTowerSlot = this;
        if (OldTowerObject != null) {
            Destroy(OldTowerObject);
        }
        SR.sprite = null;
    }

    public void CalculateAdjecentTowers()
    {
        TowerSlotsByDirections8 = TowerUtils.CardinalTowersNoAnglesLoopOver(gameObject, SelectorTest2.instance.TowerSlotsWithPositions, TowerUtils.Cardinal8,20);
        for(int i = 0 ; i < 8 ; i++) {
            TowersDebug[i].GO = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].TowerSlotGo;
            TowersDebug[i].Direction = TowerUtils.Cardinal8.directionNamesClockwise[i];
            TowersDebug[i].Position = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].TowerPosition;
            TowersDebug[i].Distance = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].Distance;
        }
    }

    public event Action onTowerPositionCalculation;

    public void OnTowerPositionCalculation()
    {
        onTowerPositionCalculation?.Invoke();
    }

    protected void Awake()
    {
        onTowerPositionCalculation += CalculateAdjecentTowers;
        RangeDetector = GetComponentInChildren<RangeDetector>();
        PathDiscoveryTargetBank = GetComponent<PathDiscoveryTargetBank>();
        onPathDiscoveryEvent += TryToFindHighestProximityPathDiscoveryPoint;
    }
    
    


    protected void Start() {
        SR = GetComponent<SpriteRenderer>();
        //TowerSlotsByDirections8 = TowerUtils.CardinalTowersNoAnglesLoopover(gameObject, SelectorTest2.instance.TowerSlotsWithPositions, TowerUtils.Cardinal8);
        OnTowerPositionCalculation();
        if (TowerObject == null) {
            PlaceNewTower(TowerArsenal.arsenal.EmptyTowerSlot.TowerPrefab);
        }

    }

}

