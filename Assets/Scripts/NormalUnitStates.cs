using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalUnitStates : States
{
    // states[0] = new StringAndBool("Death", true);
    //     states[1] = new StringAndBool("Default");
    //     states[2] = new StringAndBool("PreBattle");
    //     states[3] = new StringAndBool("InBattle");
    //     states[4] = new StringAndBool("Frozen");
    //     states[5] = new StringAndBool("Berserk");
    UnitState death;
    public UnitState Death {get => death;}
    UnitState preBattle;
    public UnitState PreBattle {get => preBattle;}
    UnitState inBattle;
    public UnitState InBattle {get => inBattle;}
    UnitState frozen;
    public UnitState Frozen {get => frozen;}
    UnitState _default;
    public UnitState Default {get => _default;}
    UnitState berserk;
    public UnitState Berserk {get => berserk;}
    UnitState postBattle;
    public UnitState PostBattle {get => postBattle;}

    public NormalUnitStates (MonoBehaviour mono) {
        berserk = new UnitState(false, "Berserk", mono);
        _default = new UnitState(false, "Default", mono);
        frozen = new UnitState(false, "Frozen", mono);
        death = new UnitState(true, "Death", mono);
        preBattle = new UnitState(false, "PreBattle", mono);
        inBattle = new UnitState(false, "InBattle", mono);
        postBattle = new UnitState(false, "PostBattle", mono);
    }
   
}
