using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class PlacementSelectionMenu : MonoBehaviour
{
    [Required]
    public TowerSelectionDisplay InitialSlot;

    public static PlacementSelectionMenu instance;

    private void Awake()
    {
        instance = this;
    }
}