using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyBox;
using UnityEngine.Serialization;
using UnityEngine.Events;



[System.Serializable]
public class BeamRenderingFunction : UnityEvent<LineRenderer, Vector2, Vector2,float>
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
public class TowerComponentProjectileData
{
    public float projectileSpeed;
    public Projectile projectilePrefab;
    public float aoeProjectileRadius;
}


[System.Serializable]
public class TowerComponentOrbitalControllerData
{
    public int maxNumberOfOrbitals;
    public float distanceFromRotatorBase;
    public float orbitingSpeed;
    public float rotationSpeed;
    public Transform orbitBase;
    private int _numOfOrbitals;
    public int NumOfOrbitals {
        get => _numOfOrbitals;
        set => _numOfOrbitals = value;
    }
    public OrbitalWeaponGeneric orbitalGunPrefab;
}

[System.Serializable]
public class TowerComponentUnitSpawnerData
{
    public PlayerUnitController playerUnitPrefab;
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


public class Effect<T>  {
    Buffs<T> buffs;
    Nerfs<T> nerfs;

    public void AddBuff(string buffName, T buff) {
        if (buffs.BuffStack.ContainsKey(buffName)) {
        buffs.BuffStack.Add(buffName, buff);
        }
    }
    public void AddNerf(string nerfName, T nerf) {
        if (nerfs.NerfStack.ContainsKey(nerfName)) {
        nerfs.NerfStack.Add(nerfName, nerf);
        }
    }

    public void RemoveBuff(string buffName) {
        if (buffs.BuffStack.ContainsKey(buffName)) {
        buffs.BuffStack.Remove(buffName);
        }
    }

    public void RemoveNerf(string nerfName) {
        if (nerfs.NerfStack.ContainsKey(nerfName)) {
        nerfs.NerfStack.Remove(nerfName);
        }
    }

    public Effect(T t) {
        buffs = new Buffs<T>(t);
        nerfs = new Nerfs<T>(t);
    }
}

public interface IEffect<T> {
    
    void GetValue(T t);
    void GetValues(List<T> t);
    T ApplyEffect(T t);
}

[System.Serializable]
public class Buffs<T> {

    public Dictionary<string, T> BuffStack;
    public Buffs(T t) {
        BuffStack = new Dictionary<string, T>();
    }
}

public class Nerfs<T> {

    public Dictionary<string, T> NerfStack;
    public Nerfs(T t) {
        NerfStack = new Dictionary<string, T>();
    }
}

[System.Serializable]
public class TargetUnit
{

    [SerializeField] private Transform targetTransform;
    
    public Transform TargetTransform
    {
        get => targetTransform;
        set => targetTransform = value;
    }

    UnitController unitController = null;
    public UnitController UnitController {
        get => unitController;
        set {
            unitController = value;
        }
    }
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
    public float Proximity {get => UnitController.Proximity;}

    public TargetUnit(string targetName) {
        try {
            UnitController = GameObjectPool.Instance.ActiveUnitPool.Pool[targetName];
            Effectable = GameObjectPool.Instance.ActiveEffectables.Pool[targetName];
            TargetTransform = transform;
        }
        catch(Exception e) {
            Debug.LogWarning(e.Message);
        }
        if (UnitController == null || Effectable == null) {
            UnitController = null;
            Effectable = null;
            TargetTransform = null;
            Debug.LogWarning("Target did not have either an effectable or a unit controller, null value returned.");
        }
    }

}

[System.Serializable]
public class TowerItem {
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
                actions = TowerPrefab.GetComponent<TowerController>().TowerActions;
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
public class TowerAction : UnityEvent
{
    public bool IsUpgradeFunction = true;
    [ConditionalField("IsUpgradeFunction")]
    public GameObject UpgradePrefab;

    public TowerController someTowerController;
}

[System.Serializable]
public class TowerSlotAction {
    public TowerComponent TowerComponent = null;
    [SerializeField]
    public string ActionDescription = "Empty Action";
    [SerializeField]
    public Sprite ButtonSprite = null;
    [SerializeField]
    public event Action ActionFunctions = null;

    public TowerAction MainAction = new TowerAction();
    public int ActionCost;
    public Predicate<TowerComponent> ExecutionCondition = null;
    public TowerSlotAction(TowerComponent towerComponent, string actionDescription, Sprite buttonSprite, Action action) {
        TowerComponent = towerComponent;
        ActionDescription = actionDescription;
        ButtonSprite = buttonSprite;
        ActionFunctions += action;
    }

    //for empty functions
    public TowerSlotAction() {

    }
    public void ExecuteFunction() {
        if (ExecutionCondition != null && ExecutionCondition.Invoke(TowerComponent) == true )
        ActionFunctions?.Invoke();
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



[System.Serializable]
public class Subwave {
    public SubWavePackage Package;
    [SerializeField]
    public float StartupPauseSeconds;
    public Subwave(SubWavePackage _subWavePackage, float _startupPauseSeconds) {
        Package = _subWavePackage;
        StartupPauseSeconds = _startupPauseSeconds;
    }
}

public class PlayerUnitStateMachine {
    
}

[System.Serializable]
public class Wave {
    public Subwave[] Subwaves;
    //PlaceHolder comment for effect on entire wave
    public Wave(int _amountOfWaves) {
        Subwaves = new Subwave[_amountOfWaves];
    }
}

[System.Serializable]
public class SubWavePackage {
    [SerializeField]
    public float _intervalBetweenSpawns;
    public int _amountOfUnits;
    public UnitController _unitPrefab;
    public string _splineOrder;
    public int _splinePosition;
    public float _timeToSpawnEntireSubwave() {
        return (_intervalBetweenSpawns * _amountOfUnits);
    }
    public SubWavePackage(float IntervalBetweenSpawns, int AmountOfUnits, UnitController UnitPrefab, string SplineOrder ) {
        _intervalBetweenSpawns = IntervalBetweenSpawns;
        _amountOfUnits = AmountOfUnits;
        _unitPrefab = UnitPrefab;
        _splineOrder = SplineOrder;
        _splinePosition = -1;
    }

    public SubWavePackage(float IntervalBetweenSpawns, int AmountOfUnits, UnitController UnitPrefab, string SplineOrder, int SplinePosition ) {
        _intervalBetweenSpawns = IntervalBetweenSpawns;
        _amountOfUnits = AmountOfUnits;
        _unitPrefab = UnitPrefab;
        _splineOrder = SplineOrder;
        _splinePosition = SplinePosition;
    }


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

    [SerializeField]
    public EffectableTargetBank TargetBank;
    [SerializeField]
    public RangeDetector RangeDetector;
}

public class Queueable : Component, IQueueable<Queueable> {
    public Type QueueableType {get;set;}

    public PoolObjectQueue<Queueable> QueuePool {get;set;}

    public void OnEnqueue() {}
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

    



