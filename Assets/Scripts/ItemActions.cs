using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Sirenix.OdinInspector;

public class ItemActions : MonoBehaviour
{
    public Dictionary<ButtonDirectionsNames, LootAction> Actions
    {
        get
        {
            if (!actionsInitialized)
            {
                initActions(parentLootObjectSlot);
            }

            return actions;
        }
    }
    
    
    public Dictionary<ButtonDirectionsNames, LootAction> actions =
        new Dictionary<ButtonDirectionsNames, LootAction>();


    private LootObjectSlot parentLootObjectSlot;
    [TypeFilter("GetTowerActions")][SerializeReference]
    public LootAction[] ActionsByIndex = new LootAction[4];

   
    
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
    public void initActions(LootObjectSlot lootObjectSlot)
    {
        
        if (actionsInitialized == false) { 
            actions.Clear();
            actions.Add(ButtonDirectionsNames.North,ActionsByIndex[0]);
            actions.Add(ButtonDirectionsNames.East,ActionsByIndex[1]);
            actions.Add(ButtonDirectionsNames.South,ActionsByIndex[2]);
            actions.Add(ButtonDirectionsNames.West,ActionsByIndex[3]);
            parentLootObjectSlot = lootObjectSlot;
            if (parentLootObjectSlot != null) {
                for (int i = 0; i < ActionsByIndex.Length; i++)
                {
                    ActionsByIndex[i].InitAction(lootObjectSlot,i);
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
