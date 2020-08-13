using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    [SerializeField]
    float movementSpeed;
    public float MovementSpeed {get => movementSpeed ; set {movementSpeed = value;}}
    
}
