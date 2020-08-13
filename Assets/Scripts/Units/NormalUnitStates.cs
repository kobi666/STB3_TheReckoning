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
    ObjectState death;
    public ObjectState Death {get => death;}
    ObjectState preBattle;
    public ObjectState PreBattle {get => preBattle;}
    ObjectState inDirectBattle;
    public ObjectState InDirectBattle {get => inDirectBattle;}
    ObjectState frozen;
    public ObjectState Frozen {get => frozen;}
    ObjectState _default;
    public ObjectState Default {get => _default;}
    ObjectState initialState;
    public ObjectState InitialState {get => initialState;}
    ObjectState berserk;
    public ObjectState Berserk {get => berserk;}
    ObjectState postBattle;
    public ObjectState PostBattle {get => postBattle;}

    ObjectState joinBattle;
    public ObjectState JoinBattle {get => joinBattle;}

    public NormalUnitStates (MonoBehaviour mono) {

        berserk = new ObjectState(false, "Berserk", mono, Color.red);
        initialState = new ObjectState(false, "InitialState", mono, Color.black);
        _default = new ObjectState(false, "Default", mono, Color.green);
        frozen = new ObjectState(false, "Frozen", mono, Color.blue);
        death = new ObjectState(true, "Death", mono, Color.black);
        preBattle = new ObjectState(false, "PreBattle", mono, Color.magenta);
        inDirectBattle = new ObjectState(false, "InDirectBattle", mono, Color.red);
        joinBattle = new ObjectState(false, "JoinBattle", mono, Color.yellow);
        postBattle = new ObjectState(false, "PostBattle", mono, Color.cyan);

    }
   
}
