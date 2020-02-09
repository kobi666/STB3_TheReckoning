using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class SubWave {
    [SerializeField]
    public float _intervalBetweenSpawns;
    public int _amountOfUnits;
    public GameObject _unitPrefab;
    public string _splineOrder;
    public int _splinePosition;
    public SubWave(float IntervalBetweenSpawns, int AmountOfUnits, GameObject UnitPrefab, string SplineOrder ) {
        _intervalBetweenSpawns = IntervalBetweenSpawns;
        _amountOfUnits = AmountOfUnits;
        _unitPrefab = UnitPrefab;
        _splineOrder = SplineOrder;
        _splinePosition = -1;
    }

    public SubWave(float IntervalBetweenSpawns, int AmountOfUnits, GameObject UnitPrefab, string SplineOrder, int SplinePosition ) {
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

    



