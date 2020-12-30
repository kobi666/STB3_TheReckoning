using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class TowerActionManager : MonoBehaviour
{
    
    private TowerController TowerController;
    
    protected void Awake()
    {
        TowerController = GetComponent<TowerController>();
    }


    [TypeFilter("GetTowerActions")][SerializeReference] public TowerAction testAction;
    /*[TypeFilter("GetTowerActions")] public TowerAction NorthAction;
    [TypeFilter("GetTowerActions")] public TowerAction EastAction;
    [TypeFilter("GetTowerActions")] public UpgradeToNewTower SouthAction;
    [TypeFilter("GetTowerActions")]
    public TowerAction WestAction = new TowerAction();*/


    private static IEnumerable<Type>  GetTowerActions()
    {
        var q = typeof(TowerAction).Assembly.GetTypes()
                .Where(x => !x.IsAbstract) // Excludes BaseClass
                .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
                .Where(x => x.IsSubclassOf(typeof(TowerAction)));
        
        return q;
    }


    private ValueDropdownList<TowerAction> towerActions = new ValueDropdownList<TowerAction>()
    {
        {"item1", new TowerAction()},
        {"item2", new UpgradeToNewTower()}
    };



}
