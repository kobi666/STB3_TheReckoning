using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsManager : MonoBehaviour
{
    public TowerBuildData[] TowersLevel1 = new TowerBuildData[4];
    public TowerBuildData[] TowersLevel2 = new TowerBuildData[4];
    public TowerBuildData[] TowersLevel3 = new TowerBuildData[4];

    public TowerBuildData GetTowerBuildData(int towerLevel, int towerIndex)
    {
        TowerBuildData tbd = null;
        TowerBuildData[] level = null;
        switch (towerLevel)
        {
            case 0:
            {
                level = TowersLevel1;
                break;
            }
            case 1:
            {
                level = TowersLevel2;
                break;
            }
            case 2:
            {
                level = TowersLevel3;
                break;
            }
        }

        if (level != null)
        {
            tbd = level[towerIndex];
        }

        return tbd;
    }
    
    public void SetTowerBuildData(int towerLevel, int towerIndex, TowerController NewTowerPrefab, int NewTowerCost)
    {
        
        TowerBuildData[] level = null;
        switch (towerLevel)
        {
            case 0:
            {
                level = TowersLevel1;
                break;
            }
            case 1:
            {
                level = TowersLevel2;
                break;
            }
            case 2:
            {
                level = TowersLevel3;
                break;
            }
        }

        if (level != null)
        {
            level[towerIndex] = new TowerBuildData
            {
                TowerPrefab = NewTowerPrefab,
                TowerCost = NewTowerCost
            };
        }

        
    }
}


public class TowerBuildData
{
    public TowerController TowerPrefab;
    public int TowerCost;
}
