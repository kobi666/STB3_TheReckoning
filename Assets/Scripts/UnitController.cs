using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    // Start is called before the first frame update
    public UnitType unitType;
    public NormalUnitStates states {get => unitType.states;}
}
