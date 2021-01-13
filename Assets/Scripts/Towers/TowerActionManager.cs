using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class TowerActionManager : MonoBehaviour
{
    private TowerSlotController parentSlotController;
    private TowerController parentTowerController;
    public TowerActions Actions = new TowerActions();

    public void initActionManager(TowerSlotController tsc)
    {
        parentSlotController = tsc;
        parentTowerController = tsc.ChildTower;
        Actions.initActions(tsc);
    }
}
