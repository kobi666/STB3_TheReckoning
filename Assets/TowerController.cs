using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public Dictionary<Vector2, TowerUtils.TowerPositionData> TowerSlotsByDirections8 = new Dictionary<Vector2, TowerUtils.TowerPositionData>();
    public DebugTowerPositionData[] TowersDebug = new DebugTowerPositionData[8];
    
    [SerializeField]
    TowerSlotController slotManager;
    public TowerSlotController SlotManager {
        get => slotManager;
        set {
            slotManager = value;
        }
    }
    public GameObject SlotManagerObject;
    public Sprite TowerSprite;
    public SpriteRenderer TowerSpriteRenderer;
    public TowerSlotActions TowerActions;
    // Start is called before the first frame update
    public virtual void PostStart() {

    }
    private void Start() {
        TowerSlotsByDirections8 = TowerUtils.CardinalTowersNoAnglesLoop(gameObject, SelectorTest2.instance.TowerSlotsWithPositions, TowerUtils.Cardinal8);
        for(int i = 0 ; i < 8 ; i++) {
            TowersDebug[i].GO = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].TowerSlotGo;
            TowersDebug[i].Direction = TowerUtils.Cardinal8.directionNamesClockwise[i];
            TowersDebug[i].Position = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].TowerPosition;
            TowersDebug[i].Distance = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].Distance;
        }
        TowerActions = TowerUtils.DefaultSlotActions;

        PostStart();
    }
}
