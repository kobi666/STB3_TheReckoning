using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;

public class GenericTowerController : SerializedMonoBehaviour
{
    public TowerActionManager TowerActionManager;
    
    public TowerComponent mainTowerComponent;
    public bool debug;
    static string Tag = "Tower";
    public string TypeTag {get => Tag;}

    public TowerSlotAction NorthAction()
    {
        return null;
    }

    public bool NorthExecutionCondition(TowerComponent tc)
    {
        return true;
    }

    public TowerSlotAction EastAction()
    {
        return null;
    }

    public bool EastExecutionCondition(TowerComponent tc)
    {
        return true;
    }

    public TowerSlotAction SouthAction()
    {
        return null;
    }

    public bool SouthExecutionCondition(TowerComponent tc)
    {
        return true;
    }

    public TowerSlotAction WestAction()
    {
        return null;
    }

    public bool WestExecutionCondition(TowerComponent tc)
    {
        return true;
    }
    public Dictionary<Vector2, TowerPositionData> TowerSlotsByDirections8 = new Dictionary<Vector2, TowerPositionData>();
    public DebugTowerPositionData[] TowersDebug = new DebugTowerPositionData[8];
    
    public OrbitalGunsController orbitalGunsController;
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
        orbitalGunsController = GetComponentInChildren<OrbitalGunsController>();
    }

    protected void Awake()
    {
        TowerActionManager = GetComponent<TowerActionManager>() ?? null;
        gameObject.tag = TypeTag;
    }

    

    
}
