using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class UnitStates 
{
    
    public static StringAndBool[] NormalEnemy() {
        StringAndBool[] states = new StringAndBool[7];
        states[0] = new StringAndBool("Death", true);
        states[1] = new StringAndBool("Default");
        states[2] = new StringAndBool("PreBattle");
        states[3] = new StringAndBool("InBattle");
        states[4] = new StringAndBool("Frozen");
        states[5] = new StringAndBool("Berserk");
        states[6] = new StringAndBool("PostBattle");
        return states;
    }

    public static StringAndBool[] NormalPlayerUnit() {
        StringAndBool[] states = new StringAndBool[6];
        states[0] = new StringAndBool("Death", true);
        states[1] = new StringAndBool("Default");
        states[2] = new StringAndBool("PreBattle");
        states[3] = new StringAndBool("InBattle");
        states[4] = new StringAndBool("Frozen");
        states[5] = new StringAndBool("Berserk");
        states[6] = new StringAndBool("PostBattle");
        return states;
    }

}
