using System.Collections.Generic;
using UnityEngine;
using System;
using MyBox;


[System.Serializable]
public abstract class TowerControllerLegacy : MonoBehaviour,ITypeTag
{
    [ConditionalField("debug")] public TowerActionManager TowerActionManager;

    public TowerComponent mainTowerComponent;
    public bool debug;
    static string Tag = "Tower";
    public string TypeTag {get => Tag;}
    public abstract TowerSlotAction NorthAction();
    public abstract bool NorthExecutionCondition(TowerComponent tc);
    public abstract TowerSlotAction EastAction();
    public abstract bool EastExecutionCondition(TowerComponent tc);
    public abstract TowerSlotAction SouthAction();
    public abstract bool SouthExecutionCondition(TowerComponent tc);
    public abstract TowerSlotAction WestAction();
    public abstract bool WestExecutionCondition(TowerComponent tc);
    public Dictionary<Vector2, TowerPositionData> TowerSlotsByDirections8 = new Dictionary<Vector2, TowerPositionData>();
    public DebugTowerPositionData[] TowersDebug = new DebugTowerPositionData[8];

    
    [SerializeField]
    private TowerSlotController parentSlotController;
    public TowerSlotController ParentSlotController {
        get => parentSlotController;
        set {
            parentSlotController = value;
        }
    }
    public Sprite TowerSprite;
    public SpriteRenderer TowerSpriteRenderer;
    public TowerSlotActions TowerActions;
    public abstract void PostStart();
    protected void Start()
    {
        ParentSlotController = ParentSlotController ?? transform.parent.GetComponent<TowerSlotController>();
        TowerSlotsByDirections8 = TowerUtils.CardinalTowersNoAnglesLoop(ParentSlotController, SelectorTest2.instance.towerSlotsWithPositions, TowerUtils.Cardinal8);
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

    protected void Awake()
    {
        TowerActionManager = GetComponent<TowerActionManager>() ?? null;
        gameObject.tag = TypeTag;
        PostAwake();
    }

    public abstract void PostAwake();

    
}
