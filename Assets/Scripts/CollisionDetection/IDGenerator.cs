using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IDGenerator
{
    public static int CollisionIDcounter = 1;
    public static int GameIDCounter = 1;

    public static int GetCollisionID()
    {
        CollisionIDcounter++;
        return CollisionIDcounter - 1;
    }
    
    public static int GetGameObjectID()
    {
        GameIDCounter++;
        return GameIDCounter - 1;
    }
}