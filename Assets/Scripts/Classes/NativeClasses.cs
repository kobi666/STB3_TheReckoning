using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[System.Serializable]
public class FloatMultiplier {

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

public class TowerSlotAction {
    [SerializeField]
    public string ActionDescription;
    [SerializeField]
    public Sprite ButtonSprite;
    [SerializeField]
    public event Action ActionFunctions;
    public TowerSlotAction(string actionDescription, Sprite buttonSprite) {
        ActionDescription = actionDescription;
        ButtonSprite = buttonSprite;
    }
    public void ExecuteFunction() {
        ActionFunctions?.Invoke();
    }
}




[System.Serializable]
public class TowerPositionQuery {
    public Vector2 ThisTower;
    public Vector2 TargetTower;
    public float Assistingfloat1;
    public float Assistingfloat2;

    public TowerPositionQuery(Vector2 thisTower, Vector2 targetTower, float assistingFloat) {
        ThisTower = thisTower;
        TargetTower = targetTower;
        Assistingfloat1 = assistingFloat;
    }

    public TowerPositionQuery(Vector2 thisTower, Vector2 targetTower, float assistingFloat1, float assistingFloat2) {
        ThisTower = thisTower;
        TargetTower = targetTower;
        Assistingfloat1 = assistingFloat1;
        Assistingfloat2 = assistingFloat2;
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

public abstract class State {

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
    public GameObject _unitPrefab;
    public string _splineOrder;
    public int _splinePosition;
    public float _timeToSpawnEntireSubwave() {
        return (_intervalBetweenSpawns * _amountOfUnits);
    }
    public SubWavePackage(float IntervalBetweenSpawns, int AmountOfUnits, GameObject UnitPrefab, string SplineOrder ) {
        _intervalBetweenSpawns = IntervalBetweenSpawns;
        _amountOfUnits = AmountOfUnits;
        _unitPrefab = UnitPrefab;
        _splineOrder = SplineOrder;
        _splinePosition = -1;
    }

    public SubWavePackage(float IntervalBetweenSpawns, int AmountOfUnits, GameObject UnitPrefab, string SplineOrder, int SplinePosition ) {
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

    



