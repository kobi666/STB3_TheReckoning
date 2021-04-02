using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDGenerator : MonoBehaviour
{
    public static IDGenerator Instance;

    private void Awake()
    {
        Instance = this;
    }

    public  int CollisionIDcounter = 1;
    public  int GameIDCounter = 1;

    public  int GetCollisionID()
    {
        CollisionIDcounter++;
        return CollisionIDcounter - 1;
    }
    
    public  int GetGameObjectID()
    {
        GameIDCounter++;
        return GameIDCounter - 1;
    }
}