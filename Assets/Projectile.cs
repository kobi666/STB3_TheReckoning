using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    UnitController targetUnit;
    public UnitController TargetUnit { get => targetUnit ; set { targetUnit = value;}}

    Vector2 targetPosition;
    public Vector2 TargetPosition { get => targetPosition; set {targetPosition = value;}}

}
