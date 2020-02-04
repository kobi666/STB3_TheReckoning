using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class  DamageRange {
    [SerializeField]
    public int min;
    [SerializeField]
    public int max;
}

[System.Serializable]
public class HitPointManager {


    int _HP;
    int _overkill;
    public int HP {get => _HP ; set {
        if (_HP > 0) {
            _HP = value;
        }
        if (_HP < 0) {
            _overkill = _HP;
            Debug.Log("Overkilled by " + _overkill);
        }
    }}

    int _Armor;
    public int Armor {get => _Armor ; set {
        _Armor = value;
    }}
    }
    



