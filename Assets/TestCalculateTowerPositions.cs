using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCalculateTowerPositions : MonoBehaviour
{
    private PlayerInput PInput;

    private TowerSlotController[] TowerSlotControllers;

    private event Action onRecalculate;

    public void OnRecalculate()
    {
        onRecalculate?.Invoke();
    }
    
    private void OnEnable()
    {
        PInput.Enable();
    }


    private void Awake()
    {
        PInput = new PlayerInput();
    }

    private void Start()
    {
        TowerSlotControllers = GetComponentsInChildren<TowerSlotController>();

        foreach (var VARIABLE in TowerSlotControllers)
        {
            onRecalculate += VARIABLE.CalculateAdjecentTowers;
        }
        
        onRecalculate += SelectorTest2.instance.ShowIndicators;

        PInput.TestButtons.J.performed += ctx => OnRecalculate();
        PInput.TestButtons.J.performed += ctx => Debug.LogWarning("J WAS PRESSED" + TowerSlotControllers.Length);
        ;

    }
}
