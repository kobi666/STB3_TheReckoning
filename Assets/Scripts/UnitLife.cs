using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class UnitLifeManager
{
    public event Action hp_changed;
    public void HP_changed() {
        if (hp_changed != null) {
            hp_changed.Invoke();
        }
    }
    public UnitLifeManager(int hp, int armor, int special_armor) {
        HP = hp;
        Armor = armor;
        SpecialArmor = special_armor;
    }
    public event Action onUnitDeath;
    public void OnUnitDeath() {
        if (onUnitDeath != null) {
            onUnitDeath.Invoke();
            
        }
    }

   
    int _HP;
    
    
    public int HP {get => _HP ; set {
        if (value <= 0) {
            _HP = value;
            HP_changed();
            OnUnitDeath();
        }
        else {
            _HP = value;
            HP_changed();
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
