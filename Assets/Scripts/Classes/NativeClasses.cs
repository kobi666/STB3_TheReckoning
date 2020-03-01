using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
public class TowerAndPosition {
    GameObject towerGO;
    Vector2 towerPosition;
    public GameObject TowerGO {
        get => towerGO;
        set {
            if (value == null) {
                Debug.Log("Tower set to null");
            }
            towerGO = value;
            }
    
        }
    public Vector2 TowerPosition {
        get => towerPosition;
        set {
            towerPosition = value;
        }
    }
    public TowerAndPosition(GameObject _towerGO) {
        TowerGO = _towerGO;
        TowerPosition = (Vector2)_towerGO.transform.position;
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

    



