using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class UnitData
{
    // Start is called before the first frame update
    public bool CanBeStoppedByOtherUnit;
    public int AttackRate;
    public DamageRange DamageRange;
    public int HP;
    public int Armor;
    public int SpecialArmor;
    public int speed;
    public Vector2 SetPosition;
    public GameObject Target;
}
