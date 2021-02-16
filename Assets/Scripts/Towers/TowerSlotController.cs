using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Serialization;

public class TowerSlotController : MonoBehaviour
{
    
    public event Action onPathDiscoveryEvent;
    public Vector2 lowestProximityPointFound;
    public void OnPathDiscoveryEvent()
    {
        onPathDiscoveryEvent?.Invoke();
    }

    public TowerActions TowerActions
    {
        get => childTower.TowerActionManager.Actions;
    }

    void TryToFindHighestProximityPathDiscoveryPoint()
    {
        float proximity = 0;
        (PathDiscoveryPoint,bool)[] points = PathDiscoveryTargetBank.Targets.Values.ToArray();
        foreach (var point in points)
        {
            if (proximity < point.Item1.Proximity)
            {
                lowestProximityPointFound = point.Item1.transform.position;
            }
        }
    }
    
    

    private PathDiscoveryTargetBank PathDiscoveryTargetBank;
    private RangeDetector RangeDetector;
    SpriteRenderer SR;
    public Dictionary<Vector2, TowerPositionData> TowerSlotsByDirections8 = new Dictionary<Vector2, TowerPositionData>();
    public DebugTowerPositionData[] TowersDebug = new DebugTowerPositionData[8];
    // Start is called before the first frame update
    public GameObject OldTowerObjectLegacy;
    [SerializeField]
    GameObject towerObject;
    public GameObject TowerObject {
        get => towerObject;
        set {
            towerObject = value;
            towerObjectControllerLegacy = value?.GetComponent<TowerControllerLegacy>() ?? null;
        }
    }

    private TowerController oldTower;
    private TowerController childTower;

    public TowerController ChildTower
    {
        get => childTower;
        set => childTower = value;
    }
    
    

    
    //legacy actions
    [FormerlySerializedAs("TowerObjectController")] public TowerControllerLegacy towerObjectControllerLegacy;
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

    public void PlaceNewTowerLegacy(GameObject TowerPrefab) {
        if (TowerObject != null) {
            OldTowerObjectLegacy = TowerObject;
            TowerObject = null;
        }
        GameObject newTower = Instantiate(TowerPrefab, transform.position, Quaternion.identity, gameObject.transform);
        TowerObject = newTower;
        TowerObject.name = (TowerPrefab.name + UnityEngine.Random.Range(10000, 99999).ToString());
        SelectorTest2.instance.SelectedTowerSlot = this;
        if (OldTowerObjectLegacy != null) {
            Destroy(OldTowerObjectLegacy);
        }
        SR.sprite = null;
    }
    
    public void PlaceNewTower(TowerController newTowerPrefab) {
        if (childTower != null) {
            oldTower = childTower;
            childTower = null;
        }

        TowerController newTower =
            Instantiate(newTowerPrefab, transform.position, Quaternion.identity, gameObject.transform);
        newTower.ParentSlotController = this;
        childTower = newTower;
        childTower.name = (newTowerPrefab.name + UnityEngine.Random.Range(10000, 99999).ToString());
        SelectorTest2.instance.SelectedTowerSlot = this;
        if (oldTower != null) {
            Destroy(oldTower);
        }
        SR.sprite = null;
    }

    public void CalculateAdjecentTowers()
    {
        TowerSlotParentManager.instance.OnGetTowerSlotsWithPositions();
        TowerSlotsByDirections8 = TowerUtils.CardinalTowersNoAnglesLoopOver(gameObject, TowerSlotParentManager.instance.V2_Tsc, TowerUtils.Cardinal8,20);
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
        ChildTower = GetComponentInChildren<TowerController>();
        /*TowerSlotsByDirections8 = TowerUtils.CardinalTowersNoAnglesLoopOver(gameObject, SelectorTest2.instance.towerSlotsWithPositions, TowerUtils.Cardinal8,6);*/
        OnTowerPositionCalculation();
        /*if (TowerObject == null) {
            PlaceNewTowerLegacy(TowerArsenal.arsenal.EmptyTowerSlot.TowerPrefab);
        }*/
        if (ChildTower == null)
        {
            PlaceNewTower(TowerArsenal.arsenal.EmptyTowerSlot.TowerPrefab);
        }
        ChildTower.OnInit(this);

    }

}

