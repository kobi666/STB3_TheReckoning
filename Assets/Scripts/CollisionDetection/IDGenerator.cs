using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IDGenerator
{
    public static int IDcounter = 1;

    public static int GetID()
    {
        IDcounter++;
        return IDcounter - 1;
    }
}