using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class UnitLifeManager
{
    public UnitLifeManager(int hp, int armor, int special_armor) {
        HP = hp;
        Armor = armor;
        SpecialArmor = special_armor;
    }
    public event Action _onUnitDeath;
    public void OnUnitDeath() {
        if (_onUnitDeath != null) {
            _onUnitDeath.Invoke();
        }
    }

   
    int _HP;
    
    public int HP {get => _HP ; set {
        if (value <= 0) {
            _HP = value;
            OnUnitDeath();
        }
        else {
            _HP = value;
        }
    }}
    int _armor;
    public int Armor {get => _armor ; set {
        _armor = value;
    }}
    int _specialArmor;
    public int SpecialArmor {get => _specialArmor ; set {
        _specialArmor = value;
    }}
    int _fireResistence;
    int _poisonResistence;
    public void DamageToUnit(int _damage, Damage_Type _damageType) {
        switch (_damageType.DamageType)
        {
            case "normal":
                if ((_damage - Armor) <= 1) {
                    HP -= 1;
                    break;
                }
                else {
                    HP -= (_damage - Armor);
                    break;
                }
            case "special":
                if ((_damage - (Armor / 2) - SpecialArmor) <= 1) {
                    HP -= 1;
                    break;
                }
                else {
                    HP -= (_damage - (Armor /2) - SpecialArmor);
                    break;
                }
        }

    }
}
