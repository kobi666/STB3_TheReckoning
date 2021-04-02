using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[DefaultExecutionOrder(-11)]
public class MyGameObject : MonoBehaviour,IhasGameObjectID
{
    [ShowInInspector]
    private int myGameObjectID = 0;
    
    public int MyGameObjectID
    {
        get
        {
            if (myGameObjectID == 0)
            {
                myGameObjectID = IDGenerator.Instance.GetGameObjectID();
            }

            return myGameObjectID;
        }
    }
}

public interface IhasGameObjectID
{
    int MyGameObjectID { get; }
}
