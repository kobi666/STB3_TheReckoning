using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Unity.Entities;

[Serializable]
public class TestEntity : MonoBehaviour
{
    [ShowInInspector]
    public Entity myEntity;
    void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        myEntity = entityManager.CreateEntity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
