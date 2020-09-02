using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlotController : MonoBehaviour
{
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
        SelectorTest2.instance.SelectedTowerSlot = this.gameObject;
        if (OldTowerObject != null) {
            Destroy(OldTowerObject);
        }
        SR.sprite = null;
    }

    private void Update() {
        
    }
    
    

    private void Start() {
        SR = GetComponent<SpriteRenderer>();
        //TowerSlotsByDirections8 = TowerUtils.CardinalTowersNoAnglesLoopover(gameObject, SelectorTest2.instance.TowerSlotsWithPositions, TowerUtils.Cardinal8);
        TowerSlotsByDirections8 = TowerUtils.CardinalTowersNoAnglesLoopOver(gameObject, SelectorTest2.instance.TowerSlotsWithPositions, TowerUtils.Cardinal8, 5);
        for(int i = 0 ; i < 8 ; i++) {
            TowersDebug[i].GO = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].TowerSlotGo;
            TowersDebug[i].Direction = TowerUtils.Cardinal8.directionNamesClockwise[i];
            TowersDebug[i].Position = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].TowerPosition;
            TowersDebug[i].Distance = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].Distance;
        }

        if (TowerObject == null) {
            PlaceNewTower(TowerArsenal.arsenal.EmptyTowerSlot.TowerPrefab);
        }

    }

}

