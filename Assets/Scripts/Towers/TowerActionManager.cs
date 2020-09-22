using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerActionManager : MonoBehaviour
{
    private TowerController TowerController;
    
    
    
    
    protected void Awake()
    {
        TowerController = GetComponent<TowerController>();
    }
}
