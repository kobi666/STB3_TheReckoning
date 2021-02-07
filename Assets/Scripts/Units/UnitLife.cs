using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class UnitLifeManager 
{
    public bool UnitInVaulenrable = false;
    public event Action<int> hp_changed;
    public event Action<int> damageTaken;
    
    public void HP_changed(int _hp) {
        if (hp_changed != null) {
            hp_changed.Invoke(HP);
        }
    }
    
    [HideInInspector]
    public int InitialHP;
    public UnitLifeManager(int hp, int armor, int special_armor, string tag, string name) {
        HP = hp;
        Armor = armor;
        SpecialArmor = special_armor;
    }
    public UnitLifeManager()
    {
        HP = InitialHP;
    }
    public event Action onUnitDeath;
    public void OnUnitDeath() {
        if (onUnitDeath != null) {
            onUnitDeath.Invoke();
        }
    }
    
    [SerializeField]
    int CurrentHP;


    public void Init()
    {
        
    }
    
    public int HP {get => CurrentHP ; set {
        if (value <= 0) {
            CurrentHP = value;
            HP_changed(value);
            OnUnitDeath();
        }
        else {
            CurrentHP = value;
            HP_changed(value);
        }
    }}

    [SerializeField]
    int _armor;
    public int Armor {get => _armor ; set {
        _armor = value;
    }}

    [SerializeField]
    int _specialArmor;
    public int SpecialArmor {get => _specialArmor ; set {
        _specialArmor = value;
    }}
    int _fireResistence;
    int _poisonResistence;
    public void DamageToUnit(int _damage, Damage_Type _damageType) {
        if (!UnitInVaulenrable) {
        int damageDelta = 0;
        switch (_damageType.DamageType)
        {
            case "normal":
                if ((_damage - Armor) <= 1) {
                    damageDelta = 1;
                    break;
                }
                else {
                    damageDelta = (_damage - Armor);
                    break;
                }
            case "special":
                if ((_damage - (Armor / 2) - SpecialArmor) <= 1) {
                    damageDelta = 1;
                    break;
                }
                else {
                    damageDelta = (_damage - (Armor /2) - SpecialArmor);
                    break;
                }
        }
        HP -= damageDelta;
        damageTaken?.Invoke(damageDelta);
        }
        
    }

    public void DamageToUnit(int _damage) {
        int damageDelta = 0;
                if ((_damage - Armor) <= 1) {
                    damageDelta = 1;
                }
                else {
                    damageDelta = (_damage - Armor);
                }
        HP -= damageDelta;
        damageTaken?.Invoke(damageDelta);
    }
}
