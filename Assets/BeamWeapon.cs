using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class BeamWeapon : WeaponController
{
    public LineRenderer LineRenderer;
    
    
    
    
    

    protected void Awake()
    {
        base.Awake();
        LineRenderer = GetComponent<LineRenderer>() ?? null;
    }
    protected void Start()
    {
        base.Start();
    }

}
