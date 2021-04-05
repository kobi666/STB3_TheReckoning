using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;
using UnityEngine.Events;





[System.Serializable]
public class BeamRenderingFunctionLegacy : UnityEvent<LineRenderer, Vector2, Vector2,float>
{
    
}

[System.Serializable]
public class BeamOscilationMinMax
{
    public float min;
    public float max;
}




[System.Serializable]
public class TowerComponentWeaponData
{
    
}

[System.Serializable]
public class TowerComponentBeamData
{
    public float BeamDuration;
    public float CooldownTime;
    [SerializeField]
    public BeamOscilationMinMax OscliatingBeamWidthMinMax;
    public SingleAnimationObject OnHitBeamAnimation;
    public Material BeamMaterial;
    public float beamWidth;
    public bool IsOscillating;
    public float BeamOsciliationSpeed;
    public float BeamMovementSpeed;
}


[System.Serializable]
public class TowerComponentOrbitalControllerData
{
    public int maxNumberOfOrbitals;
    public float distanceFromRotatorBase;
    public float orbitingSpeed;
    public float rotationSpeed;
    public Transform orbitBase;
    public int initialNumberOfOrbitals;
    private int _numOfOrbitals;
    public int NumOfOrbitals {
        get => _numOfOrbitals;
        set => _numOfOrbitals = value;
    }
}

[System.Serializable]
public class TowerComponentUnitSpawnerData
{
    public int maxUnits;
    public float playerUnitSpawnTime;
}

[System.Serializable]
public class TowerComponentAOEData
{
    
}






[System.Serializable]
public class PoolObjectQueueDictionariesContainer<T> where T : Component,IQueueable<T> {
    public Dictionary<string, Dictionary<string,PoolObjectQueue<T>>> AllDictionaries = new Dictionary<string, Dictionary<string, PoolObjectQueue<T>>>();
    public (string,T) GetQueueableType(T t) {

        Type componentType = t.QueueableType;
        if (componentType == null) {
            Debug.LogWarning("Queueable Type is NULL!!");
            return (null,null);
        }
        string TypeName = componentType.FullName;
        return (TypeName,componentType as T);
    }

    public Dictionary<string,PoolObjectQueue<T>> AddOrGetNewTypeDictionary(T t) {
        (string,T) tempT = GetQueueableType(t);
        if (tempT.Item2 != null) {
            if (!AllDictionaries.ContainsKey(tempT.Item1)) {
                AllDictionaries.Add(tempT.Item1, new Dictionary<string, PoolObjectQueue<T>>());
                return AllDictionaries[tempT.Item1];
            }
            else if (AllDictionaries.ContainsKey(tempT.Item1)) {
                return AllDictionaries[tempT.Item1];
            }
        }
        Debug.LogWarning("Could not add type " + tempT.Item1 + " To all Dictionaries for some reason..");
        return null;
    }
}

public class QueueableType<T> where T: Component,IQueueable<T> {
    public string QueueableTypeName;
    public Type Queueable_Type;

    public QueueableType (T t) {
        QueueableTypeName = t.QueueableType.FullName;
        Queueable_Type = t.QueueableType;
    }
}



[System.Serializable]
public class RoomLimits {
    [SerializeField]
    public float MaxX;
    [SerializeField]
    public float MaxY;
    [SerializeField]
    public float MinX;
    [SerializeField]
    public float MinY;
    public RoomLimits (float maxX,float maxY, float minX, float minY) {
        MaxX = maxX;
        MaxY = maxY;
        MinX = minX;
        MinY = minY;
    }
}

[System.Serializable]
public class RoomBorders {
    [SerializeField]
    public RoomBorder Top;
    [SerializeField]
    public RoomBorder Bottom;
    [SerializeField]
    public RoomBorder Left;
    [SerializeField]
    public RoomBorder Right;
}



/// <summary>
/// I Seriously have to fix this shit, I'm generating a class on every Target Identifcation when I have everything I need in an object pool.
/// probably change this into a struct once i figure out how GC works on those. Jesus this is going to be a painful refactor....
/// </summary>
[System.Serializable]
public class TargetUnit 
{
    [SerializeField] private Transform targetTransform;
    public GenericUnitController GenericUnitController;
    public Transform TargetTransform
    {
        get => targetTransform;
        set => targetTransform = value;
    }

    /*public Vector2 Position
    {
        
    }*/
    
    [SerializeField]
    Effectable effectable = null;
    public Effectable Effectable {
        get => effectable;
        set {
            effectable = value;
        }
    }
    [SerializeField]
    public string name {get => Effectable?.name ?? null;}
    public Transform transform {get => Effectable?.transform ?? null;
    }
    public float Proximity {get => GenericUnitController.PathWalker.ProximityToPathEnd;}

    public TargetUnit(int targetGameObjectID) {
        try
        {
            GenericUnitController = GameObjectPool.Instance.ActiveUnits[targetGameObjectID];
            
            Effectable = GameObjectPool.Instance.ActiveEffectables.Pool[targetGameObjectID];
            TargetTransform = transform;
        }
        catch(Exception e) {
            Debug.LogWarning(e.Message);
        }
        if (GenericUnitController == null || Effectable == null) {
            Effectable = null;
            TargetTransform = null;
            GenericUnitController = null;
            Debug.LogWarning("Target did not have either an effectable or a unit controller, null value returned.");
        }
    }

    
}





[System.Serializable]
public class TowerItemLegacy {
    [SerializeField]
    public GameObject TowerPrefab;
    [SerializeField]
    public int Price;
    
    TowerSlotActions actions;
    public TowerSlotActions Actions {
        get {
            if (actions != null) {
                return actions;
            }
            else {
                actions = TowerPrefab.GetComponent<TowerControllerLegacy>().TowerActions;
                return actions;
            }
        }
    }

    Sprite towerSprite;
    public Sprite TowerSprite {
        get {
            if (towerSprite != null) {
                return towerSprite;
            }
            else {
                towerSprite = TowerPrefab.GetComponent<SpriteRenderer>().sprite;
                return towerSprite;
            }
        }
    }
}

[System.Serializable]
public class TowerSlotActions  {
    [SerializeField]
    public TowerSlotAction ButtonNorth;
    [SerializeField]
    public TowerSlotAction ButtonEast;
    [SerializeField]
    public TowerSlotAction ButtonSouth;
    [SerializeField]
    public TowerSlotAction ButtonWest;

    public TowerSlotActions(TowerSlotAction north, TowerSlotAction east, TowerSlotAction south, TowerSlotAction west) {
        ButtonWest = west;
        ButtonEast = east;
        ButtonNorth = north;
        ButtonSouth = south;
    }
}

[System.Serializable]
public class TowerActionLegacy : UnityEvent<TowerControllerLegacy>
{
    [FormerlySerializedAs("TowerController")] public TowerControllerLegacy towerControllerLegacy;
    public GameObject UpgradePrefab;
    public string ActionName;
}

[System.Serializable]
public class TowerSlotAction {
    public TowerComponent towerComponent = null;
    [SerializeField]
    public string ActionDescription = "Empty Action";
    [SerializeField]
    public Sprite ButtonSprite = null;
    [SerializeField]
    public event Action ActionFunctions = null;

    [FormerlySerializedAs("MainAction")] public TowerActionLegacy mainActionLegacy = new TowerActionLegacy();
    public int ActionCost;
    public Predicate<TowerComponent> ExecutionCondition = null;
    public TowerSlotAction(TowerComponent towerComponent, string actionDescription, Sprite buttonSprite, Action action) {
        this.towerComponent = towerComponent;
        ActionDescription = actionDescription;
        ButtonSprite = buttonSprite;
        ActionFunctions += action;
    }

    //for empty functions
    public TowerSlotAction() {

    }
    public void ExecuteFunction() {
        if (ExecutionCondition != null && ExecutionCondition.Invoke(towerComponent) == true ) {
        ActionFunctions?.Invoke();
        }
    }
}




[System.Serializable]
public class TowerPositionQuery {
    public Vector2 ThisTower;
    public Vector2 TargetTower;
    public float BaseDiscoveryRange;
    public float DiscoveryRange;

    public TowerPositionQuery(Vector2 thisTower, Vector2 targetTower, float assistingFloat) {
        ThisTower = thisTower;
        TargetTower = targetTower;
        BaseDiscoveryRange = assistingFloat;
    }

    public TowerPositionQuery(Vector2 thisTower, Vector2 targetTower, float assistingFloat1, float assistingFloat2) {
        ThisTower = thisTower;
        TargetTower = targetTower;
        BaseDiscoveryRange = assistingFloat1;
        DiscoveryRange = assistingFloat2;
    }

}

[System.Serializable]
public struct DebugTowerPositionData {
    public string Direction;
    public GameObject GO;
    public Vector2 Position;
    public float Distance;
    public GameObject ClockWiseTower;
    public GameObject CounterClockWiseTower;
}

[System.Serializable]
public struct StringAndBool {
    [SerializeField]
    public string StateName;
    [SerializeField]
    public bool IsFinalState;
    public StringAndBool(string s, bool b) {
        StateName = s;
        IsFinalState = b;
    }
    public StringAndBool(string s) {
        StateName = s;
        IsFinalState = false;
    }
}




public class PlayerUnitStateMachine {
    
}

[System.Serializable]
public class  DamageRange {
    [SerializeField]
    public int min;
    [SerializeField]
    public int max;

    public int RandomDamage() {
        return UnityEngine.Random.Range(min, max);
    }
}

[System.Serializable]
public class BeamData
{
    
}

[System.Serializable]
public class ProjectileData {

    [SerializeField]
    public float Speed;

    [SerializeField]
    public float EffectRadius;
    [SerializeField]
    public int Damage;

    [SerializeField]
    public float ArcValue;
    [SerializeField]
    public Effectable EffectableTarget;
    [SerializeField]
    public Vector2 TargetPosition;

    public Vector2 OriginPosition;
    [SerializeField]
    public EffectableTargetBank TargetBank;
    [SerializeField]
    public RangeDetector RangeDetector;
}

public class Queueable : Component, IQueueable<Queueable> {
    public Type QueueableType {get;set;}

    public PoolObjectQueue<Queueable> QueuePool {get;set;}

    public void OnEnqueue() {}
    public void OnDequeue()
    {
        
    }
}

[System.Serializable]
public class TargetWithProximityToEndOfSpline {

    GameObject go;
    public GameObject GO {
        get => go ;
        set {
            if (value != null) {
            Proximity =  value.GetComponent<BezierSolution.UnitWalker>()?.ProximityToEndOfSplineFunc() ?? 999.0f;
            go = value;
            if (Proximity == 999.0f) {
                go = null;
            }
            }
        }
    }
    public float Proximity = 999.0f;
    public TargetWithProximityToEndOfSpline(GameObject _go) {
        GO = _go;
    }
}

    [System.Serializable]
    public class Damage_Type {
        public static List<string> DamageTypes = new List<string> (new string[] {"normal", "special", "fire", "poison", "armorShred"});
        
        string _damageType;
        public string DamageType {get => _damageType; set {
            _damageType = value;
        } }
        public Damage_Type(string dt) {
            if (DamageTypes.Contains(dt)) {
                DamageType = dt;
            }
            else {
                Debug.Log("null damage type");
                if (DamageTypes.Contains("normal")) {
                    Debug.Log("list contains normal");
                }
            }
        }
    }

    



