using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public Dictionary<Vector2, TowerUtils.TowerPositionData> towersByDirections8 = new Dictionary<Vector2, TowerUtils.TowerPositionData>();
    public DebugTowerPositionData[] TowersDebug = new DebugTowerPositionData[8];
    
    [SerializeField]
    TowerSlotManager slotManager;
    public TowerSlotManager SlotManager {
        get => slotManager;
        set {
            slotManager = value;
        }
    }
    public Sprite TowerSprite;
    public SpriteRenderer TowerSpriteRenderer;
    public TowerSlotActions TowerActions;
    // Start is called before the first frame update
    public virtual void PostStart() {

    }
    private void Start() {
        towersByDirections8 = TowerUtils.CardinalTowersNoAnglesLoop(gameObject, SelectorTest2.instance.towersWithPositions, TowerUtils.Cardinal8);
        for(int i = 0 ; i < 8 ; i++) {
            TowersDebug[i].GO = towersByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].TowerGO;
            TowersDebug[i].Direction = TowerUtils.Cardinal8.directionNamesClockwise[i];
            TowersDebug[i].Position = towersByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].TowerPosition;
            TowersDebug[i].Distance = towersByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].Distance;
        }
        TowerActions = TowerUtils.DefaultSlotActions;

        PostStart();
    }
}
