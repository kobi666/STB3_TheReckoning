using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-11)]
public class MyGameObject : MonoBehaviour,IhasGameObjectID
{
    public int gameObjectID;

    protected void Awake()
    {
        if (GameObjectID == 0) {
        gameObjectID = IDGenerator.GetGameObjectID();
        }
    }

    public int GameObjectID
    {
        get
        {
            if (gameObjectID == 0)
            {
                gameObjectID = IDGenerator.GetGameObjectID();
            }

            return gameObjectID;
        }
    }
}

public interface IhasGameObjectID
{
    int GameObjectID { get; }
}
