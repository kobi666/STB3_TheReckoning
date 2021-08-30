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
            if (!actionsInitialized)
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
    public TowerAction[] ActionsByIndex = new TowerAction[4];

   
    
    [Button]
    void north()
    {
        ActionsByIndex[0].ExecAction();
    }
    [Button]
    void east()
    {
        ActionsByIndex[1].ExecAction();
    }
    [Button]
    void south()
    {
        ActionsByIndex[2].ExecAction();
    }
    
    [Button]
    void west()
    {
        ActionsByIndex[3].ExecAction();
    }


    public bool actionsInitialized = false;
    public void initActions(TowerSlotController tsc)
    {
        
        if (actionsInitialized == false) { 
            actions.Clear();
            actions.Add(ButtonDirectionsNames.North,ActionsByIndex[0]);
            actions.Add(ButtonDirectionsNames.East,ActionsByIndex[1]);
            actions.Add(ButtonDirectionsNames.South,ActionsByIndex[2]);
            actions.Add(ButtonDirectionsNames.West,ActionsByIndex[3]);
            parentTowerSlotController = tsc;
            if (parentTowerSlotController != null) {
                for (int i = 0; i < ActionsByIndex.Length; i++)
                {
                    ActionsByIndex[i].InitAction(tsc,i);
                }
                actionsInitialized = true;
            }
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
