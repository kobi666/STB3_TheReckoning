﻿using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UnitMovementController : MonoBehaviour
{
    public Transform UnitTransform;
    public float MovementSpeed = 1;

    public void MoveTowardsTarget(Vector2 targetPos)
    {
        UnitTransform.position = Vector2.MoveTowards(UnitTransform.position, targetPos,
            MovementSpeed * StaticObjects.DeltaGameTime);
    }
}


