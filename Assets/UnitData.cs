using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class UnitData
{
    // Start is called before the first frame update
    public bool CanBeStoppedByOtherUnit;
    public Dictionary<string, GameObject> TargetsInRange;
    public int AttackRate;
    public DamageRange DamageRange;
    public int HP;
    public int Armor;
    public int SpecialArmor;
    public int speed;
    public Vector2 SetPosition;
    public GameObject Target;
    public UnitType unitType;

    public Damage_Type damageType = new Damage_Type("normal");
}
