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
    UnitState inDirectBattle;
    public UnitState InDirectBattle {get => inDirectBattle;}
    UnitState frozen;
    public UnitState Frozen {get => frozen;}
    UnitState _default;
    public UnitState Default {get => _default;}
    UnitState initialState;
    public UnitState InitialState {get => initialState;}
    UnitState berserk;
    public UnitState Berserk {get => berserk;}
    UnitState postBattle;
    public UnitState PostBattle {get => postBattle;}

    UnitState joinBattle;
    public UnitState JoinBattle {get => joinBattle;}

    public NormalUnitStates (MonoBehaviour mono) {

        berserk = new UnitState(false, "Berserk", mono, Color.red);
        initialState = new UnitState(false, "InitialState", mono, Color.black);
        _default = new UnitState(false, "Default", mono, Color.green);
        frozen = new UnitState(false, "Frozen", mono, Color.blue);
        death = new UnitState(true, "Death", mono, Color.black);
        preBattle = new UnitState(false, "PreBattle", mono, Color.magenta);
        inDirectBattle = new UnitState(false, "InDirectBattle", mono, Color.red);
        joinBattle = new UnitState(false, "JoinBattle", mono, Color.yellow);
        postBattle = new UnitState(false, "PostBattle", mono, Color.cyan);

    }
   
}
