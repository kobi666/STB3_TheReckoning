using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class TowerActions
{

    public Dictionary<ButtonDirectionsNames, TowerAction> Actions
    {
        get
        {
            if (!actionInitialized)
            {
                initActions(parentTowerSlotController);
            }

            return actions;
        }
    }
    
    
    public Dictionary<ButtonDirectionsNames, TowerAction> actions =
        new Dictionary<ButtonDirectionsNames, TowerAction>();


     private TowerSlotController parentTowerSlotController;
    [TypeFilter("GetTowerActions")][SerializeReference]
    public TowerAction North = new NullAction();
    [TypeFilter("GetTowerActions")][SerializeReference]
    public TowerAction East = new NullAction();
    [TypeFilter("GetTowerActions")][SerializeReference]
    public TowerAction South = new NullAction();
    [TypeFilter("GetTowerActions")][SerializeReference]
    public TowerAction West = new NullAction();

    public TowerAction[] actionsByButtonName
    {
        get
        {
            return new TowerAction[]
            {
                North,
                East,
                South,
                West
            };
        }
    }
    
    [Button]
    void north()
    {
        North.ExecAction();
    }
    [Button]
    void east()
    {
        East.ExecAction();
    }
    [Button]
    void south()
    {
        South.ExecAction();
    }
    
    [Button]
    void west()
    {
        West.ExecAction();
    }


    public bool actionInitialized = false;
    public void initActions(TowerSlotController tsc)
    {
        if (actionInitialized == false) {
        actions.Add(ButtonDirectionsNames.North,North);
        actions.Add(ButtonDirectionsNames.East,East);
        actions.Add(ButtonDirectionsNames.South,South);
        actions.Add(ButtonDirectionsNames.West,West);
        parentTowerSlotController = tsc;
        North?.InitAction(parentTowerSlotController);
        East?.InitAction(parentTowerSlotController);
        South?.InitAction(parentTowerSlotController);
        West?.InitAction(parentTowerSlotController);
        actionInitialized = true;
        }
    }
    
    private static IEnumerable<Type> GetTowerActions()
    {
        var q = typeof(TowerAction).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(TowerAction)));
        
        return q;
    }
}
