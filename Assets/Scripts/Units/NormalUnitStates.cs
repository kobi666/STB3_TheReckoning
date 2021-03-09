using UnityEngine;

public class NormalUnitStates : States
{

    // states[0] = new StringAndBool("Death", true);
    //     states[1] = new StringAndBool("Default");
    //     states[2] = new StringAndBool("PreBattle");
    //     states[3] = new StringAndBool("InBattle");
    //     states[4] = new StringAndBool("Frozen");
    //     states[5] = new StringAndBool("Berserk");
    ObjectStateLegacy death;
    public ObjectStateLegacy Death {get => death;}
    ObjectStateLegacy preBattle;
    public ObjectStateLegacy PreBattle {get => preBattle;}
    ObjectStateLegacy inDirectBattle;
    public ObjectStateLegacy InDirectBattle {get => inDirectBattle;}
    ObjectStateLegacy frozen;
    public ObjectStateLegacy Frozen {get => frozen;}
    ObjectStateLegacy _default;
    public ObjectStateLegacy Default {get => _default;}
    ObjectStateLegacy m_InitialStateLegacy;
    public ObjectStateLegacy InitialStateLegacy {get => m_InitialStateLegacy;}
    ObjectStateLegacy berserk;
    public ObjectStateLegacy Berserk {get => berserk;}
    ObjectStateLegacy postBattle;
    public ObjectStateLegacy PostBattle {get => postBattle;}

    ObjectStateLegacy joinBattle;
    public ObjectStateLegacy JoinBattle {get => joinBattle;}

    public NormalUnitStates (MonoBehaviour mono) {

        berserk = new ObjectStateLegacy(false, "Berserk", mono, Color.red);
        m_InitialStateLegacy = new ObjectStateLegacy(false, "InitialState", mono, Color.black);
        _default = new ObjectStateLegacy(false, "Default", mono, Color.green);
        frozen = new ObjectStateLegacy(false, "Frozen", mono, Color.blue);
        death = new ObjectStateLegacy(true, "Death", mono, Color.black);
        preBattle = new ObjectStateLegacy(false, "PreBattle", mono, Color.magenta);
        inDirectBattle = new ObjectStateLegacy(false, "InDirectBattle", mono, Color.red);
        joinBattle = new ObjectStateLegacy(false, "JoinBattle", mono, Color.yellow);
        postBattle = new ObjectStateLegacy(false, "PostBattle", mono, Color.cyan);

    }
   
}
