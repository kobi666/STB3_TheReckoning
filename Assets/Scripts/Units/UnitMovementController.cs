﻿using UnityEngine;

public class UnitMovementController : MonoBehaviour
{
    public Transform UnitTransform;
    public GenericUnitController GenericUnitController;
    public float MovementSpeed = 1;

    public void MoveTowardsTarget(Vector2? targetPos)
    {
        GenericUnitController.FlipDirection((Vector2)targetPos);
        UnitTransform.position = Vector2.MoveTowards(UnitTransform.position, (Vector2)targetPos,
        MovementSpeed * StaticObjects.DeltaGameTime);
    }

    
}

