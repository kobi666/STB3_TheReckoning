using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTypes {

    public static UnitType NormalEnemy(MonoBehaviour _monobehavior) {
        return new UnitType(UnitStates.NormalEnemy(), _monobehavior);
    }

    public static UnitType NormalPlayerUnit(MonoBehaviour _monobehavior) {
        return new UnitType(UnitStates.NormalPlayerUnit(), _monobehavior);
    }
    

    
   }


   
   

   

