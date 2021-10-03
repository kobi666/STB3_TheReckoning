using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

[System.Serializable]
public class PlayerTowersManager : MonoBehaviour
{
    //public TowerController[] PlayerTowersByIndex = new TowerController[4];

    public static PlayerTowersManager instance;

    private void Awake()
    {
        instance = this;
    }

    public bool SlotAvailable()
    {
        foreach (var bd in TowerUtils.ButtonDirectionsNamesArray)
        {
            if (GetTowerByButtonName(bd) == null)
            {
                return true;
            }
        }

        return false;
    }


    public List<TowerComponent> GetCurrentTowersComponents()
    {
        List<TowerComponent> components = new List<TowerComponent>();
        foreach (ButtonDirectionsNames bdn in TowerUtils.ButtonDirectionsNamesArray)
        {
            var t = GetTowerByButtonName(bdn);
            if (t != null) {
            var cs = GetTowerByButtonName(bdn).TowerComponents;
            components.AddRange(cs);
            }
        }

        return components;
    }
    
    
    
    
    public ReactiveProperty<TowerController> North;
    public ReactiveProperty<TowerController> East;
    public ReactiveProperty<TowerController> South;
    public ReactiveProperty<TowerController> West;
    
    public TowerController GetTowerByButtonName(ButtonDirectionsNames buttonDirectionsName)
    {
        
        if (buttonDirectionsName == ButtonDirectionsNames.North)
        {
            return North.Value;
        }

        if (buttonDirectionsName == ButtonDirectionsNames.East)
        {
            return East.Value;
        }

        if (buttonDirectionsName == ButtonDirectionsNames.South)
        {
            return South.Value;
        }

        if (buttonDirectionsName == ButtonDirectionsNames.West)
        {
            return West.Value;
        }

        return null;
    }
    
    public ReactiveProperty<TowerController> GetTowerReactivePropertyByButtonName(ButtonDirectionsNames buttonDirectionsName)
    {
        if (buttonDirectionsName == ButtonDirectionsNames.North)
        {
            return North;
        }

        if (buttonDirectionsName == ButtonDirectionsNames.East)
        {
            return East;
        }

        if (buttonDirectionsName == ButtonDirectionsNames.South)
        {
            return South;
        }

        if (buttonDirectionsName == ButtonDirectionsNames.West)
        {
            return West;
        }

        return null;
    }

    public void SetTowerByButtonName(ButtonDirectionsNames buttonDirectionsName, TowerController towerController)
    {
        if (buttonDirectionsName == ButtonDirectionsNames.North)
        {
            North.Value = towerController;
        }

        if (buttonDirectionsName == ButtonDirectionsNames.East)
        {
            East.Value = towerController;
        }

        if (buttonDirectionsName == ButtonDirectionsNames.South)
        {
            South.Value = towerController;
        }

        if (buttonDirectionsName == ButtonDirectionsNames.West)
        {
            West.Value = towerController;
        }
    }

    int indexByDirection(ButtonDirectionsNames buttonDirectionsName)
    {
        if (buttonDirectionsName == ButtonDirectionsNames.North)
        {
            return 0;
        }
        if (buttonDirectionsName == ButtonDirectionsNames.East)
        {
            return 1;
        }
        if (buttonDirectionsName == ButtonDirectionsNames.South)
        {
            return 2;
        }
        if (buttonDirectionsName == ButtonDirectionsNames.West)
        {
            return 3;
        }

        return -1;
    }
    
    
    
    [ShowInInspector]
    public Dictionary<int, TowerController> ActiveTowers = new Dictionary<int, TowerController>();

    
}



