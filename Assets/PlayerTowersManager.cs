using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class PlayerTowersManager : MonoBehaviour
{
    public TowerController[] PlayerTowersByIndex = new TowerController[4];
    
    
    public TowerController GetTowerByButtonName(ButtonDirectionsNames buttonDirectionsName)
    {
        int index = indexByDirection(buttonDirectionsName);
        if (index == -1)
        {
            return null;
        }
        else
        {
            return PlayerTowersByIndex[index];
        }
    }

    public void SetTowerByButtonName(ButtonDirectionsNames buttonDirectionsName, TowerController towerController)
    {
        int index = indexByDirection(buttonDirectionsName);
        if (index == -1)
        {
            Debug.LogWarning("WTF");
            return;
        }
        else
        {
            PlayerTowersByIndex[index] = towerController;
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



