using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UnitTypes {

    public static UnitState[] normal(MonoBehaviour _monobehavior) {
       UnitState[] states = new UnitState[5];
       states[0] = new UnitState(false, "Default", _monobehavior);
       states[1] = new UnitState(true, "Death", _monobehavior);
       states[2] = new UnitState(false, "InBattle", _monobehavior);
       states[3] = new UnitState(false, "PreBattle", _monobehavior);
       return states;
   }

   
   

   
}
