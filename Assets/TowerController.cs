using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class TowerController : MonoBehaviour
{
    public abstract TowerSlotAction NorthAction();
    public abstract bool NorthExecutionCondition(TowerComponent tc);
    public abstract TowerSlotAction EastAction();
    public abstract bool EastExecutionCondition(TowerComponent tc);
    public abstract TowerSlotAction SouthAction();
    public abstract bool SouthExecutionCondition(TowerComponent tc);
    public abstract TowerSlotAction WestAction();
    public abstract bool WestExecutionCondition(TowerComponent tc);
    public Dictionary<Vector2, TowerUtils.TowerPositionData> TowerSlotsByDirections8 = new Dictionary<Vector2, TowerUtils.TowerPositionData>();
    public DebugTowerPositionData[] TowersDebug = new DebugTowerPositionData[8];
    
    [SerializeField]
    TowerSlotController slotController;
    public TowerSlotController SlotController {
        get => slotController;
        set {
            slotController = value;
        }
    }
    public Sprite TowerSprite;
    public SpriteRenderer TowerSpriteRenderer;
    public TowerSlotActions TowerActions;
    public abstract void PostStart();
    private void Start() {
        TowerSlotsByDirections8 = TowerUtils.CardinalTowersNoAnglesLoop(gameObject, SelectorTest2.instance.TowerSlotsWithPositions, TowerUtils.Cardinal8);
        for(int i = 0 ; i < 8 ; i++) {
            TowersDebug[i].GO = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].TowerSlotGo;
            TowersDebug[i].Direction = TowerUtils.Cardinal8.directionNamesClockwise[i];
            TowersDebug[i].Position = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].TowerPosition;
            TowersDebug[i].Distance = TowerSlotsByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].Distance;
        }

        TowerActions = new TowerSlotActions(NorthAction(),EastAction(),SouthAction(),WestAction());
        TowerActions.ButtonNorth.ExecutionCondition = new Predicate<TowerComponent>(NorthExecutionCondition);
        TowerActions.ButtonEast.ExecutionCondition = new Predicate<TowerComponent>(EastExecutionCondition);
        TowerActions.ButtonSouth.ExecutionCondition = new Predicate<TowerComponent>(SouthExecutionCondition);
        TowerActions.ButtonWest.ExecutionCondition = new Predicate<TowerComponent>(WestExecutionCondition);
        PostStart();
    }

    private void Awake() {
        PostAwake();
    }

    public abstract void PostAwake();
}
