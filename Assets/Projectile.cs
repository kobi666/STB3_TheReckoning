using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    UnitController target;
    public UnitController Target { get => target ; set { target = value;}}

}
