using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlotParentManager : MonoBehaviour
{
    public static TowerSlotParentManager instance;
    public TowerSlotController[] TowerSlotControllers;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        TowerSlotControllers = GetComponentsInChildren<TowerSlotController>();
    }

    // Update is called once per frame
    
}
