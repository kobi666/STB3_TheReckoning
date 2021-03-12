using System;
using UnityEngine;

public class UnitMovementController : MonoBehaviour
{
    public Transform UnitTransform;
    public GenericUnitController GenericUnitController;
    public float MovementSpeed = 1;
    private Rigidbody2D Rigidbody2D;

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void MoveTowardsTarget(Vector2? targetPos)
    {
        GenericUnitController.FlipDirection((Vector2)targetPos);
        
        Rigidbody2D.MovePosition(Vector2.MoveTowards(UnitTransform.position, (Vector2)targetPos,
        MovementSpeed * StaticObjects.DeltaGameTime));
        //UnitTransform.position = Vector2.MoveTowards(UnitTransform.position, (Vector2)targetPos,
        //MovementSpeed * StaticObjects.DeltaGameTime);
    }

    
}


